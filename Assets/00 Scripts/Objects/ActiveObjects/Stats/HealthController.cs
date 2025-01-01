using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [Header("Stat References")]
    [SerializeField] private Stat _healthStat;
    [SerializeField] private Stat _armourStat;

    [Space(10)]
    [Header("Stat Values")]
    [SerializeField] private float _maxHealthValue;
    [SerializeField] private float _currentHealthValue;
    [SerializeField] private float _armourValue;
    public float CurrentHealth { get => _currentHealthValue; }
    private CharStats _charInfo;

    /// <summary>
    /// Event that is invoked when the character dies. 
    /// </summary>
    [Space(20)]
    public static Action<HealthController> OnCharDeath;

    /// <summary>
    /// Initializes the health controller by setting all the values.
    /// </summary>
    void Start()
    {
        _charInfo = GetComponent<CharStats>();
        _maxHealthValue = _charInfo.GetStatValue(_healthStat);
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
            Die();
        }
    }

    /// <summary>
    /// Reduces the current health by the specified damage amount and checks health status.
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public void TakeDamage(float damage, float armourPen)
    {
        float _armourLeft = Mathf.Max(_armourValue - armourPen, 0);
        float _finalDamage = damage - _armourLeft;
        _currentHealthValue -= _finalDamage;
        CheckHealth();
    }

    /// <summary>
    /// Invokes the character death event.
    /// </summary>
    private void Die()
    {
        GameManager _gm = GameManager.Instance;
        if (_charInfo.CharType != CharTypes.Tower)
        {
            _gm.Tower.GetComponent<TowerController>().RemoveFromList(this);
        }
        else
        {
            _gm.GameOver();
        }
    }
}
