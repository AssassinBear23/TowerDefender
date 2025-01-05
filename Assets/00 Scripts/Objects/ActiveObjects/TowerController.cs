using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharStats), typeof(AttackController))]
public class TowerController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The attack controller of the tower")]
    [SerializeField] private AttackController _attackController;
    [Tooltip("The GameManager instance in the scene")]
    private GameManager _gm;
    [Tooltip("The spot from which the projectiles will launch from")]
    [SerializeField] private Transform _firePoint;

    [Header("Stats")]
    [Tooltip("The character stats of the tower")]
    [SerializeField] private CharStats _charStats;
    [Space(20)]
    [SerializeField] private Stat _attackSpeedStat;
    [Space(10)]
    [SerializeField] private float _attackSpeedValue;

    [Space(20)]
    [Header("Attack Settings")]
    [Tooltip("The characters that are within range")]
    [SerializeField] List<HealthController> _enemiesInRange;
    [Tooltip("The layer mask for the enemies")]
    [SerializeField] private LayerMask _enemyLayerMask;

    // Methods for the initialization values
    public Transform FirePoint { get => _firePoint; }

    private void Awake()
    {
        if (_firePoint == null)
        {
            Debug.Log("Fire point is not set.");
        }
        _gm = GameManager.Instance;
        _gm.Tower = transform;
        if(_charStats != null)
        {
            _attackSpeedValue = _charStats.GetStatValue(_attackSpeedStat);
        }
    }

    private void FixedUpdate()
    {
        if (_enemiesInRange.Count != 0 && _enemiesInRange[0] == null)
        {
            _enemiesInRange.RemoveAt(0);
        }

        if (CanAttack() && _enemiesInRange.Count > 0)
        {
            _attackController.DoAttack(_enemiesInRange[0]);
            _lastAttackTime = Time.time;
        }
    }

    float _lastAttackTime;

    /// <summary>
    /// Checks if the character can attack based on the attack speed and the time since the last attack.
    /// </summary>
    /// <returns>True if the character can attack, otherwise false.</returns>
    private bool CanAttack()
    {
        return Time.time - _lastAttackTime >= 1 / _attackSpeedValue;
    }

    public void RemoveFromList(HealthController hc)
    {
        if(_enemiesInRange.Contains(hc))
            _enemiesInRange.Remove(hc);
    }

    /// <summary>
    /// Called when a collider enters the trigger.
    /// Adds the entering collider's HealthController to the list of enemies in range.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Trigger entered with {other.name}");
        if (other.transform.parent.TryGetComponent<HealthController>(out var parentHealthController))
        {
            //Debug.Log($"Adding {parentHealthController.name} to the list");
            _enemiesInRange.Add(parentHealthController);
        }
        else if (other.TryGetComponent<HealthController>(out var healthController))
        {
            //Debug.Log($"Adding {healthController.name} to the list");
            _enemiesInRange.Add(healthController);
        }
    }

    /// <summary>
    /// Called when a collider exits the trigger.
    /// Removes the exiting collider's HealthController from the list of enemies in range.
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exited");
        if (other.transform.parent.TryGetComponent<HealthController>(out var parentHealthController))
        {
            Debug.Log($"Removing {parentHealthController.name} from the list");
            _enemiesInRange.Remove(parentHealthController);
        }
        else if (other.TryGetComponent<HealthController>(out var healthController))
        {
            Debug.Log($"Removing {healthController.name} from the list");
            _enemiesInRange.Remove(healthController);
        }
    }

    private void OnValidate()
    {
        if (_attackSpeedStat == null)
        {
            Debug.LogError($"Attack speed stat reference not set correctly on {gameObject.name}.");
        }
        if (_charStats == null)
        {
            Debug.LogError($"Char stats reference not set correctly on {gameObject.name}.");
        }
    }
}
