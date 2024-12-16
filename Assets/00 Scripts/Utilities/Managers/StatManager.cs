using UnityEngine;
using System.Collections.Generic;

public class StatManager : MonoBehaviour
{
    private TowerController towerController;
    private Dictionary<Stat, float> currentStats = new Dictionary<Stat, float>();
    private List<Item> equippedItems = new List<Item>();

    private void Start()
    {
        towerController = GameManager.instance.Tower.GetComponent<TowerController>();
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
    }
}