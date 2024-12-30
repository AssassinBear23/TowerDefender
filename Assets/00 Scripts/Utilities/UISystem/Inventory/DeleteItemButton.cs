using UnityEngine;

public class DeleteItemButton : MonoBehaviour
{
    private Item _item;

    public void RemoveItem()
    {
        InventorySlot slot = GetComponentInParent<InventorySlot>();
        _item = slot.Item;

        if (_item == null)
        {
            Debug.Log($"Item in {slot.gameObject.name} is {_item}");
            return;
        }

        GameManager.Instance.ItemManager.RemoveItemFromInventory(_item);
    }
}
