using UnityEngine;
using System.Collections;
using TinyTeam.UI;

public class UITopBar : TTUIPageBase {

    public UITopBar() : base(UIWindowType.Fixed, UIWindowShowMode.DoNothing, UIWindowColliderMode.None)
    {

    }

    public override void Show()
    {
        GameObject go = GameObject.Instantiate(Resources.Load("UIPrefab/Topbar")) as GameObject;
        PushUIGameObject(go);
    }


}
