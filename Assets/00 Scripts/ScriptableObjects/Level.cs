using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Core/LevelData/Level")]
public class Level : ScriptableObject
{
    [Space(10)]
    [Header("Level Settings")]
    [SerializeField] private float strengthMultiplier = 1;
    /// <summary>
    /// The rate at which enemies get stronger as the waves progress
    /// </summary>
    public float StrengthMultiplier { get => strengthMultiplier; }

    [SerializeField] private List<Wave> waves;
    /// <summary>
    /// The waves that will be spawned in the level.
    /// </summary>
    public List<Wave> Waves { get => waves; }

    [Header("Scene Settings")]
    [SerializeField] private string levelSceneName;
    /// <summary>
    /// The scene that the level will be played in.
    /// </summary>
    public string LevelSceneName { get => levelSceneName; }

    private void OnValidate()
    {
        // Extract the scene name from the asset name
        string assetName = name;
        string[] parts = assetName.Split('_');
        if (parts.Length > 1)
        {
            levelSceneName = parts[1];
        }
    }
}
