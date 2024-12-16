using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Initialization Values")]
    [Tooltip("List of the spawners")]
    [SerializeField] private List<Transform> spawners;
    [Tooltip("The speed at which to spawn")]
    [SerializeField] private float spawnSpeed;
}
