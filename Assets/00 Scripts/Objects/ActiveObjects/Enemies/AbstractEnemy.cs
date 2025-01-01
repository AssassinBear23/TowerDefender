using UnityEngine;

abstract public class AbstractEnemy : MonoBehaviour
{
    
    [Header("AI Settings")]
    [Tooltip("The range outside of its own mesh that it can attack in")]
    [SerializeField] private float _attackRange = 1f;
    [Tooltip("The speed at which the enemy attacks")]
    [SerializeField] private float attackSpeedValue;


    abstract public void Move();
    abstract public void Attack();

    /// <summary>
    /// Checks if there are enemies in range using a sphere cast.
    /// </summary>
    /// <returns>True if there are enemies in range, otherwise false.</returns>
    private bool IsWithinRange(out HealthController towerHC)
    {
        Transform towerPos = GameManager.Instance.Tower;
        towerHC = null;
        float rangeSquared = _attackRange * _attackRange;

        if ((transform.position - towerPos.position).sqrMagnitude <= rangeSquared)
        {
            towerHC = towerPos.GetComponent<HealthController>();
            return true;
        }
        return false;
    }

    
}
