using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public Camera Camera { set => _camera = value; }

    public bool _debug = false;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.transform.forward);
    }
}
