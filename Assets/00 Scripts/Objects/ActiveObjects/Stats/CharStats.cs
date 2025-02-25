using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharStats : MonoBehaviour
{
    [Header("Char Stats")]
    [Tooltip("The stats of the tower")]
    [SerializeField] private SerializedDictionary<Stat, float> _charStats;

    [Space(10)]

    [SerializeField] private CharTypes _charType;
    public CharTypes CharType { get => _charType; }

    public UnityEvent StatChange; 

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
    /// Gets the list of stats in the order they are stored.
    /// </summary>
    /// <returns>A list of stats in the order they are stored.</returns>
    public List<Stat> GetStatOrderList()
    {
        List<Stat> statOrder = new List<Stat>();
        foreach (Stat stat in _charStats.Keys)
        {
            statOrder.Add(stat);
        }
        return statOrder;
    }


    /// <summary>
    /// Sets the value for a specific stat.
    /// </summary>
    /// <param name="key">The stat to set the value for.</param>
    /// <param name="value">The value to set for the specified stat.</param>
    public void SetStatValue(Stat key, float value)
    {
        _charStats[key] = value;
    }

    /// <summary>
    /// Set the tower stats to the given dictionary.
    /// </summary>
    /// <param name="dict">The dictionary to set the tower stats equal to</param>
    public void SetStatDict(SerializedDictionary<Stat, float> dict)
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

public enum CharTypes
{
    Tower,
    Enemy
}