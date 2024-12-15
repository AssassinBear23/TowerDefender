using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave_xx", menuName = "Spawner/Wave")]
public class Wave : ScriptableObject
{
    [Header("Wave Settings")]
    [Tooltip("Name to show what kind of wave it is.\ne.g: Pure melee")]
    [SerializeField] private string waveName;
    [Tooltip("The enemies to spawn from all sides in the order in the list.")]
    [SerializeField] private List<BaseEnemy> enemies;
    [Tooltip("Is this a mini boss battle?")]
    [SerializeField] private bool isMiniBossWave;
    [Tooltip("Is this a boss battle?")]
    [SerializeField] private bool isBossWave;
}
