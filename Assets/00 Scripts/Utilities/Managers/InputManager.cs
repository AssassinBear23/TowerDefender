using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    // A global reference (Singleton) of the InputManager that other scripts can reference to
    private static InputManager _instance;
    public static InputManager Instance { get => _instance; }

    // Whether or not the player game controls are enabled
    [SerializeField] private bool playerGameControlsEnabled = true;
    /// <summary>  
    /// Gets or sets a value indicating whether the tower game controls are enabled.  
    /// </summary>  
    /// <value>  
    ///   <c>true</c> if player game controls are enabled; otherwise, <c>false</c>.  
    /// </value>  
    public bool PlayerGameControlsEnabled { get => playerGameControlsEnabled; set => playerGameControlsEnabled = value; }

    // Whether or not the tower controls are enabled
    [SerializeField] private bool playerControlsEnabled = true;
    /// <summary>
    /// Gets or sets a value indicating whether the overall tower controls are enabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if player controls are enabled; otherwise, <c>false</c>.
    /// </value>
    public bool PlayerControlsEnabled { get => playerControlsEnabled; set => playerControlsEnabled = value; }

    void Awake()
    {
        Initialization();
    }

    /// <summary>
    /// Initializes the InputManager singleton.
    /// </summary>
    void Initialization()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // ============================================= CONTROL HANDLING ===================================================

    #region Player Controls
    [Header("Player Movement Input")]
    // The movementVector that's used for movement by the playable character
    public Vector2 playerMovementInput;

    /// <summary>
    /// Handles the movement key input.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnMovementKey(InputAction.CallbackContext context)
    {
        if (!playerGameControlsEnabled) return;
        //Debug.Log("Movement Key Pressed"
        //    + $"\nKey is:\t{context.control.displayName}");

        // Reads the value of the context (which is a normalized vector of the pressed keys)
        playerMovementInput = context.ReadValue<Vector2>();
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
        if (!playerControlsEnabled) return;
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
         * The paremeter is a System.Action, this means that you can pass  a lambda function.
         * System.Action is a delegate for that lambda function.
         * 
         * This allows the user to pass a lambda fucntion that reset the desired boolean 
         * Example: StartCoroutine(ResetKey(() => pausePressed = false));
         */
        yield return new WaitForEndOfFrame();
        resetAction();
    }

    #endregion Player Controls
}
