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
    [SerializeField] private GameObject _deleteButton;

    private void Start()
    {
        _deleteButton.SetActive(false);
    }

    /// <summary>
    /// Updates the slot if there is a change in condition.
    /// </summary>
    public void UpdateSlot()
    {
        if (_item == null)
        {
            _itemIcon.enabled = false;
            _deleteButton.SetActive(false);
            return;
        }
        else
        {
            Debug.Log("Updating with icon != null for " + gameObject.name);
            _itemIcon.sprite = _item.GetItemIcon;
            _itemIcon.enabled = true;
            _deleteButton.SetActive(true);
        }
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
