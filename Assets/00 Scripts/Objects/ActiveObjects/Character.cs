using AYellowpaper.SerializedCollections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Initialization Values")]
    [Tooltip("The GameManager instance in the scene")]
    private GameManager gameManager;
    [Tooltip("The spot from which the projectiles will launch from")]
    [SerializeField] private Transform _firePoint;

    // Methods for the initialization values
    public Transform FirePoint { get => _firePoint; }

    [Header("Tower Stats")]
    [Tooltip("The stats of the tower")]
    [SerializeField] private SerializedDictionary<Stat, float> _towerStats;
    /// <summary>
    /// Get the value for a certain tower stat.
    /// </summary>
    /// <param name="key">The stat to get the value for</param>
    /// <returns>The value associated with the stat</returns>
    public float GetTowerStatValue(Stat key)
    {
        return _towerStats[key];
    }

    /// <summary>
    /// Set the tower stats to the given dictionary.
    /// </summary>
    /// <param name="dict">The dictionary to set the tower stats to</param>
    public void SetTowerStatValue(SerializedDictionary<Stat, float> dict)
    {
        if (dict == null)
        {
            Debug.Log("Passed dictionary is null.");
            return;
        }
        if (dict.Count != _towerStats.Count && _towerStats.Count != 0)
        {
            Debug.Log($"{dict} does not have the same amount of key value pairs as {_towerStats}, being {_towerStats.Count}.");
            return;
        }
        _towerStats = dict;
    }

    

    private void Start()
    {
        GetReferences();
        GameManager.instance.Tower = gameObject;
    }

    /// <summary>
    /// Get the necessary references for the character controller.
    /// </summary>
    void GetReferences()
    {
        if (inputManager == null)
        {
            inputManager = InputManager.instance;
        }
        if (gameManager == null)
        {
            gameManager = GameManager.instance;
        }
    }

    private void PlayerDied()
    {
        PlayerDeath?.Invoke();
    }
}
