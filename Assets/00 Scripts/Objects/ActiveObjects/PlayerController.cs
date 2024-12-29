using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputManager _inputManager;
    private CharacterController _cc;

    [Tooltip("The speed at which the player moves.")]
    [SerializeField] private float moveSpeed = 5f;

    private void Start()
    {
        _cc = GetComponent<CharacterController>();
        _inputManager = InputManager.Instance;
    }

    private void FixedUpdate()
    {
        if(_inputManager.playerMovementInput != Vector2.zero)
        {
            Move();
        }
    }

    /// <summary>
    /// Moves the player based on the input from the player.
    /// </summary>
    private void Move()
    {
        Vector2 moveDirection = _inputManager.playerMovementInput;
        moveDirection = moveDirection.Rotate(45f); // Rotate the movement direction by 45 degrees
        _cc.SimpleMove(new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed);
    }
}
