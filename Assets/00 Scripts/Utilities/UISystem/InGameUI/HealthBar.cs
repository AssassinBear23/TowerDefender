using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthController _healthController;
    [SerializeField] private Slider _healthSlider;

    private void Start()
    {
        if (_healthController == null)
        {
            Debug.LogError("HealthController reference is missing.");
            return;
        }

        if (_healthSlider == null)
        {
            Debug.LogError("HealthSlider reference is missing.");
            return;
        }

        // Initialize the health bar
        _healthSlider.maxValue = _healthController.CurrentHealth;
        _healthSlider.value = _healthController.CurrentHealth;
    }

    public void UpdateHealthBar()
    {
        _healthSlider.value = _healthController.CurrentHealth;
    }
}
