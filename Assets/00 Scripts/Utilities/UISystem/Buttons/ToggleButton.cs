using System;
using TMPro;
using UnityEngine;
using static GameDebug;

public class ToggleButton : Button
{
    /// <summary>
    /// Toggles the active state of the specified GameObject.
    /// </summary>
    /// <param name="var">The GameObject to toggle.</param>
    /// <exception cref="NullReferenceException">Thrown when the specified GameObject is null.</exception>
    public virtual void ToggleState(GameObject var)
    {
        if (var == null)
        {
            throw new NullReferenceException();
        }
        Debug.Log($"Changing {var.name}'s state");
        var.SetActive(!var.activeSelf);
        Debug.Log($"State: {var.activeSelf}");
    }

    /// <summary>
    /// Toggles the active state of the specified TMP_Text's GameObject.
    /// </summary>
    /// <param name="var">The TMP_Text component whose GameObject to toggle.</param>
    public virtual void ToggleState(TMP_Text var)
    {
        var.gameObject.SetActive(!var.gameObject.activeSelf);
    }

    /// <summary>
    /// Toggles the active state of the specified Transform's GameObject.
    /// </summary>
    /// <param name="var">The Transform component whose GameObject to toggle.</param>
    public virtual void ToggleState(Transform var)
    {
        var.gameObject.SetActive(!var.gameObject.activeSelf);
    }

    public DebugFlagsEnum flagToToggle;

    /// <summary>
    /// Toggles the specified debug flag.
    /// </summary>
    public void ToggleDebugFlag()
    {
        if (GameDebug.IsDebugFlagEnabled(flagToToggle))
        {
            GameDebug.DisableDebugFlag(flagToToggle);
            GameManager.Instance.UIManager.UpdateElements();
        }
        else
        {
            GameDebug.EnableDebugFlag(flagToToggle);
            GameManager.Instance.UIManager.UpdateElements();
        }
    }
}
