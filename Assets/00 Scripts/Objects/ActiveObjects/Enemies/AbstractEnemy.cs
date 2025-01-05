using UnityEngine;

[RequireComponent(typeof(CharStats), typeof(AttackController))]
abstract public class AbstractEnemy : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private HealthController _target;
    [SerializeField] private AttackController _attackController;

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
    [SerializeField] protected bool shouldDebug = true;

    private MeshRenderer _mr;
    private float _lastAttackTime;

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

    protected void OnDrawGizmos()
    {
        if (!shouldDebug) return;
        Gizmos.color = new Color(1f,0f,0f,.4f);
        Gizmos.DrawSphere(_mr.bounds.center, _attackRange);
    }
}
