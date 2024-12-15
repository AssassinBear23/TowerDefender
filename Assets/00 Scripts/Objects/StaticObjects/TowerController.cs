using UnityEngine;

public class TowerController: MonoBehaviour
{
    [Header("Initialization Values")]
    [Tooltip("The GameManager instance in the scene")]
    private GameManager gameManager;
    [Tooltip("The Input Manager instance in the scene")]
    private InputManager inputManager;
    [Tooltip("The spot from which the projectiles will launch from")]
    [SerializeField] private Transform _firePoint;

    // Methods for the initialization values
    public Transform FirePoint { get => _firePoint;}


    [Header("Basic Tower Stats")]
    [Tooltip("The current health of the tower")]
    [SerializeField] private float _health = 100f;
    [Tooltip("The maximum health of the tower")]
    [SerializeField] private float _maxHealth = 100f;
    [Tooltip("The fire rate of the tower")]
    [SerializeField] private float _fireRate = 0.5f;

    // Methods for the basic tower stats
    public float Health { get => _health; set => _health = value; }
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float FireRate { get => _fireRate; set => _fireRate = value; }

    [Header("Advanced Tower Stats")]
    [Tooltip("The critical chance rate of the tower")]
    [SerializeField] private float _criticalChance = 0.1f;
    [Tooltip("The critical damage multiplier of the tower")]
    [SerializeField] private float _criticalDamageProcent = 150f;
    [Tooltip("The armour of the tower")]
    [SerializeField] private float _armour = 0f;
    [Tooltip("The armour penetration of the tower")]
    [SerializeField] private float _armourPenetration = 0f;

    // Methods for the advanced tower stats
    public float CriticalChance { get => _criticalChance; set => _criticalChance = value; }
    public float CriticalDamageProcent { get => _criticalDamageProcent; set => _criticalDamageProcent = value; }
    public float Armour { get => _armour; set => _armour = value; }
    public float ArmourPenetration { get => _armourPenetration; set => _armourPenetration = value; }

    public static System.Action PlayerDeath;


    private void Start()
    {
        GetReferences();
        GameManager.instance.Tower = gameObject;
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
    }

    private void Update()
    {
        
    }
}
