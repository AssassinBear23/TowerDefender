using UnityEngine;

public class GroundScroll : MonoBehaviour
{
    private GameManager gameManager;
    private GroundManager groundManager;

    [Header("Deletion values")]
    [Tooltip("The distance from the player that the platform will be deleted")]
    [SerializeField] private float deleteDistance = 10f;
    [SerializeField] private Transform platformTransform;

    private void Start()
    {
        gameManager = GameManager.instance;
        groundManager = GroundManager.instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveGround();
    }

    private void Update()
    {
        DestroyAfterPass();
    }

    void DestroyAfterPass()
    {
        if (platformTransform.position.z < gameManager.player.transform.position.z - deleteDistance)
        {
            groundManager.BroadcastMessage("PlatformDestroy", gameObject);
        }
    }

    void MoveGround()
    {
        transform.position -= new Vector3(0, 0, (gameManager.levelMovementSpeed * Time.deltaTime));
        return;
    }
}
