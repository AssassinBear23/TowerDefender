using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Initialization Values")]
    [Tooltip("List of the spawners")]
    [SerializeField] private List<Transform> spawners;
}
