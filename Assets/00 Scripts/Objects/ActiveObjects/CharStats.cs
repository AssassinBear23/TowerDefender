using AYellowpaper.SerializedCollections;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    [Header("Char Stats")]
    [Tooltip("The stats of the tower")]
    [SerializeField] private SerializedDictionary<Stat, float> _charStats;
    /// <summary>
    /// Get the value for a certain tower stat.
    /// </summary>
    /// <param name="key">The stat to get the value for</param>
    /// <returns>The value associated with the stat</returns>
    public float GetStatValue(Stat key)
    {
        return _charStats[key];
    }

    /// <summary>
    /// Set the tower stats to the given dictionary.
    /// </summary>
    /// <param name="dict">The dictionary to set the tower stats to</param>
    public void SetStatValues(SerializedDictionary<Stat, float> dict)
    {
        if (dict == null)
        {
            Debug.Log("Passed dictionary is null.");
            return;
        }
        if (dict.Count != _charStats.Count && _charStats.Count != 0)
        {
            Debug.Log($"{dict} does not have the same amount of key value pairs as {_charStats}, being {_charStats.Count}.");
            return;
        }
        _charStats = dict;
    }
}
