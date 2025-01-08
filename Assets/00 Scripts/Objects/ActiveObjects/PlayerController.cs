using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputManager _inputManager;
    private CharacterController _cc;

    [Tooltip("The speed at which the player moves.")]
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private Camera _camera;

    private void Start()
    {
        _cc = GetComponent<CharacterController>();
        _inputManager = InputManager.Instance;
    }

    private void FixedUpdate()
    {
        if (_inputManager.playerMovementInput != Vector2.zero)
        {
            Move();
        }
    }

    /// <summary>
    /// Moves the player based on the input from the player.
    /// </summary>
    private void Move()
    {
        // I want a check to see if the player would move outside of the camera view, if so, set the velocity in that direction to 0
        Vector3 playerPosition = _camera.WorldToViewportPoint(transform.position);

        //Debug.Log("Player position in Camera: " + playerPosition);

        Vector2 moveDirection = _inputManager.playerMovementInput;

        moveDirection = RestrictMovementToViewport(playerPosition, moveDirection);

        moveDirection = moveDirection.Rotate(45f); // Rotate the movement direction by 45 degrees
        _cc.SimpleMove(new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed);

        Debug.DrawLine(transform.position, transform.position + new Vector3(moveDirection.x, 0, moveDirection.y));
    }

    private static Vector2 RestrictMovementToViewport(Vector3 playerPosition, Vector2 moveDirection)
    {
        switch (playerPosition.x)
        {
            case < 0:

                moveDirection.x = Mathf.Clamp01(moveDirection.x);
                break;
            case > 1:
                moveDirection.x = -Mathf.Clamp01(moveDirection.x);
                break;
        }
        switch (playerPosition.y)
        {
            case < 0:
                moveDirection.y = Mathf.Clamp01(moveDirection.y);
                break;
            case > 1:
                moveDirection.y = -Mathf.Clamp01(moveDirection.y);
                break;
        }

        return moveDirection;
    }
}
