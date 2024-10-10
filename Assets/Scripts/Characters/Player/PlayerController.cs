using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    [Header("Initialization Values")]
    [Tooltip("Is the character a player?")]
    [SerializeField] private bool isPlayer;
    [Tooltip("The Input Manager of the game")]
    [SerializeField] private InputManager inputManager;
    [Tooltip("The Rigidbody of the character")]
    [SerializeField] private CharacterController cc;

    [Header("Movement Values")]
    [Tooltip("The scale to multiply the movement vector of the character with")]
    [SerializeField] float movementScale = 10f;
    [Tooltip("The default speed the player is going to have in the z axis")]
    [SerializeField] float levelMovementSpeed = 5f;
    [Tooltip("The scale that the default speed rises with, this is a linear value")]
    [SerializeField] float levelMovementScale = 0.1f;

    private Vector3 movementVector = new();

    private void Start()
    {
        GetReferences();
    }

    /// <summary>
    /// Get the necessary references for the character controller.
    /// </summary>
    void GetReferences()
    {
        if (inputManager == null)
        {
            inputManager = InputManager.instance;
        }
        if (cc == null)
        {
            cc = GetComponent<CharacterController>();
        }
        //cc.attachedRigidbody.useGravity = true;
    }

    // Update is called once per frame  
    void FixedUpdate()
    {
        MoveCharacter();
    }

    void MoveCharacter()
    {
        DefaultMovement();
        if (isPlayer)
        {
            MovePlayer();
        }
    }

    /// <summary>
    /// Move the player based on the input from the player.
    /// </summary>
    public void MovePlayer()
    {
        // Player made movements
        float rawHorizontalMovementValue = inputManager.horizontalMovementVector;
        float rawVerticalMovementValue = inputManager.verticalMovementVector;

        float horizontalMovementVector = movementScale * rawHorizontalMovementValue;
        float verticalMovementVector = movementScale * rawVerticalMovementValue;

        movementVector = new Vector3(horizontalMovementVector, 0, verticalMovementVector);
    }

    /// <summary>
    /// Perform the default movement of the character.
    /// </summary>
    void DefaultMovement()
    {
        // Up the movement speed slowly
        levelMovementSpeed += levelMovementScale * Time.deltaTime;
        // Update the vector
        movementVector.z += levelMovementSpeed;
        //movementVector.y = Time.deltaTime * Physics.gravity.y;
        // Apply the Vector to the position
        cc.SimpleMove(movementVector);
        //cc.Move(movementVector);
        Debug.Log("Vector Default Movement:\t" + movementVector);
    }
}
