using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    [Header("Rotation Values")]
    [Tooltip("The speed at which the object will rotate")]
    [SerializeField] private float _rotationSpeed = 10f;

    /// <summary>
    /// Gets or sets the rotation speed of the object.
    /// The rotation speed determines how fast the object will rotate.
    /// The value cannot be negative; if a negative value is set, it will default to 0.
    /// </summary>
    public float RotationSpeed
    {
        // Return the rotation speed
        get { return _rotationSpeed; }
        // Make sure the rotation speed is never negative
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            _rotationSpeed = value;
        }
    }

    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.World);
    }
}
