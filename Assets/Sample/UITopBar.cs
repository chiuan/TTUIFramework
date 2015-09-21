using UnityEngine;
using System.Collections;
using TinyTeam.UI;
using UnityEngine.UI;

public class UITopBar : TTUIPage {

    public UITopBar() : base(UIWindowType.Fixed, UIWindowShowMode.DoNothing, UIWindowColliderMode.None)
    {
        uiPath = "UIPrefab/Topbar";
    }

    public override void Awake()
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(() =>
        {
            TTUIPage.ClosePage();
        });

        this.gameObject.transform.Find("btn_notice").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<UINotice>();
        });
    }


}
