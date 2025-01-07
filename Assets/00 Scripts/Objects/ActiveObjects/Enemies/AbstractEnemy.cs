using UnityEngine;

[RequireComponent(typeof(CharStats), typeof(AttackController))]
abstract public class AbstractEnemy : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("The character controller for the enemy")]
    [SerializeField] private CharacterController _characterController;
    [Tooltip("The target for the enemy")]
    [SerializeField] private HealthController _target;
    [Tooltip("The attack controller for the enemy")]
    [SerializeField] private AttackController _attackController;
    [Tooltip("The health bar for the enemy")]
    [SerializeField] private HealthBar _healthBar;

    [Header("Stats")]
    [Tooltip("The speed stat reference")]
    [SerializeField] protected Stat _attackSpeedStat;
    [Tooltip("The speed at which the enemy attacks")]
    [SerializeField] protected float attackSpeedValue;

    [Header("AI Settings")]
    [Tooltip("The range outside of its own mesh that it can attack in")]
    [SerializeField] protected float _attackRange = 1f;
    [Tooltip("The speed at which the enemy moves")]
    [SerializeField] protected float moveSpeedValue = 1f;

    [Space(50)]
    [Tooltip("Should debug messages be shown?")]
    [SerializeField] protected bool shouldDebug = true;

    private MeshRenderer _mr;
    /// <summary>
    /// Returns the size of the mesh renderer associated with this enemy.
    /// </summary>
    /// <returns>The size of the mesh renderer as a Vector3.</returns>
    public Vector3 GetBounds() { return _mr.bounds.size; }

    private float _lastAttackTime;

    private float _totalDistance;
    public float TotalDistance { get => _totalDistance; set => _totalDistance = value; }

    virtual public void Start()
    {
        if (TryGetComponent<CharStats>(out var _charStats))
        {
            attackSpeedValue = _charStats.GetStatValue(_attackSpeedStat);
        }
        else
        {
            Debug.Log("CharStats component not found.");
        }
        if (_characterController == null) _characterController = GetComponent<CharacterController>();
        if (_target == null) _target = GameManager.Instance.Tower.GetComponent<HealthController>();
        if (_attackController == null) _attackController = GetComponent<AttackController>();

        _lastAttackTime = Time.time;

        GetMeshRenderer();
        _attackRange += _mr.bounds.extents.x;

        GameManager.Instance.EnemiesAlive++;
    }

    private void OnValidate()
    {
        GetMeshRenderer();
    }

    private void GetMeshRenderer()
    {
        if (_mr == null)
        {
            _mr = GetComponent<MeshRenderer>();
        }
        if (_mr == null)
        {
            _mr = GetComponentInChildren<MeshRenderer>();
        }
    }

    virtual public void FixedUpdate()
    {
        Behaviour();
    }

    virtual public void Behaviour()
    {
        if (_target == null) Debug.Log("No target set");

        if (CanAttack())
        {
            Attack();
            _lastAttackTime = Time.time;
        }

        Move();
        return;
    }

    /// <summary>
    /// Moves the enemy towards the tower.
    /// </summary>
    virtual public void Move()
    {
        Vector3 dir = (_target.transform.position - _characterController.transform.position).normalized * moveSpeedValue;
        if (shouldDebug) Debug.Log($"{gameObject.name} moving in direction {dir}");
        _characterController.SimpleMove(dir);
    }

    /// <summary>
    /// Attacks the target.
    /// </summary>
    virtual public void Attack()
    {
        _attackController.DoAttack(_target);
    }

    /// <summary>
    /// Checks if there are enemies in range using a sphere cast.
    /// </summary>
    /// <returns>True if there are enemies in range, otherwise false.</returns>
    protected bool IsWithinRange(HealthController _target)
    {
        if (shouldDebug) Debug.Log($"Checking range for {gameObject.name}");
        float rangeSquared = _attackRange * _attackRange;

        Vector3 objectPos = _characterController.transform.position;
        Vector3 targetPos = _target.transform.position;

        float sqrMagDistance = (objectPos - targetPos).sqrMagnitude;

        if (shouldDebug) Debug.Log($"{gameObject.name} position: {objectPos}"
                                   + $"\n{_target.name} position: {targetPos}"
                                   + $"\nCurrent distance^2 float: {sqrMagDistance}"
                                   + $"\nRange^2 float: {rangeSquared}");

        if (sqrMagDistance <= rangeSquared)
        {
            if (shouldDebug) Debug.Log($"{gameObject.name} is within range!");
            return true;
        }

        if (shouldDebug) Debug.Log($"{gameObject.name} is not yet close enough!");
        return false;
    }

    /// <summary>
    /// Determines if the enemy can attack based on the attack range and attack speed.
    /// </summary>
    /// <returns>True if the enemy can attack, otherwise false.</returns>
    protected bool CanAttack()
    {
        if (IsWithinRange(_target)) return Time.time - _lastAttackTime >= 1 / attackSpeedValue;
        else return false;
    }

    /// <summary>
    /// Sets up the health bar for the enemy.
    /// </summary>
    /// <param name="_canvas">The canvas to which the health bar will be attached.</param>
    /// <param name="_camera">The camera that the health bar will face.</param>
    public void SetupHealthBar(Canvas _canvas, Camera _camera)
    {
        if (!TryGetComponent<HealthController>(out var _healthController))
        {
            Debug.Log("Enemy doesn't have a healthcontroller component, removing healthbar");
            _healthBar.gameObject.SetActive(false);
            return;
        }

        _healthBar.transform.SetParent(_canvas.transform);
        FaceCamera _faceCamera = _healthBar.GetComponent<FaceCamera>();
        _faceCamera.Camera = _camera;
    }

    protected void OnDrawGizmos()
    {
        if (!shouldDebug) return;
        Gizmos.color = new Color(1f, 0f, 0f, .4f);
        Gizmos.DrawSphere(_mr.bounds.center, _attackRange);
    }

    private void OnDestroy()
    {
        GameManager.Instance.EnemiesAlive--;
    }
}
