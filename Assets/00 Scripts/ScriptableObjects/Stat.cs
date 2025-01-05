using UnityEngine;

[CreateAssetMenu(fileName = "NewStat", menuName = "Core/Tower/Stat")]
public class Stat : ScriptableObject
{
    [SerializeField] private string statName;
    public string StatName { get => statName; }
}
