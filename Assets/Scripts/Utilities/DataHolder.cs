using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;

[FilePath("/Data/ScoreData.txt", FilePathAttribute.Location.ProjectFolder)]
public class DataHolder : ScriptableSingleton<DataHolder>
{
    public Dictionary<string, float> highScores;

    public void AddScore(string playerName, float score)
    {
        highScores.Add(playerName, score);
        highScores = highScores.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        Save(true);
        Debug.Log("Saved to:\t" + GetFilePath());
    }
}

#endif