using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{
    public float Speed { get; set; }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        transform.Translate(Speed * Time.deltaTime * Vector3.down);
    }
}
