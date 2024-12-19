using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<AbstractEnemy> _toSpawnWave;
    public void SetEnemiesToSpawn(List<AbstractEnemy> enemies) => _toSpawnWave = enemies;
    [SerializeField] private Transform _parentTransform;

    [SerializeField] private readonly float _timeBetweenEnemies = 1;

    public IEnumerator StartWave()
    {
        foreach (var enemy in _toSpawnWave)
        {
            Instantiate(original: enemy, position: transform.position, rotation: Quaternion.identity, parent: _parentTransform);
            yield return new WaitForSeconds(_timeBetweenEnemies);
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