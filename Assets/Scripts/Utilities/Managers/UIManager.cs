using UnityEngine;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public static UIManager instance;

    [Header("UI Elements")]
    [Tooltip("The text that displays the distance the player has traveled")]
    [SerializeField] private TMPro.TextMeshProUGUI distanceText;
    [Tooltip("The text that displays the speed the player is going")]
    [SerializeField] private TMPro.TextMeshProUGUI speedText;

    private float speed;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SetupSingleton(); // Set up the singleton instance
        SubscribeToEvents(); // Subscribe to events from GameManager
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        UnsubscribeFromEvents(); // Unsubscribe from events to avoid memory leaks
    }

    /// <summary>
    /// Sets up the UIManager singleton instance.
    /// </summary>
    void SetupSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this instance persists across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    /// <summary>
    /// Subscribes to the necessary events.
    /// </summary>
    void SubscribeToEvents()
    {
        GameManager.OnSpeedChanged += UpdateSpeed; // Subscribe to speed change event
        GameManager.OnDistanceChanged += UpdateDistance; // Subscribe to distance change event
    }

    /// <summary>
    /// Unsubscribes from the subscribed events.
    /// </summary>
    void UnsubscribeFromEvents()
    {
        GameManager.OnSpeedChanged -= UpdateSpeed; // Unsubscribe from speed change event
        GameManager.OnDistanceChanged -= UpdateDistance; // Unsubscribe from distance change event
    }

    /// <summary>
    /// Updates the speed text based on the current level movement speed.
    /// </summary>
    internal void UpdateSpeed()
    {
        // Guard Block
        if (GameManager.instance == null) return;

        speed = GameManager.instance.levelMovementSpeed; // Get the current speed from GameManager
        float speedInKmh = Mathf.Round(speed * 3.6f * 10f) / 10f; // Convert m/s to km/h and round to 2 decimal places
        speedText.text = $"SPEED: {speedInKmh}KM/H"; // Update the speed text in the UI
    }

    /// <summary>
    /// Updates the distance text based on the distance traveled by the player.
    /// </summary>
    internal void UpdateDistance()
    {
        // Guard Block
        if (GameManager.instance == null) return;

        var distanceTraveled = GameManager.instance.distanceTraveled; // Get the distance traveled from GameManager
        if (distanceTraveled < 1000)
        {
            distanceText.text = $"DISTANCE: {(int)distanceTraveled}M"; // Display distance in meters if less than 1000
        }
        else
        {
            float distanceInKm = Mathf.Round((distanceTraveled / 1000f) * 10f) / 10f; // Convert meters to kilometers and round to 1 decimal place
            distanceText.text = $"DISTANCE: {distanceInKm}KM"; // Update the distance text in the UI
        }
    }
}
