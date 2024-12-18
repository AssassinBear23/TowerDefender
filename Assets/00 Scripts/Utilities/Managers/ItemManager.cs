using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemManager : MonoBehaviour
{
    private CharStats tower;
    [SerializeField] private SerializedDictionary<Item, int> _equippedItems = new();

    void Start()
    {
        tower = GameManager.instance.Tower.GetComponent<CharStats>();
        Debug.Log("StatManager initialized");
    }

#if UNITY_EDITOR
    private SerializedDictionary<Item, int> _previousItems = new();


    /// <summary>
    /// Called when the script is loaded or a value is changed in the Inspector.
    /// This method ensures that the stats are updated correctly when items are added or removed in the editor.
    /// </summary>
    void OnValidate()
    {
        // Initialize _previousItems if it is null
        _previousItems ??= new SerializedDictionary<Item, int>(_equippedItems);

        // Iterate through the currently equipped items
        foreach (Item item in _equippedItems.Keys)
        {
            Debug.Log($"Checking item:\t{item.ItemName}"
                + $"\nDoes old dic contain key?:\t{_previousItems.ContainsKey(item)}"
                + $"\nAre the values different?:\t{(_previousItems.ContainsKey(item) ? _previousItems[item] : 0) != _equippedItems[item]}"
                + $"\nOld value:\t{(_previousItems.ContainsKey(item) ? _previousItems[item] : 0)}"
                + $"\nNew value:\t{_equippedItems[item]}"
                );
            // Update stats if the item is new or its level has changed
            if (!_previousItems.ContainsKey(item) || (_previousItems.ContainsKey(item) && (_previousItems[item] != _equippedItems[item])))
            {
                UpdateStats(item, _equippedItems[item], true);
            }
        }

        // Iterate through the previously equipped items
        foreach (Item item in _previousItems.Keys)
        {
            // Update stats if the item is no longer equipped or its level has changed
            if (!_equippedItems.ContainsKey(item) || (_equippedItems.ContainsKey(item) && (_previousItems[item] != _equippedItems[item])))
            {
                UpdateStats(item, _previousItems[item], false);
            }
        }

        // Update _previousItems to the current state of _equippedItems
        _previousItems = new SerializedDictionary<Item, int>(_equippedItems);
    }
#endif


    public void EquipItem(Item item, int itemLevel)
    {
        _equippedItems.Add(item, itemLevel);
        UpdateStats(item, itemLevel, true);
    }

    public void UnequipItem(Item item, int itemLevel)
    {
        _equippedItems.Remove(item);
        UpdateStats(item, itemLevel, false);
    }

    private void UpdateStats(Item itemUpdate, int itemLevel, bool IsBeingAdded)
    {
        Debug.Log("Updating stats with following item:"
                  + $"\nItem:\t{itemUpdate.ItemName}" 
                  + $"\nLevel:\t{itemLevel}"
                  + $"\nAdding:\t{IsBeingAdded}");
        if (IsBeingAdded)
        {
            foreach (KeyValuePair<Stat, float> stat in itemUpdate.GetStatModifiers(itemLevel))
            {
                if (stat.Value == 0) return;
                float _currentValue = tower.GetStatValue(stat.Key);
                tower.SetStatValue(stat.Key, _currentValue + stat.Value);
            }
        }
        else
        {
            foreach (KeyValuePair<Stat, float> stat in itemUpdate.GetStatModifiers(itemLevel))
            {
                if (stat.Value == 0) return;
                float _currentValue = tower.GetStatValue(stat.Key);
                if (_currentValue - stat.Value < 0)
                {
                    tower.SetStatValue(stat.Key, 0);
                    continue;
                }
                tower.SetStatValue(stat.Key, _currentValue - stat.Value);
            }
        }
    }
}
