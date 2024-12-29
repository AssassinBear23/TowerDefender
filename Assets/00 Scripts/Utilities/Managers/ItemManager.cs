using System.Collections.Generic;
using UnityEngine;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemManager : MonoBehaviour
{
    private CharStats tower;
    [Header("Inventory Settings")]
    [SerializeField] private List<Item> _inventory = new();
    [Space(10)]
    [SerializeField] int _inventoryCapacity = 15;

    [Header("General Settings")]
    [SerializeField] private Transform _inventorySlotsParent;

    private InventorySlot[] _inventorySlots;



    IEnumerator Start()
    {
        tower = GameManager.Instance.Tower.GetComponent<CharStats>();
        Debug.Log("StatManager initialized");
        GetLists();
        yield return new WaitForEndOfFrame();
        GameManager.Instance.UIManager.UpdateStats();
    }

    void GetLists()
    {
        _inventorySlots = _inventorySlotsParent.GetComponentsInChildren<InventorySlot>();
    }

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="obj">The item to be added to the inventory.</param>
    /// <returns>True if the item was successfully added, otherwise false.</returns>
    public bool AddItemToInventory(Item obj)
    {
        if (obj == null)
        {
            Debug.Log("Passed item is null.");
            return false;
        }
        if (IsInventoryFull())
        {
            return false;
        }

        _inventory.Add(obj);
        UpdateSlots();
        UpdateStats(obj, true);
        return true;
    }

    /// <summary>
    /// Checks if the inventory is full.
    /// </summary>
    /// <returns>True if the inventory is full, otherwise false.</returns>
    private bool IsInventoryFull()
    {
        if (_inventory.Count == _inventoryCapacity)
        {
            Debug.Log("Inventory is Full. Cannot add item to inventory");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Updates the inventory slots to reflect the current state of the inventory.
    /// </summary>
    private void UpdateSlots()
    {
        for (int i = 0; i < _inventory.Count; i++)
        {
            _inventorySlots[i].Item = _inventory[i];
            _inventorySlots[i].UpdateSlot();
        }
        for (int i = _inventory.Count - 1; i < _inventorySlots.Length; i++)
        {
            _inventorySlots[i].Item = null;
            _inventorySlots[i].UpdateSlot();
        }
    }

    /// <summary>
    /// Updates the character's stats based on the item being equipped or unequipped.
    /// </summary>
    /// <param name="item">The item that is being equipped or unequipped.</param>
    /// <param name="IsBeingAdded">True if the item is added, false if removed.</param>
    private void UpdateStats(Item item, bool IsBeingAdded)
    {
        if (item == null)
        {
            Debug.Log("No item passed for UpdateStats(). Make sure the passed Item is not Null.");
        }
        Debug.Log("Updating stats with following item:"
                  + $"\nItem:\t{item.ItemName}"
                  + $"\nLevel:\t{item.ItemLevel}"
                  + $"\nAdding:\t{IsBeingAdded}");
        if (IsBeingAdded)
        {
            foreach (KeyValuePair<Stat, float> stat in item.StatModifiers)
            {
                if (stat.Value == 0) return;
                float _currentValue = tower.GetStatValue(stat.Key);
                tower.SetStatValue(stat.Key, _currentValue + stat.Value);
            }
        }
        else
        {
            foreach (KeyValuePair<Stat, float> stat in item.StatModifiers)
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
        GameManager.Instance.UIManager.UpdateStats();
    }
}