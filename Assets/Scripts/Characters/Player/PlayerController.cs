using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Initialization Values")]
    [Tooltip("The GameManager instance in the scene")]
    [SerializeField] private GameManager gameManager;
    [Tooltip("The Input Manager instance in the scene")]
    [SerializeField] private InputManager inputManager;
    [Tooltip("The Rigidbody of the character")]
    [SerializeField] private CharacterController cc;

    [Header("Movement Values")]
    [Tooltip("The scale to multiply the movement vector of the character with")]
    [SerializeField] float movementScale = 10f;

    private Vector3 movementVector = new();

    public static System.Action PlayerDeath;


    private void Start()
    {
        GetReferences();
        GameManager.instance.player = gameObject;
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
        if (gameManager == null)
        {
            gameManager = GameManager.instance;
        }
        if (cc == null)
        {
            cc = GetComponent<CharacterController>();
        }
    }

    // Update is called once per frame  
    void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Move the player based on the input from the player.
    /// </summary>
    public void MovePlayer()
    {
        movementVector = new Vector3(inputManager.playerInput * movementScale, 0, 0);
        cc.SimpleMove(movementVector);
    }
}
