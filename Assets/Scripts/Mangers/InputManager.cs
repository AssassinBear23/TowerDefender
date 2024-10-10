using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;


    void Awake()
    {
        Initialization();
    }

    void Initialization()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // The movementVector that's used for movement by the playable character
    [HideInInspector] public Vector3 verticalMovementVector;
    [HideInInspector] public Vector3 horizontalMovementVector;
    public void OnMovementKey(InputAction.CallbackContext context)
    {
        Debug.Log("Movement Key Pressed"
            + $"\nKey is:\t{context.control.displayName}");
        var readValue = context.ReadValue<Vector3>();
        verticalMovementVector = new Vector3(0, 0, readValue.z);
        horizontalMovementVector = new Vector3(readValue.x, 0, 0);
    }
}
