using System;
using System.Collections.Generic;
using TMPro;

public static class GameDebug
{
    // Variables
    private static TMP_Text debuggingText;
    private static bool showDebug;
    private static int textSize;
    private static DebugFlagsEnum debugFlags;

    // Dictionary to hold the debug flags and their corresponding text
    private static readonly Dictionary<DebugFlagsEnum, string> DebugFlagsText = new()
    {
        { DebugFlagsEnum.SpawnedPlatform, "Spawned Platform: " },
        { DebugFlagsEnum.SpawnedPlatformDetailed, "Spawned Platform Difficulty: " },
        { DebugFlagsEnum.Difficulty, "Difficulty: " },
        { DebugFlagsEnum.DifficultyOdds, "OddRatio's:\neasy: {0}\nmedium: {1}\nhard: {2}" },
    };

    // Enum to hold the debug flags
    [System.Flags]
    public enum DebugFlagsEnum
    {
        None = 0,
        SpawnedPlatform = 1 << 0,
        SpawnedPlatformDetailed = 1 << 1,
        Difficulty = 1 << 2,
        DifficultyOdds = 1 << 3,
    }

    // Method to set up the debugging text
    public static void SetupDebug(TMP_Text text, bool show, int size, DebugFlagsEnum flags)
    {
        debuggingText = text;
        showDebug = show;
        textSize = size;
        debugFlags = flags;

        if (CheckIfNone())
        {
            debuggingText.gameObject.SetActive(false);
            UnityEngine.Debug.Log("No Debug Flags are set, disabling debugging text");
            return;
        }
        if (showDebug)
        {
            UnityEngine.Debug.Log("Debugging text is enabled");
            debuggingText.gameObject.SetActive(true);
            debuggingText.fontSize = textSize;
        }
        else
        {
            UnityEngine.Debug.Log("Debugging text is disabled");
            debuggingText.gameObject.SetActive(false);
        }
    }

    // Method to check if no debug flags are set
    private static bool CheckIfNone()
    {
        return debugFlags == DebugFlagsEnum.None;
    }

    // Method to update the debugging text
    public static void UpdateDebug()
    {
        if (!debuggingText.gameObject.activeSelf)
        {
            return;
        }

        // List to hold the debug messages
        List<string> debugMessages = new();

        // Loop through all the debug flags
        foreach (DebugFlagsEnum flag in Enum.GetValues(typeof(DebugFlagsEnum)))
        {
            // Check if the flag is set and if the flag is in the dictionary, if not continue to next loop.
            if (!debugFlags.HasFlag(flag) || !DebugFlagsText.ContainsKey(flag))
            {
                continue;
            }

            // Get the message from the dictionary
            string message = DebugFlagsText[flag];

            // Switch statement to get the correct message
            switch (flag)
            {
                case DebugFlagsEnum.SpawnedPlatform:
                    message += GameManager.instance.groundManager.LastSpawnedPlatform;
                    break;
                case DebugFlagsEnum.SpawnedPlatformDetailed:
                    message += GameManager.instance.groundManager.LastSpawnedPlatformDifficulty;
                    break;
                case DebugFlagsEnum.Difficulty:
                    message += GameManager.instance.DifficultyLevel;
                    break;
                case DebugFlagsEnum.DifficultyOdds:
                    float easyRatio = GameManager.instance.groundManager.EasyRatio;
                    float mediumRatio = GameManager.instance.groundManager.MediumRatio;
                    float hardRatio = 100 - easyRatio - mediumRatio;
                    message = string.Format(message, easyRatio, mediumRatio, hardRatio);
                    break;
            }

            // Add the message to the list
            debugMessages.Add(message);
        }

        // Set the text to the debug messages
        debuggingText.text = string.Join("\n", debugMessages);
    }
}
