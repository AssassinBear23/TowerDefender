using UnityEngine;
using TMPro;



/// <summary>
/// Class that manages the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [Tooltip("The player object in the scene")]
    public GameObject player;
    [Space(10)]
    // The script that managers others. Its a reference to itself (Singleton pattern)
    [HideInInspector] public static GameManager instance;
    // The UI Manager that manages the UI elements in the current scene
    public UIManager uiManager;
    // The Ground Manager that manages the ground/platforms in the scene.
    public GroundManager groundManager;

    [Header("Scores")]
    [Tooltip("The score the player has in the game")]
    public int score;
    [Tooltip("The high score the player has in the game on this device")]
    public int highScore;

    [Header("Speed Values")]
    [Tooltip("The default speed the player is going to have in the z axis")]
    public float levelMovementSpeed = 5f;
    [Tooltip("The scale that the default speed rises with, this is a linear value")]
    public float levelMovementScale = 0.1f;
    [Tooltip("The distance the player has traveled in the game")]
    public float distanceTraveled = 0f;

    [HideInInspector] public int DifficultyLevel { get; private set; }

    public static event System.Action OnDifficultyIncrease;

    #endregion Variables

    // ========================================== METHOD DECLERATION =======================================================

    #region Methods

    #region SetupMethods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SetupReferences();
    }

    private void Start()
    {
        SubscribeToEvents();
        groundManager.CalculateDifficultyRatio();
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
    }

    private void SubscribeToEvents()
    {
        PlayerController.PlayerDeath += GameOver;
    }

    #endregion SetupMethods

    // =========================================== FUNCTIONAL METHODS ===============================================

    #region FunctionalMethods

    /// <summary>
    /// FixedUpdate is called once every physics update.
    /// </summary>
    void FixedUpdate()
    {
        if (Time.timeScale != 0)
        {
            IncreaseSpeed();
            IncreaseDistanceTraveled();
            uiManager.UpdateElements();
        }
    }

    /// <summary>
    /// Increase the distance traveled by the player.
    /// </summary>
    void IncreaseDistanceTraveled()
    {
        distanceTraveled += levelMovementSpeed * Time.deltaTime;
        int predictedLevel = (int)distanceTraveled / 100;
        if ((int)distanceTraveled % 100 == 0 && DifficultyLevel != predictedLevel)
        {
            IncreaseDifficulty();
            Debug.Log("Difficulty Increased");
        }
    }

    /// <summary>
    /// Used to increase the speed of everything in the game.
    /// </summary>
    void IncreaseSpeed()
    {
        levelMovementSpeed += levelMovementScale * Time.deltaTime;
    }

    void IncreaseDifficulty()
    {
        DifficultyLevel += 1;
        OnDifficultyIncrease?.Invoke();
    }

    public void CalculateScore()
    {
        Debug.Log("Calculating Score...");
        instance.score = ((int)(distanceTraveled / 10) + (DifficultyLevel * 100) + (int)(Time.timeSinceLevelLoad));
        Debug.Log("Score:\t" + instance.score + "\nHigh Score:\t" + instance.highScore);
        if (instance.score > instance.highScore)
        {
            IncreaseHighscore();
        }
    }

    void IncreaseHighscore()
    {
        instance.highScore = instance.score;
        PlayerPrefs.SetInt("HighScore", instance.highScore);
        PlayerPrefs.Save();
    }

    //===================================== END GAME FUNCTIONALITY =========================================

    [Header("Game Over")]
    [SerializeField] private int gameOverPageIndex = 0;

    /// <summary>
    /// Loads the game over scene.
    /// </summary>
    void GameOver()
    {
        Time.timeScale = 0;
        Debug.Log("Game Over");
        if (uiManager != null)
        {
            uiManager.allowPause = false;
            uiManager.GoToPage(gameOverPageIndex);
        }
        CalculateScore();
        uiManager.UpdateElements();
    }

    #endregion FunctionalMethods

    #endregion Methods
}
