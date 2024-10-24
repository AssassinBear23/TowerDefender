using TMPro;
using UnityEngine;

public class DisplayDistance : UIElements
{
    [Tooltip("The text element that displays the distance.")]
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Updates the display of the distance.
    /// </summary>
    public override void UpdateDisplay()
    {
        var distanceTraveled = GameManager.instance.distanceTraveled; // Get the distance traveled from GameManager
        if (distanceTraveled < 1000)
        {
            text.text = $"DISTANCE: {(int)distanceTraveled}M"; // Display distance in meters if less than 1000
        }
        else
        {
            float distanceInKm = Mathf.Round((distanceTraveled / 1000f) * 10f) / 10f; // Convert meters to kilometers and round to 1 decimal place
            text.text = $"DISTANCE: {distanceInKm}KM"; // Update the distance text in the UI
        }
        text.text = $"Distance: {GameManager.instance.distanceTraveled}";
    }
}
