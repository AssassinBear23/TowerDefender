using Unity.VisualScripting;
using UnityEngine;

public class GroundScroll : MonoBehaviour
{
    [Header("Deletion values")]
    [Tooltip("The distance from the player that the platform will be deleted")]
    [SerializeField] private float deleteDistance = 10f;
    [Tooltip("The midpoint of the platform")]
    [SerializeField] private Transform midPointTransform;
    [Tooltip("The start point of the platform")]
    [Min(0)] public float offset;


    void Start()
    {
        MoveGround();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DestroyAfterPass();
        MoveGround();   
    }

    void DestroyAfterPass()
    {
        if (midPointTransform.position.z < GameManager.instance.player.transform.position.z - deleteDistance)
        {
            //Debug.Log("All info for line 22:" +
            //    "\nGameManager instance:\t" + GameManager.instance +
            //    "\ngroundManager reference in instance:\t" + GameManager.instance.groundManager +
            //    "\ngameObject reference passed in Broadcast method:\t" + gameObject);
            GameManager.instance.groundManager.PlatformDestroy(gameObject);
        }
    }

    void MoveGround()
    {
        transform.position -= new Vector3(0, 0, (GameManager.instance.levelMovementSpeed * Time.deltaTime));
        return;
    }
}
