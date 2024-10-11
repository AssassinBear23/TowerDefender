using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public static InputManager instance;

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
            DontDestroyOnLoad(gameObject); // Make this instance persistent
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // The movementVector that's used for movement by the playable character
    [HideInInspector] public float playerInput;

    /// <summary>
    /// Handles the movement key input.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnMovementKey(InputAction.CallbackContext context)
    {
        Debug.Log("Movement Key Pressed"
            + $"\nKey is:\t{context.control.displayName}");

        // Reads the value of the context (which is a normalized vector of the pressed keys)
        playerInput = context.ReadValue<float>();
    }
}
