using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class StatDisplay : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The CharStats component to get the stats from")]
    [SerializeField] private CharStats _towerStats;
    [Tooltip("The text object to display the stat names")]
    [SerializeField] private TMP_Text _statNameText;
    [Tooltip("The text object to display the stat values")]
    [SerializeField] private TMP_Text _statValueText;

    private List<Stat> statListOrder;

    //============================================== Methods ================================================== 

    private void Start()
    {
        SetupReferences();
        if (_statNameText == null)
        {
            Debug.LogError($"{_statNameText} not set. Add the reference in the inspector of a {typeof(TMP_Text)} object");
            return;
        }

        if (_statValueText == null)
        {
            Debug.LogError($"{_statValueText} not set. Add the reference in the inspector of a {typeof(TMP_Text)} object");
            return;
        }
        statListOrder = _towerStats.GetStatOrderList();
        SetNameOrder();
    }

    /// <summary>
    /// Sets up references for the StatDisplay component.
    /// If _towerStats is not set, it retrieves the CharStats component from the GameManager's Tower.
    /// </summary>
    void SetupReferences()
    {
        if (_towerStats == null)
        {
            _towerStats = GameManager.Instance.Tower.GetComponent<CharStats>();
        }
    }

    /// <summary>
    /// Updates the stat display with the current values from the tower stats.
    /// </summary>
    public void UpdateStatDisplay()
    {
        string messageBuilder = "";
        foreach (Stat stat in statListOrder)
        {
            //Debug.Log($"Stat: {stat.StatName} being checked");
            float value = Mathf.Round(_towerStats.GetStatValue(stat)*10)/10;
            messageBuilder += $"{value}\n";
        }
        _statValueText.text = messageBuilder;
    }

    /// <summary>
    /// Sets the order of stat names to be displayed.
    /// </summary>
    private void SetNameOrder()
    {
        string messageBuilder = "";
        foreach (Stat stat in statListOrder)
        {
            messageBuilder += $"{stat.StatName}:\n";
        }
        _statNameText.text = messageBuilder;
    }

    //private void OnGUI()
    //{
    //    // Create a button at the top-left corner of the screen
    //    if (GUI.Button(new Rect(10, 60, 150, 30), "Update Stats"))
    //    {
    //        UpdateStatDisplay();
    //    }
    //}
}