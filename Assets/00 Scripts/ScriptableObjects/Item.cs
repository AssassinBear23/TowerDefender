using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Core/Items/Item")]
public class Item : ScriptableObject
{
    [Header("Item Information")]
    [Tooltip("The name of the item")]
    public string ItemName;

    [Tooltip("The icon of the item in the UI")]
    [SerializeField] private Sprite _itemIcon;
    public Sprite GetItemIcon { get => _itemIcon;  }

    [Tooltip("The tier of the Item")]
    [SerializeField] private ItemRarity _itemLevel;
    public ItemRarity ItemLevel { get => _itemLevel; }

    [Tooltip("The stats of the item")]
    [SerializeField] private SerializedDictionary<Stat, float> _statModifiers;

    /// <summary>
    /// Gets the stat modifiers of the item for the specified level.
    /// </summary>
    /// <param name="level">The level of the item.</param>
    /// <returns>A dictionary of stat modifiers for the specified level, or null if the level is invalid.</returns>
    public SerializedDictionary<Stat, float> StatModifiers{ get => _statModifiers; }

    [Tooltip("The description of the item")]
    [SerializeField] private string _itemDescription;
    /// <summary>
    /// Gets the description of the item.
    /// </summary>
    public string ItemDescription { get => _itemDescription; }
}
