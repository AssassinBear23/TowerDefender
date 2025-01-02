using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Initialization Values")]
    [Tooltip("List of the spawners")]
    [SerializeField] private List<Spawner> spawners;
    private List<Wave> waveOrder;
    /// <summary>
    /// The order of the waves to spawn
    /// </summary>
    public List<Wave> WaveOrder { set => waveOrder = value; }
    private Wave _currentWave;
    private int _currentWaveIndex = 0;
    public int CurrentWaveIndex { get => _currentWaveIndex; }

    public Wave Wave { set => _currentWave = value; }

    public void Start()
    {
        waveOrder = GameManager.Instance.LevelData.Waves;
        GetNextWave();
    }

    public void StartWave()
    {
        if (_currentWave == null) return;
        List<AbstractEnemy> enemies = new(_currentWave.Enemies);
        foreach (var spawner in spawners)
        {
            spawner.SetEnemiesToSpawn(enemies);
            StartCoroutine(spawner.StartWave());
        }
        GetNextWave();
    }

    private void GetNextWave()
    {
        if (waveOrder.Count > 0)
        {
            _currentWave = waveOrder[_currentWaveIndex];
            _currentWaveIndex++;
        }
    }

    private void OnGUI()
    {
        // Create a button at the top-left corner of the screen
        if (GUI.Button(new Rect(10, 10, 150, 30), "Start Next Wave"))
        {
            StartWave();
        }
    }
}
