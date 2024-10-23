using UnityEngine;

public class ShooterObstacle : Obstacle
{
    // ======================================================== VARIABLES =========================================================================

    [Header("Setup Variables")]
    [Tooltip("The bullet prefab that the shooter will shoot")]
    [SerializeField] private GameObject projectilePrefab;
    [Tooltip("The GameObject that will hold the bullets made by this Shooter")]
    [SerializeField] private Transform projectileHolder;
    [Tooltip("The position that the bullet will spawn at")]
    [SerializeField] private Transform projectileSpawnPosition;
    [Space(10)]
    [Tooltip("The rotation of the bullet")]
    [SerializeField] private Quaternion bulletRotation;

    [Header("Shooter Variables")]
    [Tooltip("The time between each shot in seconds")]
    [Range(0.1f, 10f)] public float shotCooldown = 1f;
    [Tooltip("The speed of the bullet")]
    public float bulletSpeed = 5f;

    /// <summary>
    /// The Time.time at which this script last shot a bullet.
    /// </summary>
    private float lastShotTime;

    // ======================================================= SETUP METHODS =======================================================================

    private void OnValidate()
    {
        if (projectileHolder == null)
        {
            Debug.LogWarning($"The {nameof(projectileHolder)} is not set! Please set it in the inspector.");
        }
        if (projectileSpawnPosition == null)
        {
            Debug.LogWarning($"The {nameof(projectileSpawnPosition)} is not set! Please set it in the inspector.");
        }
        if (projectilePrefab == null)
        {
            Debug.LogWarning($"There is no {nameof(projectilePrefab)} reference set! Please set it in the inspector");
        }
    }

    // ==================================================== FUNCTIONAL METHODS =====================================================================

    /// <summary>
    /// Update is called once per frame.
    /// Checks if the shooter is on cooldown and shoots if not.
    /// </summary>
    void Update()
    {
        if (!IsOnCooldown())
        {
            Shoot();
        }
    }

    /// <summary>
    /// Instantiates a projectile at the specified spawn position.
    /// Logs a warning if the projectile prefab is not set.
    /// </summary>
    void Shoot()
    {
        if (projectilePrefab != null)
        {
            var bullet = Instantiate(projectilePrefab,
                                     projectileSpawnPosition.position,
                                     new Quaternion(0, 0, 90, 0),
                                     projectileHolder != null ? projectileHolder : null);
            bullet.transform.rotation = bulletRotation;
            bullet.GetComponent<Bullet>().Speed = bulletSpeed;
            lastShotTime = Time.time;
        }
    }

    /// <summary>
    /// Checks if the shooter is on cooldown.
    /// </summary>
    /// <returns>True if the shooter is on cooldown, false otherwise.</returns>
    bool IsOnCooldown()
    {
        return Time.time - lastShotTime < shotCooldown;
    }
}
