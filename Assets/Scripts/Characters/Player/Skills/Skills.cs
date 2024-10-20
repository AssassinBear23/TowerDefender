using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public virtual GameObject SkillPrefab { get; private set; }
    public List<Skills> enabledSkills = new();

    public virtual void SpawnSkillObject()
    {
        Instantiate(SkillPrefab, transform.position, Quaternion.identity);
    }
}
