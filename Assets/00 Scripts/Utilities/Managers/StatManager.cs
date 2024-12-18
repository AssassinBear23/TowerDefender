using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using System.Collections;

public class StatManager : MonoBehaviour
{
    private CharStats tower;
    [SerializeField] private SerializedDictionary<Stat, float> _currentPlayerStats;
    private List<Item> _equippedItems = new List<Item>();


    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        tower = GameManager.instance.Tower.GetComponent<CharStats>();
        tower.SetStatValues(_currentPlayerStats);
        Debug.Log("StatManager initialized");
    }

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

    private void UpdateStats(Item updatedItem, bool itemAdded)
    {
        foreach (KeyValuePair<Stat, float> stat in updatedItem.StatModifiers)
        {
            if (_currentPlayerStats.ContainsKey(stat.Key) && itemAdded)
            {
                _currentPlayerStats[stat.Key] += stat.Value;
            }
            else if (_currentPlayerStats.ContainsKey(stat.Key) && !itemAdded)
            {
                _currentPlayerStats[stat.Key] -= stat.Value;
            }
            else if (!_currentPlayerStats.ContainsKey(stat.Key) && itemAdded)
            {
                _currentPlayerStats.Add(stat.Key, stat.Value);
            }
        }
        UpdateTower();
    }

    private void UpdateTower()
    {
        tower.SetStatValues(_currentPlayerStats);
    }
}