using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the attack logic for a character.
/// </summary>
[RequireComponent(typeof(CharStats))]
public class AttackController : MonoBehaviour
{
    [Header("Initialization Values")]
    [SerializeField] bool _isPlayer;

    [Header("Stat References")]
    [SerializeField] private Stat _damageStat;
    [SerializeField] private Stat _criticalDamageStat;
    [SerializeField] private Stat _criticalChanceStat;
    [SerializeField] private Stat _attackSpeedStat;
    [SerializeField] private Stat _armourPenStat;

    [Header("Stat Values")]
    [SerializeField] private float _damageValue;
    [SerializeField] private float _criticalDamageValue;
    [SerializeField] private float _criticalChanceValue;
    [SerializeField] private float _attackSpeedValue;
    [SerializeField] private float _armourPenValue;

    [Header("Attack Settings")]
    [Tooltip("The characters that are within range")]
    [SerializeField] List<HealthController> _enemiesInRange;
    [Tooltip("The layer mask for the enemies")]
    [SerializeField] private LayerMask _enemyLayerMask;

    [Header("AI Settings")]
    [Tooltip("The range outside of its own mesh that it can attack in")]
    [SerializeField] private float _attackRange = 1f;


    private MeshRenderer _mr;
    private float _lastAttackTime;

    private void Start()
    {
        GetValues();
        if (!_isPlayer)
        {
            _mr = GetComponentInChildren<MeshRenderer>();
            _attackRange += _mr.bounds.extents.x;
        }
    }

    private void OnValidate()   
    {
        if (_damageStat == null)
        {
            Debug.LogError($"Damage stat reference not set correctly on {gameObject.name}.");
        }
        if (_criticalDamageStat == null)
        {
            Debug.LogError($"Critical damage stat reference not set correctly on {gameObject.name}.");
        }
        if (_criticalChanceStat == null)
        {
            Debug.LogError($"Critical chance stat reference not set correctly on {gameObject.name}.");
        }
        if (_attackSpeedStat == null)
        {
            Debug.LogError($"Attack speed stat reference not set correctly on {gameObject.name}.");
        }
        if (_armourPenStat == null)
        {
            Debug.LogError($"Armour penetration stat reference not set correctly on {gameObject.name}.");
        }
    }

    /// <summary>
    /// Retrieves the values for damage, critical damage, and critical chance from the character's stats.
    /// </summary>
    public void GetValues()
    {
        if (TryGetComponent<CharStats>(out var _charStats))
        {
            _damageValue = _charStats.GetStatValue(_damageStat);
            _criticalDamageValue = _charStats.GetStatValue(_criticalDamageStat);
            _criticalChanceValue = _charStats.GetStatValue(_criticalChanceStat);
            _attackSpeedValue = _charStats.GetStatValue(_attackSpeedStat);
        }
        else
        {
            Debug.Log("CharStats component not found.");
        }
    }

    /// <summary>
    /// Checks if the character can attack and if there are enemies in range. If so, performs an attack on the first enemy in range.
    /// </summary>
    private void Update()
    {
        if (!_isPlayer && IsWithinRange(out var healthController))
        {
            DoAttack(healthController);
        }

        // Player behaviour
        if (CanAttack() && _enemiesInRange.Count > 0)
        {
            DoAttack(_enemiesInRange[0]);
        }
    }

    /// <summary>
    /// Checks if there are enemies in range using a sphere cast.
    /// </summary>
    /// <returns>True if there are enemies in range, otherwise false.</returns>
    private bool IsWithinRange(out HealthController towerHC)
    {
        Transform towerPos = GameManager.Instance.Tower;
        towerHC = null;
        float rangeSquared = _attackRange * _attackRange;

        if ((transform.position - towerPos.position).sqrMagnitude <= rangeSquared)
        {
            towerHC = towerPos.GetComponent<HealthController>();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Performs an attack on the specified character, dealing damage and potentially removing the character from the enemies in range if they die.
    /// </summary>
    /// <param name="_toDamageChar">The character to attack.</param>
    public void DoAttack(HealthController _toDamageChar)
    {
        if (_toDamageChar == null) return;

        bool isCrit = RollForCrit();

        float _damageDealt = isCrit ? _damageValue * _criticalDamageValue : _damageValue;

        _toDamageChar.TakeDamage(_damageDealt, _armourPenValue);

        if (_toDamageChar.CurrentHealth <= 0)
        {
            _enemiesInRange.Remove(_toDamageChar);
        }

        _lastAttackTime = Time.time;
    }



    public void RemoveFromList(HealthController _diedChar)
    {
        if (_enemiesInRange.Contains(_diedChar))
        {
            _enemiesInRange.Remove(_diedChar);
        }
        else
        {
            Debug.Log($"{_diedChar.gameObject.name} not in {gameObject.name}'s {nameof(_enemiesInRange)} List.");
        }
    }

    ///<summary>
    /// Rolls a random number to determine if the attack is a critical hit.
    /// </summary>
    /// <returns>True if the attack is a critical hit, otherwise false.</returns>
    private bool RollForCrit()
    {
        float roll = Random.Range(0, 100);
        return roll <= _criticalChanceValue;
    }

    /// <summary>
    /// Checks if the character can attack based on the attack speed and the time since the last attack.
    /// </summary>
    /// <returns>True if the character can attack, otherwise false.</returns>
    private bool CanAttack()
    {
        return Time.time - _lastAttackTime >= 1 / _attackSpeedValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (!_isPlayer)
        {
            return;
        }

        if (other.TryGetComponent<HealthController>(out var healthController))
        {
            Debug.Log($"Adding {healthController.name} to the list");
            _enemiesInRange.Add(healthController);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        if (!_isPlayer)
        {
            return;
        }

        if (other.TryGetComponent<HealthController>(out var healthController))
        {
            RemoveFromList(healthController);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_isPlayer)
        {
            return;
        }
        _mr = GetComponentInChildren<MeshRenderer>();
        float _range = _attackRange + _mr.bounds.extents.x;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
#endif
}