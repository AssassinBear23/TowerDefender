using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using System.Collections;

public class StatManager : MonoBehaviour
{
    private CharacterController towerController;
    [SerializeField] private SerializedDictionary<Stat, float> currentStats;
    private List<Item> equippedItems = new List<Item>();


    IEnumerator Start()
    { 
        towerController = GameManager.instance.Tower.GetComponent<CharacterController>();
        yield return new WaitForEndOfFrame();
        towerController.SetTowerStatValue(currentStats);
        Debug.Log("StatManager initialized");
    }

    public void EquipItem(Item item)
    {
        equippedItems.Add(item);
        UpdateStats(item, true);
    }

    public void UnequipItem(Item item)
    {
        equippedItems.Remove(item);
        UpdateStats(item, false);
    }

    private void UpdateStats(Item updatedItem, bool itemAdded)
    {
        foreach (KeyValuePair<Stat, float> stat in updatedItem.StatModifiers)
        {
            if (currentStats.ContainsKey(stat.Key) && itemAdded)
            {
                currentStats[stat.Key] += stat.Value;
            }
            else if (currentStats.ContainsKey(stat.Key) && !itemAdded)
            {
                currentStats[stat.Key] -= stat.Value;
            }
            else if (!currentStats.ContainsKey(stat.Key) && itemAdded)
            {
                currentStats.Add(stat.Key, stat.Value);
            }
        }
        UpdateTower();
    }

    private void UpdateTower()
    {
        towerController.SetTowerStatValue(currentStats);
    }
}