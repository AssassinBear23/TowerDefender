using TMPro;
using UnityEngine;

public class DisplayScore : UIElements
{
    private TMP_Text text;
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Updates the display of the score.
    /// </summary>
    public override void UpdateDisplay()
    {
        //Debug.Log($"All references used in {nameof(UpdateDisplay)}:\t" +
        //    "\nGameManager:\t" + GameManager.instance +
        //    "\nScore:\t" + GameManager.instance.score +
        //    "\nHigh Score:\t" + GameManager.instance.highScore);
        text.text = "Score: " + GameManager.instance.score;
        
    }
}
