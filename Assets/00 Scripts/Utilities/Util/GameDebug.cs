using System;
using System.Collections.Generic;
using TMPro;

public static class GameDebug
{
    // Variables  
    private static TMP_Text debuggingText;
    private static int textSize;
    public static DebugFlagsEnum debugFlags { get; set; }

    /// <summary>  
    /// Dictionary to hold the debug flag text messages.  
    /// </summary>  
    private static readonly Dictionary<DebugFlagsEnum, string> DebugFlagsText = new()
   {
       { DebugFlagsEnum.SpawnedPlatform, "Spawned Platform: " },
       { DebugFlagsEnum.SpawnedPlatformDetailed, "Spawned Platform Difficulty: " },
       { DebugFlagsEnum.Difficulty, "Difficulty: " },
       { DebugFlagsEnum.DifficultyOdds, "OddRatio's:\neasy: {0}\nmedium: {1}\nhard: {2}" },
   };

    /// <summary>  
    /// Enum to define the different debug flags.  
    /// </summary>  
    [System.Flags]
    public enum DebugFlagsEnum
    {
        None = 0,
        SpawnedPlatform = 1 << 0,
        SpawnedPlatformDetailed = 1 << 1,
        Difficulty = 1 << 2,
        DifficultyOdds = 1 << 3,
    }

    /// <summary>  
    /// Sets up the debugging text, size, and flags.  
    /// </summary>  
    /// <param name="text">The TMP_Text component to display debug information.</param>  
    /// <param name="size">The font size of the debug text.</param>  
    /// <param name="flags">The debug flags to enable.</param>  
    public static void SetupDebug(TMP_Text text, int size, DebugFlagsEnum flags)
    {
        debuggingText = text;
        textSize = size;
        debugFlags = flags;

        if (CheckIfNone())
        {
            debuggingText.gameObject.SetActive(false);
            UnityEngine.Debug.Log("No Debug Flags are set, disabling debugging text");
            return;
        }

        text.fontSize = textSize;
    }

    /// <summary>  
    /// Checks if no debug flags are set.  
    /// </summary>  
    /// <returns>True if no debug flags are set, otherwise false.</returns>  
    private static bool CheckIfNone()
    {
        return debugFlags == DebugFlagsEnum.None;
    }

    /// <summary>  
    /// Updates the debug text based on the enabled debug flags.  
    /// </summary>  
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
                    //message += GameManager.instance.groundManager.LastSpawnedPlatform;
                    break;
                case DebugFlagsEnum.SpawnedPlatformDetailed:
                    //message += GameManager.instance.groundManager.LastSpawnedPlatformDifficulty;
                    break;
                case DebugFlagsEnum.Difficulty:
                    //message += GameManager.instance.DifficultyLevel;
                    //break;
                case DebugFlagsEnum.DifficultyOdds:
                    //float easyRatio = GameManager.instance.groundManager.EasyRatio;
                    //float mediumRatio = GameManager.instance.groundManager.MediumRatio;
                    //float hardRatio = 100 - easyRatio - mediumRatio;
                    //message = string.Format(message, easyRatio, mediumRatio, hardRatio);
                    break;
            }

            // Add the message to the list  
            debugMessages.Add(message);
        }

        // Set the text to the debug messages  
        debuggingText.text = string.Join("\n", debugMessages);
    }

    /// <summary>  
    /// Enables a specific debug flag.  
    /// </summary>  
    /// <param name="flag">The debug flag to enable.</param>  
    public static void EnableDebugFlag(DebugFlagsEnum flag)
    {
        debugFlags |= flag;
    }

    /// <summary>  
    /// Disables a specific debug flag.  
    /// </summary>  
    /// <param name="flag">The debug flag to disable.</param>  
    public static void DisableDebugFlag(DebugFlagsEnum flag)
    {
        debugFlags &= ~flag;
    }

    /// <summary>  
    /// Checks if a specific debug flag is enabled.  
    /// </summary>  
    /// <param name="flag">The debug flag to check.</param>  
    /// <returns>True if the debug flag is enabled, otherwise false.</returns>  
    public static bool IsDebugFlagEnabled(DebugFlagsEnum flag)
    {
        return debugFlags.HasFlag(flag);
    }
}
