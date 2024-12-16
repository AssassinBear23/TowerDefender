using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "NewItem", menuName = "Core/Tower/Item")]
public class Item : ScriptableObject
{
    [Header("Item Information")]
    [Tooltip("The name of the item")]
    public string ItemName;
    [Tooltip("The icon of the item in the UI")]
    [SerializeField] private Sprite _itemIcon;
    [Tooltip("The stats of the item")]
    [SerializeField] private SerializedDictionary<Stat, float> _statModifiers;
    public Dictionary<Stat, float> StatModifiers { get => _statModifiers; }
    [Tooltip("The description of the item")]
    [SerializeField] private string _itemDescription;
}