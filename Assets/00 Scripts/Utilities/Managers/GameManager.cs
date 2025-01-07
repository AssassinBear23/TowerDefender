using UnityEngine;

/// <summary>
/// Class that manages the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [Tooltip("The tower object in the scene")]
    public Transform Tower;
    [Tooltip("The player object in the scene")]
    public Transform player;
    [Space(10)]
    // The script that managers others. Its a reference to itself (Singleton pattern)
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }
    // The UI Manager that manages the UI elements in the current scene
    public UIManager UIManager;
    public ItemManager ItemManager;
    //public WaveManager WaveManager;

    [Header("Stats")]
    [Tooltip("The wave the player is currently at")]
    public int Wave;

     //The level data of the loaded level
    [Tooltip("The level data of the loaded level")]
    [SerializeField] private Level _levelData;
    public Level LevelData { get => _levelData; set => _levelData = value; }

    [Header("Wave Data")]
    [SerializeField] private float _enemiesAlive;
    public float EnemiesAlive { get => _enemiesAlive; set => _enemiesAlive = value; }

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

    /// <summary>
    /// Set up the GameManager Singleton.
    /// </summary>
    void SetupSingleton()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion SetupMethods

    //===================================== END GAME FUNCTIONALITY =========================================

    [Header("Game Over")]
    [SerializeField] private int gameOverPageIndex = 0;

    /// <summary>
    /// Loads the game over scene.
    /// </summary>
    public void GameOver()
    {
        Time.timeScale = 0;
        Debug.Log("Game Over");
        if (UIManager != null)
        {
            UIManager.allowPause = false;
            UIManager.GoToPage(gameOverPageIndex);
        }

    }

    #endregion FunctionalMethods
}
