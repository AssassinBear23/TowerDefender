using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Initialization Values")]
    [Tooltip("List of the spawners")]
    [SerializeField] private List<Spawner> _spawners;

    [Header("Boss prefabs")]
    [SerializeField] private GameObject _bossPrefab;

    private List<Wave> waveOrder;
    /// <summary>
    /// The order of the waves to spawn
    /// </summary>
    public List<Wave> WaveOrder { set => waveOrder = value; }
    private Wave _currentWave;
    private int _currentWaveIndex = 0;
    public int CurrentWaveIndex { get => _currentWaveIndex; }

    [SerializeField] private TMP_Text _waveText;

    public void Start()
    {
        waveOrder = GameManager.Instance.LevelData.Waves;
        GetNextWave();
        GameManager.Instance.WaveManager = this;
    }

    public void TryStartWave(GameObject _callElement)
    {
        Debug.Log("Trying to start wave");
        if (_currentWave == null) return;

        Debug.Log("Disabling Button");
        StartCoroutine(DisableButton(_callElement));

        Debug.Log("Starting wave");
        StartWave();
    }

    private IEnumerator DisableButton(GameObject _callElement)
    {
        UnityEngine.UI.Button _elementButton = _callElement.GetComponent<UnityEngine.UI.Button>();
        _elementButton.interactable = false;
        yield return new WaitForEndOfFrame();
        while (GameManager.Instance.EnemiesAlive != 0) yield return null;
        _elementButton.interactable = true;
    }

    private void StartWave()
    {
        _waveText.text = $"Wave {_currentWaveIndex} / {waveOrder.Count}";

        if (_currentWave.IsBossWave)
        {
            Debug.Log("Spawning Boss Wave");
            SpawnBossWave();
        }
        else
        {
            Debug.Log("Spawning Normal Wave");
            SpawnNormalWave();
        }

        GetNextWave();
        foreach (GameObject enemy in _currentWave.Enemies)
        {
            Debug.Log($"Enemy: {enemy.name}");
        }
    }

    private void SpawnBossWave()
    {
        int _bossSpawnerIndex = Random.Range(0, _spawners.Count - 1);
        for (int i = 0; i < _spawners.Count; i++)
        {
            if (i != _bossSpawnerIndex)
            {
                _spawners[i].ToSpawnWave = _currentWave.Enemies;
            }
            else
            {
                Wave _bossWave = _currentWave;
                _bossWave.Enemies.RemoveAt(_bossWave.Enemies.Count - 1);
                _bossWave.Enemies.Add(_bossPrefab);
                _spawners[i].ToSpawnWave = _bossWave.Enemies;
            }
            Debug.Log($"Starting StartWave method in {_spawners[i].name}");
            StartCoroutine(_spawners[i].StartWave());
        }
    }

    private void SpawnNormalWave()
    {
        foreach (Spawner spawner in _spawners)
        {
            Debug.Log($"Starting StartWave method in {spawner.name}");
            spawner.ToSpawnWave = _currentWave.Enemies;
            StartCoroutine(spawner.StartWave());
        }
    }

    private void GetNextWave()
    {
        if (waveOrder.Count - 1 > _currentWaveIndex)
        {
            _currentWave = waveOrder[_currentWaveIndex];
            _currentWaveIndex++;
        }
        else
        {
            Debug.Log("At last wave already");
        }
    }

    //private void OnGUI()
    //{
    //    // Create a button at the top-left corner of the screen
    //    if (GUI.Button(new Rect(10, 10, 150, 30), "Start Next Wave"))
    //    {
    //        StartWave();
    //    }
    //}

    public enum WaveType
    {
        Normal,
        MiniBoss,
        Boss
    }
}
