using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Light))]
public class ItemPickup : MonoBehaviour
{
    private Transform _player;
    [SerializeField] private Item _item;
    [SerializeField] private Light _light;

    private void OnEnable()
    {
        _player = GameManager.Instance.player;
    }

    public void SetValues(Item item)
    {
        this._item = item;
        this._light.color = GetColorForRarity(item.ItemLevel);
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
        if(_light == null)
        {
            Debug.LogError("Light is not set in " + gameObject.name);
        }
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _player)
        {
            GameManager.Instance.ItemManager.AddItemToInventory(_item);
            Destroy(gameObject);
        }
    }


}
