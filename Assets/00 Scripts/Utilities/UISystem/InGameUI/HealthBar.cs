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
        CheckForFillVisibility();
    }

    public void UpdateHealth()
    {
        _slider.value = _healthController.CurrentHealth;
        //Debug.Log($"{gameObject.name}"
        //          + $"\nHealth has been updated to:\t{_slider.value}"
        //          + $"\nMax health is:\t\t{_slider.maxValue}"
        //          + $"\nAre they considered equal to each other? {_slider.value == _slider.maxValue}");
        SetColor();
        CheckForFillVisibility();
    }

    private void CheckForFillVisibility()
    {
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
