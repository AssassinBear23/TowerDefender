using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : Obstacle
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed of the obstacle")]
    public float movementSpeed = 5f;
    [Tooltip("The frequency that the obstacle will move at.\nHow long it waits before it starts moving again after getting to its point."
             + "\nIn seconds.")]
    [SerializeField] private float holdTime = 1f;
    [Space(10)]
    [Tooltip("The starting direction of the obstacle."
             + "\nIs dependant on correct naming of the limits.")]
    [SerializeField] private Direction startingDirection;
    [Tooltip("The transforms of the two edge colliders.\nSet by default to the ones that are part of the prefab")]
    [SerializeField] private List<Transform> limitTransforms = new();

    private enum Direction { Left, Right }

    private Transform leftLimit;
    private Transform rightLimit;

    // ========================================== METHOD DECLERATIONS =======================================================

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Start()
    {
        ValidateList();
        SetLimitsAndStartingDirection();
        StartCoroutine(WaitAndMove(rightLimit));
    }

    /// <summary>
    /// Validates the limitTransforms list to ensure it contains exactly two valid transforms.
    /// </summary>
    void ValidateList()
    {
        if (limitTransforms.Count != 2)
        {
            Debug.LogError("The limit transforms are not set correctly in the inspector."
                           + "\nCurrently there are "
                           + limitTransforms.Count
                           + " transforms in the list."
                           + "\nThis needs to be 2.");
            gameObject.SetActive(false);
        }
        if (limitTransforms[0] == null || limitTransforms[1] == null)
        {
            Debug.LogError("One of the limit transforms is not set correctly in the list."
                           + $"\nPlease check the limitTransform list.");
            gameObject.SetActive(false);
        }
        if (limitTransforms[0] == limitTransforms[1])
        {
            Debug.LogError("The 2 limit transforms are the same."
                           + $"\nPlease check the limitTransform list.");
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the left and right limits and the starting direction of the obstacle.
    /// </summary>
    void SetLimitsAndStartingDirection()
    {
        foreach (Transform limit in limitTransforms)
        {
            if (limit.name.ToLower() == "leftedge")
            {
                leftLimit = limit;
            }
            else if (limit.name.ToLower() == "rightedge")
            {
                rightLimit = limit;
            }
            else
            {
                Debug.LogError("The limit transforms are not named correctly."
                               + $"\nPlease name the left limit 'LeftEdge' and the right limit 'RightEdge'.");
                gameObject.SetActive(false);
            }

            // Check if the limit name contains the starting direction
            if (limit.name.ToLower().Contains(startingDirection.ToString().ToLower()))
            {
                StartCoroutine(WaitAndMove(limit));
            }
        }
    }

    /// <summary>
    /// Called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("COLLIDED WITH: " + other.transform.name.ToUpper());
        if (other.transform == leftLimit)
        {
            Debug.Log("Moving to right limit");
            StartCoroutine(WaitAndMove(rightLimit));
        }
        else if (other.transform == rightLimit)
        {
            Debug.Log("Moving to left limit");
            StartCoroutine(WaitAndMove(leftLimit));
        }
    }

    /// <summary>
    /// Coroutine to wait for a specified time and then move the obstacle towards the target.
    /// </summary>
    /// <param name="target">The target transform to move towards.</param>
    /// <returns>IEnumerator for the coroutine.</returns>
    private IEnumerator WaitAndMove(Transform target)
    {
        // Wait for the specified hold time before starting to move
        yield return new WaitForSeconds(holdTime);

        // Continue moving towards the target until the obstacle is close enough (within half its width)
        while (Vector3.Distance(transform.position, target.position) > gameObject.transform.localScale.x / 2)
        {
            // Move the obstacle towards the target position at the specified movement speed
            transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);

            // Wait for the next frame before continuing the loop
            yield return null;
        }
    }
}
