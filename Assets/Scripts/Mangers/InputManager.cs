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

    /// <summary>
    /// Initializes the InputManager singleton.
    /// </summary>
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
    [HideInInspector] public float verticalMovementVector;
    [HideInInspector] public float horizontalMovementVector;

    /// <summary>
    /// Handles the movement key input.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnMovementKey(InputAction.CallbackContext context)
    {
        Debug.Log("Movement Key Pressed"
            + $"\nKey is:\t{context.control.displayName}");

        // Reads the value of the context (which is a normalized vector of the pressed keys)
        var readValue = context.ReadValue<Vector3>();

        // Assigns the read value to the movement vectors
        verticalMovementVector = readValue.z;
        horizontalMovementVector = readValue.x;

        // Lowers the value if the player is trying to move backwards
        if (verticalMovementVector <= 0) verticalMovementVector /= 2;
    }
}
