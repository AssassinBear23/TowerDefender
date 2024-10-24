using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public List<Skill> enabledSkills;

    private void Start()
    {
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddSkill(Skill skill)
    {
        enabledSkills.Add(skill);
    }

    public void RemoveSkill(Skill skill)
    {
        enabledSkills.Remove(skill);
    }

    public void UseSkills()
    {
        foreach (var skill in enabledSkills)
        {
            skill.UseSkill();
        }
    }
}
