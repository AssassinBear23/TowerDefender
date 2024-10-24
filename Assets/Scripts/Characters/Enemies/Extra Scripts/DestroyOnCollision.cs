using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Destroy(gameObject);
    }
}
