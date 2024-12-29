using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Item _item;
    /// <summary>
    /// The item that this inventoryslot holds
    /// </summary>
    public Item Item { get => _item; set => _item = value; }

    [SerializeField] private Image _itemIcon;
    [SerializeField] private UnityEngine.UI.Button _deleteButton;

    private void Start()
    {
        _deleteButton.enabled = false;
    }

    /// <summary>
    /// Updates the slot if there is a change in condition.
    /// </summary>
    public void UpdateSlot()
    {
        if (!IsUpdateNeeded())
        {
            Debug.Log("No Update is needed for " + gameObject.name);
            return;
        }

        if (_item == null)
        {
            _itemIcon.enabled = false;
            _deleteButton.enabled = false;
            return;
        }
        else
        {
            _itemIcon.sprite = _item.GetItemIcon;
            _itemIcon.enabled = true;
            _deleteButton.enabled = true;
        }
    }


    /// <summary>
    /// Checks if the slot needs to be updated.
    /// </summary>
    /// <returns>True if update is needed, false otherwise.</returns>
    private bool IsUpdateNeeded()
    {
        if ((_item != null && _itemIcon.sprite == null) | (_item == null && _itemIcon.sprite != null))
        {
            return true;
        }
        return false;
    }

    private void OnValidate()
    {
        if (_itemIcon == null)
        {
            Debug.LogError(nameof(_itemIcon) + " is not set in " + gameObject.name);
        }
        if (_deleteButton == null)
        {
            Debug.LogError(nameof(_deleteButton) + " is not set in " + gameObject.name);
        }
    }
}
