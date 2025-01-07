using UnityEngine;

public class BounceMovement : MonoBehaviour
{
    [Header("Bounce Movement")]
    [Tooltip("The amount of force to apply to the object")]
    [SerializeField] private float _forceAmount = 10f;
    [Tooltip("The amount of bounces before the object stops bouncing")]
    [SerializeField] private float _bounceAmount = 1;
    [Tooltip("Minimum and maximum values for the x and z components of the direction vector")]
    [SerializeField] private float _minXZ = 0.4f;
    [SerializeField] private float _maxXZ = 1f;

    private Vector3 _direction;

    private Rigidbody _rb;
    private BoxCollider _collider;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _direction = GetRandomDirection();
        Bounce();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 300, 150, 100), $"Bounce {gameObject.name}"))
        {
            _direction = GetRandomDirection();
            _bounceCount = 0;
            _collider.isTrigger = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;  
            Bounce();
        }
    }

    void Bounce()
    {   
        _rb.AddForce(_direction * _forceAmount, ForceMode.Impulse);
    }

    /// <summary>
    /// Returns a random direction vector where the sum of x and z are always the same so the offset is done by a multiplier that is between <see cref="_minXZ"/> and <see cref="_maxXZ"/>.
    /// </summary>
    /// <returns></returns>
    Vector3 GetRandomDirection()
    {
        Vector3 _randomDirection;

        float _xValue = Random.Range(-1f, 1f);
        float _remainder = 1 - Mathf.Abs(_xValue);
        bool _posZValue = Random.Range(0, 2) == 0;
        float _zValue = _posZValue ? _remainder : -_remainder;

        _randomDirection = new Vector3(_xValue, 1, _zValue);

        float _multiplierXZ = Random.Range(_minXZ, _maxXZ);

        _randomDirection.x *= _multiplierXZ;
        _randomDirection.z *= _multiplierXZ;

        return _randomDirection;
    }

    int _bounceCount = 0;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);
        int _groundLayer = LayerMask.NameToLayer("Ground");

        if (collision.gameObject.layer != _groundLayer)
        {
            return;
        }

        Debug.Log(gameObject.name + " has touched the ground, count is now:\t" + _bounceCount);
        if (_bounceCount >= _bounceAmount)
        {
            Debug.Log("Bounce count is greater than or equal to bounce amount, stopping bounce");
            _rb.constraints = RigidbodyConstraints.FreezePosition;
            _collider.isTrigger = true;
        }
        else
        {
            _bounceCount++; 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _direction);
    }
}
