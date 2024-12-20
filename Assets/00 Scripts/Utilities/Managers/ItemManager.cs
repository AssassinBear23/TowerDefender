using System.Collections.Generic;
using UnityEngine;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemManager : MonoBehaviour
{
    private CharStats tower;
    [SerializeField] private List<Item> _equippedItems = new();

    void Start()
    {
        tower = GameManager.instance.Tower.GetComponent<CharStats>();
        Debug.Log("StatManager initialized");
    }

#if UNITY_EDITOR
    private List<Item> _previousItems = new();

    /// <summary>
    /// Called when the script is loaded or a value is changed in the Inspector.
    /// This method ensures that the stats are updated correctly when items are added or removed in the editor.
    /// </summary>
    void OnValidate()
    {
        // Initialize _previousItems if it is null
        _previousItems ??= new List<Item>(_equippedItems);

        // Iterate through the currently equipped items
        foreach (Item item in _equippedItems)
        {
            if (!_previousItems.Contains(item))

            {
                UpdateStats(item, true);
            }
        }

        // Iterate through the previously equipped items
        foreach (Item item in _previousItems)
        {
            if (!_equippedItems.Contains(item))
            {
                UpdateStats(item, false);
            }
        }

        // Update _previousItems to the current state of _equippedItems
        _previousItems = new List<Item>(_equippedItems);
    }
#endif


    public void EquipItem(Item item)
    {
        _equippedItems.Add(item);
        UpdateStats(item, true);
    }

    public void UnequipItem(Item item, int itemLevel)
    {
        _equippedItems.Remove(item);
        UpdateStats(item, false);
    }

    private void UpdateStats(Item itemUpdate, bool IsBeingAdded)
    {
        if(itemUpdate == null)
        {
            Debug.Log("No item passed for UpdateStats(). Make sure the passed Item is not Null.");
        }
        Debug.Log("Updating stats with following item:"
                  + $"\nItem:\t{itemUpdate.ItemName}"
                  + $"\nLevel:\t{itemUpdate.ItemLevel}"
                  + $"\nAdding:\t{IsBeingAdded}");
        if (IsBeingAdded)
        {
            foreach (KeyValuePair<Stat, float> stat in itemUpdate.StatModifiers)
            {
                if (stat.Value == 0) return;
                float _currentValue = tower.GetStatValue(stat.Key);
                tower.SetStatValue(stat.Key, _currentValue + stat.Value);
            }
        }
        else
        {
            foreach (KeyValuePair<Stat, float> stat in itemUpdate.StatModifiers)
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
        GameManager.instance.uiManager.UpdateStats();
    }
}
