using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [Header("Initialization Values")]
    [Tooltip("Is the character a player?")]
    [SerializeField] private bool isPlayer;
    [Tooltip("The Input Manager of the game")]
    [SerializeField] private InputManager inputManager;
    [Tooltip("The Rigidbody of the character")]
    [SerializeField] private Rigidbody rb;

    [Header("Movement Values")]
    [Tooltip("The scale to multiply the movement vector of the character with")]
    [SerializeField] float movementScale = 10f;
    [Tooltip("The default speed the player is going to have in the z axis")]
    [SerializeField] float levelMovementSpeed = 5f;
    [Tooltip("The scale that the default speed rises with, this is a linear value")]
    [SerializeField] float levelMovementScale = 0.1f;

    private Vector3 defaultForce = new();

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
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
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

    public void MovePlayer()
    {
        // Player made movements
        Vector3 rawHorizontalMovementVector = inputManager.horizontalMovementVector;
        Vector3 rawVerticalMovementVector = inputManager.verticalMovementVector;

        Vector3 horizontalMovementVector = movementScale * Time.deltaTime * rawHorizontalMovementVector;

        Debug.Log("Vector horizontal Movement:\t" + horizontalMovementVector);

        // Move the player
        transform.SetPositionAndRotation(transform.position + horizontalMovementVector, transform.rotation);

        defaultForce += movementScale * Time.deltaTime * rawVerticalMovementVector;
    }

    /// <summary>
    /// Perform the default movement of the character.
    /// </summary>
    void DefaultMovement()
    {
        if (!grounded)
        {
            FallMovement();
        }
        // Up the movement speed slowly
        levelMovementSpeed += levelMovementScale * Time.deltaTime;
        // Update the vector
        defaultForce.z = movementScale * Time.deltaTime * levelMovementSpeed;
        // Apply the Vector to the position
        rb.AddRelativeForce(defaultForce);
        Debug.Log("Vector Default Movement:\t" + defaultForce);
        
    }

    void FallMovement()
    {
        float gravity = 9.8f;
        defaultForce.y -= gravity * Time.deltaTime;
    }

    #region Grounded Check
    private bool grounded = true;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            defaultForce.y = 0f;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exit Collision");
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
    #endregion
}
