using UnityEngine;

/// <summary>
/// Handles the attack logic for a character.
/// </summary>
[RequireComponent(typeof(CharStats))]
public class AttackController : MonoBehaviour
{
    [Header("Stat References")]
    [SerializeField] private Stat _damageStat;
    [SerializeField] private Stat _criticalDamageStat;
    [SerializeField] private Stat _criticalChanceStat;
    [SerializeField] private Stat _armourPenStat;

    [Header("Stat Values")]
    [SerializeField] private float _damageValue;
    [SerializeField] private float _criticalDamageValue;
    [SerializeField] private float _criticalChanceValue;
    [SerializeField] private float _armourPenValue;

    private TowerController _controller;

    private void Start()
    {
        GetValues();
        TryGetComponent<TowerController>(out var _controller);
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
        }
        else
        {
            Debug.Log("CharStats component not found.");
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

        float _damageDealt = isCrit ? _damageValue * CalculateCriticalDamage() : _damageValue;

        _toDamageChar.TakeDamage(_damageDealt, _armourPenValue);
    }

    ///<summary>
    /// Rolls a random number to determine if the attack is a critical hit.
    /// </summary>
    /// <returns>True if the attack is a critical hit, otherwise false.</returns>
    private bool RollForCrit()
    {
        return Random.Range(0, 100) <= _criticalChanceValue;
    }

    /// <summary>
    /// Calculates the critical damage based on the critical chance value.
    /// </summary>
    /// <returns>The calculated critical damage as a percentage.</returns>
    private float CalculateCriticalDamage()
    {
        // If the critical chance is less than 100, then return _criticalDamageValue as a percentage
        if (_criticalChanceValue < 100) return _criticalDamageValue / 100;

        // Otherwise return the _criticalDamageValue added with the excess of the _criticalChanceValue
        // and return that as a percentage
        else return (_criticalDamageValue + (_criticalChanceValue - 100)) / 100;
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
        if (_armourPenStat == null)
        {
            Debug.LogError($"Armour penetration stat reference not set correctly on {gameObject.name}.");
        }
    }
}