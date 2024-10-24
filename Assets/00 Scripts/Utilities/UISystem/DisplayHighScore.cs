using TMPro;
using UnityEngine;

public class DisplayHighScore : UIElement
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Updates the display with the high score.
    /// </summary>
    public override void UpdateDisplay()
    {
        text.text = $"High Score: {GameManager.instance.highScore}";
    }
}
