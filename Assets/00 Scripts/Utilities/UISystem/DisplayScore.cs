using TMPro;
using UnityEngine;

public class DisplayScore : UIElement
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
        text.text = "Score: " + GameManager.instance.score;
    }
}
