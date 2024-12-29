using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Light))]
public class ItemPickup : MonoBehaviour
{
    private Transform _player;
    [SerializeField] private Item _item;
    [SerializeField] private Light _light;
    [SerializeField] private Material _material;

    public static Action ItemPickedUp;

    private void OnEnable()
    {
        _player = GameManager.Instance.player;
    }

    public void SetValues(Item item)
    {
        this._item = item;
        Color color = GetColorForRarity(item.ItemLevel);
        this._light.color = color;
        this._material.color = color;
    }

    private Color GetColorForRarity(int itemLevel)
    {
        switch (itemLevel)
        {
            case 1:
                return Color.white;
            case 2:
                return Color.blue;
            case 3:
                return Color.magenta;
            default:
                return Color.white;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _light = GetComponent<Light>();
        if (_light == null)
        {
            Debug.LogError("Light is not set in " + gameObject.name);
        }
    }
#endif

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
