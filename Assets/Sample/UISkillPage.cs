using UnityEngine;
using System.Collections;
using TinyTeam.UI;
using UnityEngine.UI;

public class UISkillPage : TTUIPage {

    public UISkillPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UISkill";
    }

    public override void Awake()
    {

    }


}
