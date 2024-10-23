using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillManager", menuName = "Skills/SkillManager")]
public class SkillManager : ScriptableObject
{
    public List<Skill> enabledSkills;

}
