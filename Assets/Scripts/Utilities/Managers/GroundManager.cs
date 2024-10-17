using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [Header("Platform Management")]
    [Tooltip("A list of platforms in the scene\nShould have the starter platforms in here, in order of close to far.")]
    [SerializeField] private List<GameObject> platforms = new();
    [Tooltip("A list of easy difficulty platform prefabs")]
    [SerializeField] private List<GameObject> easyPlatformPrefabs;
    [Tooltip("A list of medium difficulty platform prefabs")]
    [SerializeField] private List<GameObject> mediumPlatformPrefabs;
    [Tooltip("A list of hard difficulty platform prefabs")]
    [SerializeField] private List<GameObject> hardPlatformPrefabs;
    
    // Start is called before the first frame update
    void Awake()
    {
        SetupReferences();
        SubscribeToEvents();
    }

    /// <summary>
    /// Set up the references for the GroundManager instance.
    /// </summary>
    void SetupReferences()
    {
        if (GameManager.instance.groundManager == null)
        {
            GameManager.instance.groundManager = this;
        }
        else if(GameManager.instance.groundManager != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void SubscribeToEvents()
    {
        GameManager.OnDifficultyIncrease += CalculateDifficultyRatio;
    }

    void UnsubscribeToEvents()
    {
        GameManager.OnDifficultyIncrease -= CalculateDifficultyRatio;
    }

    /// <summary>
    /// Remove the platform from the list, create a new platform and add it to the list, and destroy the platform object.
    /// </summary>
    /// <param name="platformPrefab">The platform prefab to be destroyed.</param>
    public void PlatformDestroy(GameObject platformPrefab)
    {
        // Remove the platform from the list
        platforms.Remove(platformPrefab);

        // Create a new platform and add it to the list
        platforms.Add(CreateNewPlatform());

        // Destroy the platform object
        Destroy(platformPrefab);
    }


    /// <summary>
    /// Create a new platform and add it to the list, then return it.
    /// </summary>
    /// <returns>The newly created platform.</returns>
    GameObject CreateNewPlatform()
    {
        //Debug.Log("Last platform: " + platforms[^1].name);
        Vector3 toSetPosition = platforms[^1].transform.position;
        //Debug.Log("Position before adding size: " + toSetPosition);
        var sizeOfPlatform = 15f;
        toSetPosition.z += sizeOfPlatform;
        //Debug.Log("Creating new platform at: " + toSetPosition);
        // Instantiate a random platform prefab from the list and set its parent to the Ground empty object, and then return it
        var platformPrefab = GetRandomPlatformPrefab();
        return Instantiate(platformPrefab, toSetPosition, new Quaternion(), parent: this.transform);
    }



    private float easyRatio = 80f;
    private float mediumRatio = 18f;
    GameObject GetRandomPlatformPrefab()
    {
        int t_num = Random.Range(0, 100);
        if (t_num <= easyRatio)
        {
            return easyPlatformPrefabs[Random.Range(0, easyPlatformPrefabs.Count)];
        }
        else if (t_num <= easyRatio + mediumRatio)
        {
            return mediumPlatformPrefabs[Random.Range(0, mediumPlatformPrefabs.Count)];
        }
        else
        {
            return hardPlatformPrefabs[Random.Range(0, hardPlatformPrefabs.Count)];
        }
    }

    [SerializeField] private AnimationCurve easyPlatformsSpawnRateCurve;
    [SerializeField] private AnimationCurve mediumPlatformsSpawnRateCurve;
    private 
    void CalculateDifficultyRatio()
    {
        float distanceInKm = GameManager.instance.distanceTraveled / 1000;
        easyRatio = EvaluateCurve(easyPlatformsSpawnRateCurve, distanceInKm) * 100;
        mediumRatio = EvaluateCurve(mediumPlatformsSpawnRateCurve, distanceInKm) * 100;
    }

    float EvaluateCurve(AnimationCurve curve, float value)
    {
        return curve.Evaluate(value);
    }
}
