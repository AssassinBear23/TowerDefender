using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Core/Tower/Item")]
public class Item : ScriptableObject
{
    [Header("Item Information")]
    [Tooltip("The name of the item")]
    public string ItemName;

    [Tooltip("The icon of the item in the UI")]
    [SerializeField] private List<Sprite> _itemIcons;

    /// <summary>
    /// Gets the icon of the item for the specified level.
    /// </summary>
    /// <param name="level">The level of the item.</param>
    /// <returns>The sprite icon of the item for the specified level, or null if the level is invalid.</returns>
    public Sprite GetItemIcon(int level)
    {
        if (!CheckLevel(level)) return null;
        return _itemIcons[level];
    }

    [Tooltip("The stats of the item")]
    [SerializeField] private SerializedDictionary<int, SerializedDictionary<Stat, float>> _statModifiers;

    /// <summary>
    /// Gets the stat modifiers of the item for the specified level.
    /// </summary>
    /// <param name="level">The level of the item.</param>
    /// <returns>A dictionary of stat modifiers for the specified level, or null if the level is invalid.</returns>
    public Dictionary<Stat, float> GetStatModifiers(int level)
    {
        if (!CheckLevel(level)) return null;
        return _statModifiers[level];
    }

    [Tooltip("The description of the item")]
    [SerializeField] private string _itemDescription;

    /// <summary>
    /// Gets the description of the item.
    /// </summary>
    public string ItemDescription { get => _itemDescription; }

    /// <summary>
    /// Checks if the specified level is valid.
    /// </summary>
    /// <param name="obj">The level to check.</param>
    /// <returns>True if the level is valid, otherwise false.</returns>
    private bool CheckLevel(int obj)
    {
        if (obj < 1 && obj > 3)
        {
            Debug.LogError("Invalid level for item stat modifiers");
            return false;
        }
        return true;
    }
}
