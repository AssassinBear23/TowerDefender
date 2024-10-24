using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StraightBullet : Bullet
{
    void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        Debug.Log("MOVING" +
            "\nSpeed: " + Speed +
            "\nDirection: " + direction +
            "\nTotal: " + direction * -Speed);
        transform.Translate(-Speed * Time.deltaTime * direction);
    }
}
