using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider), typeof(Light))]
public class ItemPickup : MonoBehaviour
{
    private Transform _player;
    [SerializeField] private Item _item;

    [SerializeField] private Light _light;
    [SerializeField] private Material _materialPrefab;
    private Material _material;

    [SerializeField] private MeshRenderer _mr;

    [Header("Color Options")]
    [SerializeField] private Color _commonColor;
    [SerializeField] private Color _rareColor;
    [SerializeField] private Color _epicColor;

    [SerializeField][Range(0, 1f)] private float _materialAlpha = 0.5f;
    public void SetItem (Item item) { _item = item; }

    private void Start()
    {
        _player = GameManager.Instance.player;
        _material = new Material(_materialPrefab);
        _mr.material = _material;
        SetColor(GetRarityColor());
    }

    private Color GetRarityColor()
    {
        ItemRarity rarity = _item.ItemLevel;
        Color color = rarity switch
        {
            ItemRarity.Common => _commonColor,
            ItemRarity.Rare => _rareColor,
            ItemRarity.Epic => _epicColor,
            _ => _commonColor
        };

        return color;
    }

    private void SetColor(Color color)
    {
        Debug.Log("Setting color to " + color);
        _light.color = color;
        color.a = _materialAlpha;
        _material.color = color;
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

    private void OnValidate()
    {
        _material = new Material(_materialPrefab);
        _mr.material = _material;
        if (_item != null)
            SetColor(GetRarityColor());
    }
}
