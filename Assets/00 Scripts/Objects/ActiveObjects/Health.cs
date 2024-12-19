using System;
using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("Health Values")]
    [SerializeField] private Stat _healthStatAsset;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    private CharStats _charInfo;

    /// <summary>
    /// Event that is invoked when a character dies. 
    /// <para>
    /// The bool parameter is true if the character is the player.
    /// </para>
    /// </summary>
    public static event Action<bool> OnCharDeath;

    void Start()
    {
        _charInfo = GetComponent<CharStats>();
        _maxHealth = _charInfo.GetStatValue(_healthStatAsset);
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnCharDeath?.Invoke(_charInfo.IsTower);
        Destroy(gameObject);
    }
}
