using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [Header("Health Values")]
    [SerializeField] private Stat _healthStatAsset;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    public float CurrentHealth { get => _currentHealth; }


    private CharStats _charInfo;

    /// <summary>
    /// Event that is invoked when a character dies. 
    /// <para>
    /// The bool parameter is true if the character is the player.
    /// </para>
    /// </summary>
    [Space(20)]
    public UnityEvent OnCharDeath;

    void Start()
    {
        _charInfo = GetComponent<CharStats>();
        _maxHealth = _charInfo.GetStatValue(_healthStatAsset);
        _currentHealth = _maxHealth;
    }

    private void CheckHealth()
    {
        if(_currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        CheckHealth();
    }

    private void Die()
    {
        OnCharDeath?.Invoke();
        Destroy(gameObject);
    }
}
