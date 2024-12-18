using System;
using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("Health Values")]
    [SerializeField] private Stat _healthStatAsset;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    private CharStats stat;

    public event Action<bool> OnCharDeath;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        stat = GetComponent<CharStats>();
        yield return new WaitForEndOfFrame();
        _maxHealth = stat.GetStatValue(_healthStatAsset);
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
        GameManager.instance.CharDeath?.Invoke();
        Destroy(gameObject);
    }
}
