using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharStats))]
public class HealthController : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("The health bar for the character")]
    [SerializeField] private HealthBar _healthBar;

    [Header("Stat References")]
    [SerializeField] private Stat _healthStat;
    [SerializeField] private Stat _armourStat;

    [Space(10)]
    [Header("Stat Values")]
    [SerializeField] private float _maxHealthValue;
    [SerializeField] private float _currentHealthValue;
    [SerializeField] private float _armourValue;

    /// <summary>
    /// The maximum health of the character.
    /// </summary>
    public float MaxHealth { get => _maxHealthValue; }
    /// <summary>
    /// The current health of the character.
    /// </summary>
    public float CurrentHealth { get => _currentHealthValue; }

    private CharStats _charInfo;

    /// <summary>
    /// Event that is invoked when the character dies. 
    /// </summary>
    [Space(20)]
    public UnityEvent OnDamage;
    public UnityEvent OnMaxHealthChange;
    public UnityEvent OnDeath;

    /// <summary>
    /// Initializes the health controller by setting all the values.
    /// </summary>
    void OnEnable()
    {
        _charInfo = GetComponent<CharStats>();
        UpdateStats();
        _currentHealthValue = _maxHealthValue;
    }

    /// <summary>
    /// Validates the health controller's configuration in the editor.
    /// </summary>
    private void OnValidate()
    {
        if (_healthStat == null)
        {
            Debug.LogError($"Health Stat not set in {gameObject.name}.");
        }
        if (_armourStat == null)
        {
            Debug.LogError($"Armour Stat not set in {gameObject.name}.");
        }
    }

    /// <summary>
    /// Checks the current health and triggers the death event if health is zero or below.
    /// </summary>
    private void CheckHealth()
    {
        if (_currentHealthValue <= 0)
        {
            OnDeath?.Invoke();
            Die();
        }
    }

    public void UpdateStats()
    {
        _maxHealthValue = _charInfo.GetStatValue(_healthStat);
        _armourValue = _charInfo.GetStatValue(_armourStat);
    }


    /// <summary>
    /// Reduces the current health by the specified damage amount and checks health status.
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public void TakeDamage(float damage, float armourPen)
    {
        // Calculate the damage taken
        float _armourLeft = Mathf.Max(_armourValue - armourPen, 0);
        float _finalDamage = damage - _armourLeft;
        // Remove damage from health
        _currentHealthValue -= _finalDamage;
        // Check if dead
        CheckHealth();
        // Invoke damage event
        OnDamage?.Invoke();
    }

    /// <summary>
    /// Invokes the character death event.
    /// </summary>
    private void Die()
    {
        GameManager _gm = GameManager.Instance;
        if (_charInfo.CharType == CharTypes.Tower)
        {
            _gm.GameOver();
        }
        else
        {
            Destroy(_healthBar.gameObject);
            Destroy(gameObject);
        }
    }
}