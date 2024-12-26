using System.Collections.Generic;
using UnityEngine;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemManager : MonoBehaviour
{
    private CharStats tower;
    [Header("Inventory Lists")]
    [SerializeField] private List<Item> _equippedItems = new();
    [SerializeField] private List<Item> _inventory = new();
    [Space(10)]
    [SerializeField] int _inventoryCapacity = 15;
    [SerializeField] int _equippedCapacity = 10;

    [Header("UI Settings")]
    [SerializeField] private Transform _inventorySlotsParent;
    [SerializeField] private Transform _equippedSlotsParent;

    private List<InventorySlot> _inventorySlots;
    private List<InventorySlot> _equippedSlots;


    void Start()
    {
        tower = GameManager.Instance.Tower.GetComponent<CharStats>();
        Debug.Log("StatManager initialized");
        GetLists();
    }

    void GetLists()
    {
        _inventorySlots = new List<InventorySlot>();
        _equippedSlots = new List<InventorySlot>();
        _inventorySlots = GetComponentsInChildren<InventorySlot>().ToList();
        _equippedSlots = GetComponentsInChildren<InventorySlot>().ToList();
    }

    void UpdateSlots(List<InventorySlot> list)
    {
        foreach (InventorySlot slot in list)
        {
            slot.UpdateSlot();
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 110, 150, 30), "Update Slots"))
        {
            UpdateSlots();
        }
    }


    void UpdateSlots()
    {
        UpdateSlots(_inventorySlots);
        UpdateSlots(_equippedSlots);
    }




    /// <summary>
    /// Checks if all equipped slots are full.
    /// </summary>
    /// <returns>True if all equipped slots are full, otherwise false.</returns>
    private bool IsEquipedSlotsFull()
    {
        if (_equippedItems.Count == _equippedCapacity)
        {
            Debug.Log("All slots are occupied. Cannot equip item.");
            return true;
        }
        return false;
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
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add to the inventory.</param>
    /// <returns>True if the item was added successfully, otherwise false.</returns>
    public bool AddItemToInventory(Item item)
    {
        if (!IsInventoryFull())
        {
            _inventory.Add(item);
            UpdateSlots(_inventorySlots);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes an item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove from the inventory.</param>
    /// <remarks>
    /// If the item is not found in the inventory, an error message is logged.
    /// </remarks>
    public void RemoveItemFromInventory(Item item)
    {
        if (!_inventory.Contains(item))
        {
            Debug.LogError("Item not found in inventory. Cannot remove item.");
            UpdateSlots(_inventorySlots);
            return;
        }
        _inventory.Remove(item);
    }

    /// <summary>
    /// Equips an item to the character.
    /// </summary>
    /// <param name="item">The item to equip.</param>
    /// <returns>True if the item was equipped successfully, otherwise false.</returns>
    public bool EquipItem(Item item)
    {
        if (!IsEquipedSlotsFull())
        {
            _equippedItems.Add(item);
            UpdateSlots(_equippedSlots);
            UpdateStats(item, true);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Unequips an item from the character.
    /// </summary>
    /// <param name="item">The item to unequip.</param>
    /// <returns>True if the item was unequipped successfully, otherwise false.</returns>
    /// <remarks>
    /// If the item is not found in the equipped items or the inventory is full, an error message is logged.
    /// </remarks>
    public bool UnequipItem(Item item)
    {
        if (!_equippedItems.Contains(item))
        {
            Debug.LogError("Item not found in equipped items. Cannot unequip item.");
            return false;
        }
        // if Inventory is not full, then unequip the item
        if (!IsInventoryFull())
        {
            _equippedItems.Remove(item);
            _inventory.Add(item);
            UpdateSlots();
            UpdateStats(item, false);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Updates the character's stats based on the item being equipped or unequipped.
    /// </summary>
    /// <param name="item">The item that is being equipped or unequipped.</param>
    /// <param name="IsBeingAdded">True if the item is being equipped, false if it is being unequipped.</param>
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
}