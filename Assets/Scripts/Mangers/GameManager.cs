using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance;

    [Header("References")]
    [Tooltip("The UI Manager of the game")]
    [SerializeField] private UIManager uiManager;
    [Tooltip("The player object in the scene")]
    public GameObject player;

    [Header("Speed Values")]
    [Tooltip("The default speed the player is going to have in the z axis")]
    public float levelMovementSpeed = 5f;
    [Tooltip("The scale that the default speed rises with, this is a linear value")]
    public float levelMovementScale = 0.1f;
    [Tooltip("The distance the player has traveled in the game")]
    public float distanceTraveled = 0f;

    public static event System.Action OnSpeedChanged;
    public static event System.Action OnDistanceChanged;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SetupReferences();
    }

    private void Start()
    {
        // Get the player
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    /// <summary>
    /// Set up the references for the GameManager.
    /// </summary>
    void SetupReferences()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        // Get the UI Manager
        if (uiManager == null)
        {
            uiManager = UIManager.instance;
        }
    }

    /// <summary>
    /// FixedUpdate is called once every physics update.
    /// </summary>
    void FixedUpdate()
    {
        if (Time.timeScale != 0)
        {
            IncreaseSpeed();
            IncreaseDistanceTraveled();
        }
    }

    /// <summary>
    /// Increase the distance traveled by the player.
    /// </summary>
    void IncreaseDistanceTraveled()
    {
        distanceTraveled += levelMovementSpeed * Time.deltaTime;
        OnDistanceChanged?.Invoke();
    }

    /// <summary>
    /// Used to increase the speed of everything in the game.
    /// </summary>
    void IncreaseSpeed()
    {
        levelMovementSpeed += levelMovementScale * Time.deltaTime;
        OnSpeedChanged?.Invoke();
    }
}
