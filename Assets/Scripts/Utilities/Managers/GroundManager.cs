using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    #region Variables

    [Header("Platform Management")]
    [Tooltip("A list of platforms in the scene\nShould have the starter platforms in here, in order of close to far.")]
    [SerializeField] private List<GameObject> platforms = new();
    [Tooltip("A list of easy difficulty platform prefabs")]
    [SerializeField] private List<GameObject> easyPlatformPrefabs;
    [Tooltip("A list of medium difficulty platform prefabs")]
    [SerializeField] private List<GameObject> mediumPlatformPrefabs;
    [Tooltip("A list of hard difficulty platform prefabs")]
    [SerializeField] private List<GameObject> hardPlatformPrefabs;


    // Used for debugging purposes
    [HideInInspector] public GameObject LastSpawnedPlatform { get; private set; }
    [HideInInspector] public string LastSpawnedPlatformDifficulty { get; private set; }

    #endregion Variables

    //===================================== METHOD DECLERATION ====================================================== 
    #region Methods


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
        else if (GameManager.instance.groundManager != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    /// <summary>
    /// Method to subscribe to events.
    /// </summary>
    void SubscribeToEvents()
    {
        GameManager.OnDifficultyIncrease += CalculateDifficultyRatio;
    }

    /// <summary>
    /// Method to unsubscribe to events.
    /// </summary>
    void UnsubscribeToEvents()
    {
        GameManager.OnDifficultyIncrease -= CalculateDifficultyRatio;
    }

    /// <summary>
    /// Default Unity Event that is called when the script is going to be destroyed.
    /// </summary>
    private void OnDestroy()
    {
        UnsubscribeToEvents();
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
    /// It does this by positioning the new platform at the last platforms pivot point.
    /// </summary>
    /// <returns>The newly created platform.</returns>
    GameObject CreateNewPlatform()
    {
        var platform = platforms[^1];
        Debug.Log("Last platform: " + platform.name);
        var platformScrollScript = platform.GetComponent<GroundScroll>();
        Vector3 toSetPosition = platform.transform.position;

        // Get the platform prefab we are going to use.
        (GameObject platformPrefab, int value) = GetRandomPlatformPrefab();

        // get the offset that needs to be applied to the new platforms position before instantiation.
        float lastPlatformOffset = platformScrollScript.offset;
        float newPlatformOffset = platformPrefab.GetComponent<GroundScroll>().offset;

        // shift the z position of the new platform by the offset.
        toSetPosition.z += lastPlatformOffset + newPlatformOffset;

        if (value > 50)
        {
            Vector3 localScale = platformPrefab.transform.localScale;
            localScale.x *= -1;
            platformPrefab.transform.localScale = localScale;
        }

        Debug.Log("Raw toSetPosition: " + platform.transform.position +
            "\ntoSetPosition offset:\n\t" + lastPlatformOffset + "\n\t" + newPlatformOffset +
            "\nCreating new platform at: " + toSetPosition +
            "\nLast Platform is at:\t" + platform.transform.position +
            "\nPosition difference: " + (toSetPosition.z - platform.transform.position.z));

        var obj = Instantiate(platformPrefab, toSetPosition, new Quaternion(), parent: this.transform);

        amountOfSpawnedPlatforms++;

        LastSpawnedPlatform = obj;
        // instantiate the random platform prefab we got at the position we calculated and then return it.
        return obj;
    }



    public float EasyRatio { get; private set; }
    public float MediumRatio { get; private set; }



/// <summary>
/// Returns a random platform prefab based on the difficulty ratios.
/// </summary>
/// <returns>The randomly selected platform prefab.</returns>
(GameObject platformPrefab, int randomValue) GetRandomPlatformPrefab()
    {
        int t_num = Random.Range(0, 100);
        if (t_num <= EasyRatio)
        {
            LastSpawnedPlatformDifficulty = "Easy";
            return (easyPlatformPrefabs[Random.Range(0, easyPlatformPrefabs.Count)], t_num);
        }
        else if (t_num <= EasyRatio + MediumRatio)
        {
            LastSpawnedPlatformDifficulty = "Medium";
            return (mediumPlatformPrefabs[Random.Range(0, mediumPlatformPrefabs.Count)], t_num);
        }
        else
        {
            LastSpawnedPlatformDifficulty = "Hard";
            return (hardPlatformPrefabs[Random.Range(0, hardPlatformPrefabs.Count)], t_num);
        }
    }

    [SerializeField] private AnimationCurve easyPlatformsSpawnRateCurve;
    [SerializeField] private AnimationCurve mediumPlatformsSpawnRateCurve;
    /// <summary>
    /// Calculates the difficulty ratio based on the distance traveled.
    /// </summary>
    public void CalculateDifficultyRatio()
    {
        // Get the value to evaluate the curves with
        float x_value = GameManager.instance.DifficultyLevel / 10;

        // Evaluate Easy Diff Curv
        float evaluatedEasyCurve = EvaluateCurve(easyPlatformsSpawnRateCurve, x_value);
        Debug.Log(evaluatedEasyCurve);
        EasyRatio = evaluatedEasyCurve * 100;
        
        // Evaluate Medium Diff Curv
        float evaluatedMediumCurve = EvaluateCurve(mediumPlatformsSpawnRateCurve, x_value);
        Debug.Log(evaluatedMediumCurve);
        MediumRatio = evaluatedMediumCurve * 100;
    }

    /// <summary>
    /// Evaluates the given AnimationCurve at the specified value.
    /// </summary>
    /// <param name="curve">The AnimationCurve to evaluate.</param>
    /// <param name="value">The value at which to evaluate the curve.</param>
    /// <returns>The evaluated value of the curve at the specified value.</returns>
    float EvaluateCurve(AnimationCurve curve, float value)
    {
        return curve.Evaluate(value);
    }
}
