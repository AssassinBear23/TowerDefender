using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("Health Values")]
    [SerializeField] private Stat healthStatAsset;
    [SerializeField] private float _currentHealth;
}
