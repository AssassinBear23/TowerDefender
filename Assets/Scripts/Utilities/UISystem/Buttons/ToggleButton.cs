using System;
using TMPro;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    public virtual void ToggleState(GameObject var)
    {
        if(var == null)
        {
            throw new NullReferenceException();
        }
        Debug.Log($"Changing {var.name}'s state");
        var.SetActive(!var.activeSelf);
        Debug.Log($"State: {var.activeSelf}");
    }

    public virtual void ToggleState(TMP_Text var)
    {
        var.gameObject.SetActive(!var.gameObject.activeSelf);
    }

    public virtual void ToggleState(Transform var)
    {
        var.gameObject.SetActive(!var.gameObject.activeSelf);
    }
}
