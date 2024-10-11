using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public static GroundManager instance;

    [Header("Platform Management")]
    [Tooltip("A list of platforms in the scene\nShould have the starter platforms in here, in order of close to far.")]
    [SerializeField] private List<GameObject> platforms = new();
    [Tooltip("A list of prefabs that can be used when creating new platforms")]
    [SerializeField] private List<GameObject> platformPrefabs;

    // Start is called before the first frame update
    void Awake()
    {
        SetupReferences();
    }

    /// <summary>
    /// Set up the references for the GroundManager instance.
    /// </summary>
    void SetupReferences()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
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
        Debug.Log("Last platform: " + platforms[^1].name);
        Vector3 toSetPosition = platforms[^1].transform.position;
        Debug.Log("Position before adding size: " + toSetPosition);
        var sizeOfPlatform = 15f;
        toSetPosition.z += sizeOfPlatform;
        Debug.Log("Creating new platform at: " + toSetPosition);
        // Instantiate a random platform prefab from the list and set its parent to the Ground empty object, and then return it
        return Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Count)], toSetPosition, new Quaternion(), parent: this.transform);
    }
}
