using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<GameObject> _toSpawnWave;
    public List<GameObject> ToSpawnWave { set => _toSpawnWave = value; }
    [SerializeField] private Transform _parentTransform;

    [SerializeField] private float _baseTimeBetweenEnemies = 2f;
    private readonly List<float> _timeBetweenEnemies = new();

    [Header("Health Bar Settings")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Camera _camera;

    private float _distanceToTowerSquared;

    private void Start()
    {
        Vector3 _towerPositionn = GameManager.Instance.Tower.transform.position;
        _distanceToTowerSquared = new Vector2(transform.position.x - _towerPositionn.x
                                             ,transform.position.z - _towerPositionn.z)
                                             .sqrMagnitude;
    }

    public void GetSpawnTimes()
    {
        for (int i = 1; i < _toSpawnWave.Count; i++)
        {
            Vector3 bounds = _toSpawnWave[i].GetComponent<AbstractEnemy>().GetBounds();
            float _toAddTime = .1f * Mathf.Pow(bounds.x,1.9f) + _baseTimeBetweenEnemies;
            _timeBetweenEnemies.Add(_toAddTime);
        }
        Debug.Log("Spawn times have been calculated" + _timeBetweenEnemies);
    }

    /// <summary>
    /// Starts the wave of enemies by instantiating each enemy in the list with a delay between each spawn.
    /// </summary>
    /// <returns>IEnumerator for coroutine handling.</returns>
    public IEnumerator StartWave()
    {
        Debug.Log("Wave has started");
        GetSpawnTimes();
        for (int i = 0; i < _toSpawnWave.Count; i++)
        {
            GameObject _instantiated = Instantiate(original: _toSpawnWave[i], position: transform.position, rotation: Quaternion.identity, parent: _parentTransform);
            AbstractEnemy _aeComponent = _instantiated.GetComponent<AbstractEnemy>();
            _aeComponent.SetupHealthBar(_canvas, _camera);
            _aeComponent.TotalDistance = _distanceToTowerSquared;
            Debug.Log("Waint for " + _timeBetweenEnemies[i] + " seconds");
            if (i != _toSpawnWave.Count - 1) yield return new WaitForSeconds(_timeBetweenEnemies[i]);
        }
        Debug.Log("Wave has ended");
    }

    private void OnDrawGizmos()
    {
        Vector3 _pos = transform.position;
        Vector3 _size = Vector3.one * 2;
        _pos.y += _size.y/2;
        Gizmos.color = new Color(1,0,0,0.5f);
        Gizmos.DrawCube(_pos, _size);
    }
}