using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // A global reference (Singleton) of the InputManager that other scripts can reference to
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

    [Header("Pause Input")]
    public bool pausePressed;

    /// <summary>
    /// Method used to read the input for pausing the game.
    /// For more information, see <a href="https://docs.unity3d.com/Packages/com.unity.inputsystem@1.11/manual/index.html">New Input System Documentation</a>.
    /// </summary>
    /// <param name="context">The context regarding the call.</param>
    public void ReadPauseInput(InputAction.CallbackContext context)
    {
        pausePressed = !context.canceled;
        StartCoroutine(ResetPausePressed());
    }

    /// <summary>
    /// Coroutine that resets the pausePressed variable at the end of the frame
    /// </summary>
    /// <returns><see cref="IEnumerator"/>, allows this function to be used as a coroutine</returns>
    IEnumerator ResetPausePressed()
    {
        yield return new WaitForEndOfFrame();
        pausePressed = false;
    }
}
