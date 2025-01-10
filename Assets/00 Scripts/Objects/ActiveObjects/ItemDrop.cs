using System.Collections;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private Item _heldItem;

    [Header("Item Drop Settings")]
    [SerializeField] private float _dropChanceAtPeak = 50f;
    [Space(10)]
    [SerializeField] private float _rareItemChance = 30f;
    [SerializeField] private float _epicItemChance = 10f;

    [SerializeField] private GameObject _itemDropPrefab;

    [SerializeField] private ItemTable _itemTable;

    private float _startDistanceSquared;
    private float _deathDistanceSquared;

    private float _dropChance;

    public void OnCharacterDeath()
    {
        if (!TryGetComponent<AbstractEnemy>(out var _char))
        {
            Debug.LogError("AbstractEnemy component is missing.");
            return;
        }

        _startDistanceSquared = _char.TotalDistance;
        Vector3 _towerPosition = GameManager.Instance?.Tower?.transform.position ?? Vector3.zero;
        if (GameManager.Instance == null || GameManager.Instance.Tower == null)
        {
            Debug.LogError("GameManager.Instance or GameManager.Instance.Tower is null.");
            return;
        }

        Vector2 _charPositionn = new(_char.transform.position.x, _char.transform.position.z);
        _deathDistanceSquared = (_charPositionn - new Vector2(_towerPosition.x, _towerPosition.z)).sqrMagnitude;

        Debug.Log($"{nameof(_startDistanceSquared)}: {_startDistanceSquared}"
                  + $"\n{nameof(_deathDistanceSquared)}: {_deathDistanceSquared}"
                  + $"\nRatio: {_deathDistanceSquared / _startDistanceSquared}");

        _dropChance = _dropChanceAtPeak * (1 - (_deathDistanceSquared / _startDistanceSquared));

        Debug.Log($"Drop chance for {gameObject.name}: {_dropChance}");

        if (!RollForDrop()) return;

        if (_itemTable == null)
        {
            Debug.LogError("_itemTable is not assigned.");
            return;
        }

        Item _item = _itemTable.GetRandomItem(RollForRarity());
        if (_item == null)
        {
            Debug.LogError("GetRandomItem returned null.");
            return;
        }

        Debug.Log($"Instantiating {_item.ItemName}...");
        if (_itemDropPrefab == null)
        {
            Debug.LogError("_itemDropPrefab is not assigned.");
            return;
        }

        ItemPickup _itemPickupComponent = Instantiate(_itemDropPrefab, transform.position, Quaternion.identity).GetComponent<ItemPickup>();
        if (_itemPickupComponent == null)
        {
            Debug.LogError("ItemPickup component is missing on _itemDropPrefab.");
            return;
        }

        _itemPickupComponent.SetItem(_item);
    }

    private bool RollForDrop()
    {
        float _roll = Random.Range(0f, 100f);
        if (_roll <= _dropChance)
        {
            Debug.Log("You got an item!");
            return true;
        }
        Debug.Log("Fuck you");
        return false;
    }

    private ItemRarity RollForRarity()
    {
        float _roll = Random.Range(0f, 100f);
        if (_roll <= _epicItemChance)
        {
            Debug.Log("Item is epic");
            return ItemRarity.Epic;
        }
        if (_roll < _rareItemChance)
        {
            Debug.Log("Item is rare");
            return ItemRarity.Rare;
        }
        Debug.Log("Item is common");
        return ItemRarity.Common;
    }
}

public enum ItemRarity
{
    Common,
    Rare,
    Epic
}
