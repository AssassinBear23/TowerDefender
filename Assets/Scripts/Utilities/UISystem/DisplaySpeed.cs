using TMPro;
using UnityEngine;


public class DisplaySpeed : UIElements
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Updates the display of the speed.
    /// </summary>
    public override void UpdateDisplay()
    {
        float speed = GameManager.instance.levelMovementSpeed; // Get the current speed from GameManager
        float speedInKmh = Mathf.Round(speed * 3.6f * 10f) / 10f; // Convert m/s to km/h and round to 2 decimal places
        text.text = $"SPEED: {speedInKmh}KM/H"; // Update the speed text in the UI
    }
}
