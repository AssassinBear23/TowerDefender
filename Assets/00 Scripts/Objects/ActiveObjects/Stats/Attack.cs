using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharStats))]
public class Attack : MonoBehaviour
{
    [Header("Initialization Values")]
    [SerializeField] bool _isPlayer;

    [Header("Stat References")]
    [SerializeField] private Stat _damageStat;
    [SerializeField] private Stat _criticalDamageStat;
    [SerializeField] private Stat _criticalChanceStat;
    [SerializeField] private Stat _attackSpeed;

    [Header("Stat Values")]
    [SerializeField] private float _damageValue;
    [SerializeField] private float _criticalDamageValue;
    [SerializeField] private float _criticalChanceValue;
    [SerializeField] private float _attackSpeedValue;

    [Header("Attack Settings")]
    [Tooltip("The characters that are within range")]
    [SerializeField] List<HealthController> _enemiesInRange;

    private float _lastAttackTime;

    private void Start()
    {
        GetValues();
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
            _attackSpeedValue = _charStats.GetStatValue(_attackSpeed);
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
        if (CanAttack() && _enemiesInRange.Count > 0)
        {
            DoAttack(_enemiesInRange[0]);
        }
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

        _toDamageChar.TakeDamage(_damageDealt);

        if (_toDamageChar.CurrentHealth <= 0)
        {
            _enemiesInRange.Remove(_toDamageChar);
        }

        _lastAttackTime = Time.time;
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
    /// Handles collision events. Adds the collided object to the enemies in range if it is the tower and the character is not the player.
    /// </summary>
    /// <param name="other">The collision information.</param>
    private void OnCollisionEnter(Collision other)
    {
        if (!_isPlayer && _enemiesInRange.Count == 0)
        {
            if (other.transform == GameManager.Instance.Tower)
            {
                if (other.gameObject.TryGetComponent<HealthController>(out var healthController))
                {
                    _enemiesInRange.Add(healthController);
                }
            }
        }
    }

    /// <summary>
    /// Handles trigger events. Adds the triggered object to the enemies in range if it is an enemy and the character is the player.
    /// </summary>
    /// <param name="other">The trigger information.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (_isPlayer && other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<HealthController>(out var healthController))
            {
                _enemiesInRange.Add(healthController);
            }
        }
    }
}

