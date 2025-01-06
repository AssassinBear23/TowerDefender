using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthController _healthController;

    [Header("Health Bar Settings")]
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fill;
    [SerializeField] private Gradient _gradient;

    public void Start()
    {
        Debug.Log("HealthBar has started");
        UpdateMaxHealth();
        UpdateHealth();
        SetColor();
    }

    public void UpdateMaxHealth()
    {
        _slider.maxValue = _healthController.MaxHealth;
        SetColor();
    }

    public void UpdateHealth()
    {
        _slider.value = _healthController.CurrentHealth;
        SetColor();
        if (_slider.value == _slider.maxValue)
        {
            _fill.enabled = false;
        }
        else
        {
             _fill.enabled = true;
        }
    }

    public void SetColor()
    {
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
