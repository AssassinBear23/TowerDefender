using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Item item;

    private bool _hadItem;

    private Image _itemIcon;
    private EventTrigger _eventTrigger;

    private bool _isInventory = false;

    private void Start()
    {
        _eventTrigger = GetComponent<EventTrigger>();
        _itemIcon = transform.GetComponentInChildren<Image>();
        _eventTrigger.enabled = false;
    }

    public void MoveItem()
    {
        Debug.Log("Item moved");
        if (_isInventory)
        {
            if (!GameManager.Instance.ItemManager.EquipItem(item))
            {
                // Tell Player that the item could not be equipped
            }
        }
        else
        {
            if (!GameManager.Instance.ItemManager.UnequipItem(item))
            {
                // Tell Player that the item could not be unequipped
            }
        }
    }

    public void UpdateSlot()
    {
        if (!IsUpdateNeeded())
        {
            Debug.Log("No Update is needed for " + gameObject.name);
            return;
        }

        if (item == null)
        {
            _itemIcon.sprite = null;
            _itemIcon.enabled = false;
            _eventTrigger.enabled = false;
            return;
        }
        else
        {
            _itemIcon.sprite = item.GetItemIcon;
            _itemIcon.enabled = true;
            _eventTrigger.enabled = true;
        }
    }

    private bool IsUpdateNeeded()
    {
        if (item == null && _hadItem)
        {
            _hadItem = false;
            return true;
        }
        else if (item != null && !_hadItem)
        {
            _hadItem = true;
            return true;
        }
        return false;
    }

    //#if UNITY_EDITOR
    //    private void OnValidate()
    //    {
    //        _isInventory = transform.parent.parent.name.Contains("Inventory");
    //    }
    //#endif
}
