using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using System;




#if UNITY_EDITOR
using System.Collections.Generic;
using TMPro;
using UnityEditor;
#endif


/// <summary>
/// Class that manages the game.
/// </summary>
public class GameManager : MonoBehaviour
{
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

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
#if UNITY_EDITOR
        SetupDebug();
#endif
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


    //========================================== DEBUGGING ========================================================
#if UNITY_EDITOR
    [Header("Debugging")]
    [Tooltip("Show the debug text?")]
    [SerializeField] private bool showDebug = false;
    [Space(10)]
    [Tooltip("The text that will be used to display debugging information")]
    [SerializeField] private TMP_Text debuggingText;
    [Tooltip("Size of the text")]
    [SerializeField] private int textSize = 24;

    /// <summary>
    /// Set up the debugging functionality.
    /// </summary>
    void SetupDebug()
    {
        if (CheckIfNone())
        {
            debuggingText.gameObject.SetActive(false);
            Debug.Log("No Debug Flags are set, disabling debugging text");
            return;
        }
        if (showDebug)
        {
            Debug.Log("Debugging text is enabled");
            debuggingText.gameObject.SetActive(true);
            debuggingText.fontSize = textSize;
        }
        else
        {
            Debug.Log("Debugging text is disabled");
            debuggingText.gameObject.SetActive(false);
        }
    }

    [System.Flags]
    public enum DebugFlagsEnum
    {
        None = 0,
        SpawnedPlatform = 1 << 0,
        SpawnedPlatformDetailed = 1 << 1,
        Difficulty = 1 << 2,
        DifficultyOdds = 1 << 3,
    }
    [Space(10)]
    [Tooltip("The debug flags to display in the inspector")]
    [SerializeField] private DebugFlagsEnum debugFlags;

    private readonly Dictionary<DebugFlagsEnum, string> DebugFlagsText = new()
    {
        { DebugFlagsEnum.SpawnedPlatform, "Spawned Platform: " },
        { DebugFlagsEnum.SpawnedPlatformDetailed, "Spawned Platform Difficulty: " },
        { DebugFlagsEnum.Difficulty, "Difficulty: " },
        { DebugFlagsEnum.DifficultyOdds, "OddRatio's:\neasy: {0}\nmedium: {1}\nhard: {2}" },
    };

    bool CheckIfNone()
    {
        return debugFlags == DebugFlagsEnum.None;
    }

    /// <summary>
    /// Update the debugging information.
    /// </summary>
    public void UpdateDebug()
    {
        if (!debuggingText.gameObject.activeSelf)
        {
            return;
        }

        List<string> debugMessages = new();

        foreach (DebugFlagsEnum flag in Enum.GetValues(typeof(DebugFlagsEnum)))
        {
            // Guard block
            if (!debugFlags.HasFlag(flag) || !DebugFlagsText.ContainsKey(flag))
            {
                continue;
            }
            string message = DebugFlagsText[flag];

            switch (flag)
            {
                case DebugFlagsEnum.SpawnedPlatform:
                    message += groundManager.LastSpawnedPlatform;
                    break;
                case DebugFlagsEnum.SpawnedPlatformDetailed:
                    message += groundManager.LastSpawnedPlatformDifficulty;
                    break;
                case DebugFlagsEnum.Difficulty:
                    message += DifficultyLevel;
                    break;
                case DebugFlagsEnum.DifficultyOdds:
                    float easyRatio = groundManager.EasyRatio;
                    float mediumRatio = groundManager.MediumRatio;
                    float hardRatio = 100 - easyRatio - mediumRatio;
                    message = string.Format(message, easyRatio, mediumRatio, hardRatio);
                    break;
            }

            // Add the debug message to the list
            debugMessages.Add(message);
        }
        //Debug.Log(string.Join("\n", debugMessages));

        // Join all debug messages into a single string and display it
        debuggingText.text = string.Join("\n", debugMessages);
    }
#endif
}
