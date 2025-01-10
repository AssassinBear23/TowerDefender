using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTable", menuName = "Core/Items/ItemTable", order = 1)]
public class ItemTable : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField] List<Item> items;
#endif

    [HideInInspector][SerializeField] private List<Item> _commonItems = new();
    [HideInInspector][SerializeField] private List<Item> _rareItems = new();
    [HideInInspector][SerializeField] private List<Item> _epicItems = new();

    /// <summary>
    /// Gets a random item from the specified rarity category.
    /// </summary>
    /// <param name="rarity">The rarity category of the item.</param>
    /// <returns>A random item from the specified rarity category, or null if the category is invalid.</returns>
    public Item GetRandomItem(ItemRarity rarity)
    {
        // Determine the list of items based on the rarity
        List<Item> _items = rarity switch
        {
            ItemRarity.Common => _commonItems, // Common items list
            ItemRarity.Rare => _rareItems,     // Rare items list
            ItemRarity.Epic => _epicItems,     // Epic items list
            _ => null                          // Invalid rarity
        };

        // If the list is null, return null
        if (_items == null)
        {
            Debug.LogError($"Item list for rarity {rarity} is null.");
            return null;
        }

        // If the list is empty, return null
        if (_items.Count == 0)
        {
            Debug.LogError($"Item list for rarity {rarity} is empty.");
            return null;
        }

        // Log the items in the list
        Debug.Log($"Items in the list for rarity {rarity}:");
        foreach (var item in _items)
        {
            Debug.Log(item.name);
        }

        // Return a random item from the list
        return _items[Random.Range(0, _items.Count)];
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Ensure that the item lists are not null
        if (items == null) return;

        _commonItems = new List<Item>();
        _rareItems = new List<Item>();
        _epicItems = new List<Item>();

        // Go through every item in the items list and add it to the appropriate list
        foreach (Item item in items)
        {
            switch (item.ItemLevel)
            {
                case ItemRarity.Common:
                    _commonItems.Add(item);
                    break;
                case ItemRarity.Rare:
                    _rareItems.Add(item);
                    break;
                case ItemRarity.Epic:
                    _epicItems.Add(item);
                    break;
            }
        }

        // Log the items in each list
        Debug.Log("Common items:");
        foreach (var item in _commonItems)
        {
            Debug.Log(item.name);
        }

        Debug.Log("Rare items:");
        foreach (var item in _rareItems)
        {
            Debug.Log(item.name);
        }

        Debug.Log("Epic items:");
        foreach (var item in _epicItems)
        {
            Debug.Log(item.name);
        }
    }
#endif
}
