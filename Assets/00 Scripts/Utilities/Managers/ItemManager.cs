using System.Collections.Generic;
using UnityEngine;

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

    void OnValidate()
    {
        _previousItems ??= new List<Item>(_equippedItems);

        foreach (Item item in _equippedItems)
        {
            if (!_previousItems.Contains(item))
            {
                UpdateStats(item, true);
            }
        }

        foreach (Item item in _previousItems)
        {
            if (!_equippedItems.Contains(item))
            {
                UpdateStats(item, false);
            }
        }

        _previousItems = new List<Item>(_equippedItems);
    }
#endif


    public void EquipItem(Item item)
    {
        _equippedItems.Add(item);
        UpdateStats(item, true);
    }

    public void UnequipItem(Item item)
    {
        _equippedItems.Remove(item);
        UpdateStats(item, false);
    }

    private void UpdateStats(Item itemUpdate, bool IsBeingAdded)
    {
        Debug.Log("Updating stats with following item:"
                  + $"\nItem:\t{itemUpdate.ItemName}"
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
    }
}
