using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData {
    private static GameData m_instance;
    public static GameData Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = new GameData();
            }
            return m_instance;
        }
    }

    private GameData() {
        //NOTE : this is Test Init Here.
        playerSkill = new UDSkill();
        playerSkill.skills = new List<UDSkill.Skill>();
        for(int i=0; i < 10; i++)
        {
            UDSkill.Skill skill = new UDSkill.Skill();
            skill.name = "skill_" + i;
            skill.level = 1;
            skill.desc = "这是个牛逼的技能";
            playerSkill.skills.Add(skill);
        }
    }

    public UDSkill playerSkill;

}
