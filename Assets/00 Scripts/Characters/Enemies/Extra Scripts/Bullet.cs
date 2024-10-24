using UnityEngine;


public abstract class Bullet : MonoBehaviour
{
    public float Speed { get; set; }

    public Vector3 direction;

    public virtual void DetermineDirection(Quaternion rotation)
    {
        Debug.Log("Determining direction");
        direction = rotation.eulerAngles;
        Debug.Log("Raw Direction: " + direction
                  + "\nNormalized Direction: " + direction.normalized);
        direction =  direction.normalized;
    }

    public abstract void Move();
}
