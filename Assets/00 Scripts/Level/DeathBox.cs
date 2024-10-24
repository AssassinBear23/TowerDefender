using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.PlayerDeath?.Invoke();
        }
    }
}
