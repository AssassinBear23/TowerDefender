using UnityEngine;

public class ShieldSkill : Skills
{
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float shieldDuration = 3f;



    private void Awake()
    {
        Instantiate(shieldPrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        
    }
}
