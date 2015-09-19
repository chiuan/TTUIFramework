using UnityEngine;
using System.Collections;
using TinyTeam.UI;
using UnityEngine.UI;

public class UIMainPage : TTUIPage {

    public UIMainPage() : base(UIWindowType.Normal, UIWindowShowMode.HideOther, UIWindowColliderMode.None)
    {
        uiPath = "UIPrefab/UIMain";
    }

    public override void Awake()
    {
        this.transform.Find("btn_skill").GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("open skill");
            TTUIPage.ShowPage<UISkillPage>();
        });
    }


}
