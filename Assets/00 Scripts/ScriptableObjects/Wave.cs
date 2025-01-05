using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveName", menuName = "Core/LevelData/Wave")]
public class Wave : ScriptableObject
{
    [Header("Wave Settings")]
    [Tooltip("The enemies to spawn from all sides in the order in the list.")]
    [SerializeField] private List<GameObject> enemies;
    public List<GameObject> Enemies { get => enemies; }
    [Tooltip("Is this a mini boss battle?")]
    [SerializeField] private bool isMiniBossWave;
    [Tooltip("Is this a boss battle?")]
    [SerializeField] private bool isBossWave;
}
