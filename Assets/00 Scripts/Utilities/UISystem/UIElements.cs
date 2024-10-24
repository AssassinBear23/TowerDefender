using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElements : MonoBehaviour
{
    /// <summary>
    /// Updates the UI element.
    /// </summary>
    public void UpdateElement()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Default method that gets overridden by the child classes. That's also the reason that it is a virtual method.
    /// </summary>
    public virtual void UpdateDisplay()
    {

    }
}
