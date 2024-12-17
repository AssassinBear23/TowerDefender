using UnityEngine;

/// <summary>
/// Class that manages the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [Tooltip("The tower object in the scene")]
    public GameObject Tower;
    [Space(10)]
    // The script that managers others. Its a reference to itself (Singleton pattern)
    [HideInInspector] public static GameManager instance;
    // The UI Manager that manages the UI elements in the current scene
    public UIManager uiManager;

    [Header("Scores")]
    [Tooltip("The score the player has in the game")]
    public int score;
    [Tooltip("The high score the player has in the game on this device")]
    public int highScore;

    // The level data of the loaded level
    //[Tooltip("The level data of the loaded level")]
    //[SerializeField] private Level _levelData;
    //public Level LevelData { get => _levelData; set => _levelData = value; }

    #endregion Variables

    // ========================================== METHOD DECLERATION =======================================================

    #region Methods

    #region SetupMethods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SetupSingleton();
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    /// <summary>
    /// Set up the GameManager Singleton.
    /// </summary>
    void SetupSingleton()
    {
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
        //CharacterController.PlayerDeath += GameOver;
    }

    #endregion SetupMethods

    // =========================================== FUNCTIONAL METHODS ===============================================

    #region FunctionalMethods

    /// <summary>
    /// FixedUpdate is called once every physics update.
    /// </summary>
    void Update()
    {
        if (Time.timeScale != 0)
        {
            uiManager.UpdateElements();
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Quiting Application");
#endif
        StaticMethods.QuitApplication();
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
        uiManager.UpdateElements();
    }

    #endregion FunctionalMethods

    #endregion Methods
}
