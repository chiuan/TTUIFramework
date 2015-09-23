using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISkillItem : MonoBehaviour {

    public UDSkill.Skill data = null;

    public void Refresh(UDSkill.Skill skill)
    {
        this.data = skill;
        this.transform.Find("title").GetComponent<Text>().text = skill.name + "[lv." + skill.level + "]";
    }
}
