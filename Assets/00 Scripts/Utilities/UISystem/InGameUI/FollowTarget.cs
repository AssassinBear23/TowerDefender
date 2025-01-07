using UnityEditor;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private Vector3 Offset;

    public bool _debug = false;

    private void Update()
    {
        Follow();
    }

    private void Follow()
    {
        try
        {
            transform.position = Target.position + Offset;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error following target: " + e.Message);
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(this);
#endif
        }
    }
}
