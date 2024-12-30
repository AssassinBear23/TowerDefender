using UnityEngine;

public class TowerController : MonoBehaviour
{
    [Header("Initialization Values")]
    [Tooltip("The GameManager instance in the scene")]
    private GameManager _gm;
    [Tooltip("The spot from which the projectiles will launch from")]
    [SerializeField] private Transform _firePoint;

    // Methods for the initialization values
    public Transform FirePoint { get => _firePoint; }

    private void Awake()
    {
        if (_firePoint == null)
        {
            Debug.Log("Fire point is not set.");
        }
        _gm = GameManager.Instance;
        _gm.Tower = transform;
    }
}
