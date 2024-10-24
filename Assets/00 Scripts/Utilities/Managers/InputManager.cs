nah
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
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
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }


    #region Player Controls
    [Header("Player Movement Input")]
    // The movementVector that's used for movement by the playable character
    public float playerMovementInput;

    /// <summary>
    /// Handles the movement key input.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnMovementKey(InputAction.CallbackContext context)
    {
        //Debug.Log("Movement Key Pressed"
        //    + $"\nKey is:\t{context.control.displayName}");

        // Reads the value of the context (which is a normalized vector of the pressed keys)
        playerMovementInput = context.ReadValue<float>();
    }

    [Header("Skill inputs")]
    public bool shieldPressed;
    /// <summary>
    /// Handles the shield key input.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnShieldKey(InputAction.CallbackContext context)
    {
        //Debug.Log("OnShieldKey called"
        //          + $"\nState is: {context.phase}");
        shieldPressed = !context.canceled;
        //StartCoroutine(ResetKey(() => shieldPressed = false));
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
        StartCoroutine(ResetKey(() => pausePressed = false));
    }

    /// <summary>
    /// Coroutine that resets the specified boolean variable at the end of the frame
    /// </summary>
    /// <param name="resetAction">The action to reset the boolean variable.</param>
    /// <returns><see cref="IEnumerator"/>, allows this function to be used as a coroutine</returns>
    public static IEnumerator ResetKey(System.Action resetAction)
    {
        /* How does this work:
         * The return type is a IEnumerator, which allows this function to be used as a coroutine.
         * 
         * The paremeter is a System.Action, this means that the parameter is a delegate that can be called. 
         * In this case that would be a lambda function.
         * 
         * This allows the user to pass a lambda fucntion that reset the desired boolean 
         * Example: StartCoroutine(ResetKey(() => pausePressed = false));
         */
        yield return new WaitForEndOfFrame();
        resetAction();
    }

    #endregion Player Controls
}
