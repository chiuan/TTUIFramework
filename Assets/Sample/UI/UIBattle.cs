using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine.UI;

public class UIBattle : TTUIPage {

    public UIBattle() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIBattle";
    }

    public override void Awake(GameObject go)
    {
        this.transform.Find("btn_skill").GetComponent<Button>().onClick.AddListener(OnClickSkillGo);
        this.transform.Find("btn_battle").GetComponent<Button>().onClick.AddListener(OnClickGoBattle);
    }

    private void OnClickSkillGo()
    {
        //goto skill upgrade page.
        ShowPage<UISkillPage>();
    }

    private void OnClickGoBattle()
    {
        Debug.Log("should load your battle scene!");
    }
}
