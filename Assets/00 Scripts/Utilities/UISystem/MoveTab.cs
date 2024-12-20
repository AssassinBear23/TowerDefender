using DG.Tweening;
using UnityEngine;

public class MoveTab : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("The amount of time for the transition")]
    [SerializeField] private float _time = 1f;
    [Tooltip("The X Position at retracted")]
    [SerializeField] private float _xPositionRetracted = -500f;
    [Tooltip("The X Position at extended")]
    [SerializeField] private float _xPositionExtended = 0f;
    [Tooltip("If the tab is already extended or not")]
    [SerializeField] private bool _isExtended = false;

    [SerializeField] private Camera _sceneCamera;

    public void OnButtonPress()
    {
        if (_isExtended)
        {
            transform.DOMoveX(_xPositionRetracted, _time);
            DOTween.To(() => _sceneCamera.rect, x => _sceneCamera.rect = x, new Rect(0, 0, 1, 1), _time);
        }
        else
        {
            transform.DOMoveX(_xPositionExtended, _time);
            DOTween.To(() => _sceneCamera.rect, x => _sceneCamera.rect = x, new Rect(0.25f, 0, 1, 1), _time);
        }
        _isExtended = !_isExtended;
    }
}
