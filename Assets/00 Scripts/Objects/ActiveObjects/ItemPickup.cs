using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider), typeof(Light))]
public class ItemPickup : MonoBehaviour
{
    private Transform _player;
    [SerializeField] private Item _item;

    private void OnEnable()
    {
        _player = GameManager.Instance.player;
    }

    public ItemPickup(Item item)
    {
        this._item = item;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != _player) return;

        // If there is space in the inventory, then add it. Otherwise do nothing
        if (GameManager.Instance.ItemManager.AddItemToInventory(_item))
        {
            Destroy(gameObject);
        }
        else
        {
            // Tell player inventory is full
            Debug.Log("Inventory is full");
        }
    }
}
