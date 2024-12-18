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

    void OnValidate()
    {
        _previousItems ??= new SerializedDictionary<Item, int>(_equippedItems);

        Debug.Log("OnValidate called" +
            $"\n_previousItems:\t{_previousItems.Keys} ");

        foreach (Item item in _equippedItems.Keys)
        {
            
            if (!_previousItems.ContainsKey(item))
            {
                UpdateStats(item, _equippedItems[item], true);
            }
        }
        foreach (Item item in _previousItems.Keys)
        {
            if (!_equippedItems.ContainsKey(item))
            {
                UpdateStats(item, _equippedItems[item], false);
            }
        }
        _previousItems = _equippedItems;
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
