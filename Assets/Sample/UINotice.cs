using UnityEngine;
using System.Collections;
using TinyTeam.UI;
using UnityEngine.UI;

public class UINotice : TTUIPage
{

    public UINotice() : base(UIWindowType.PopUp, UIWindowShowMode.DoNothing, UIWindowColliderMode.Normal)
    {
        uiPath = "UIPrefab/Notice";
    }

    public override void Awake()
    {
        this.gameObject.transform.Find("content/btn_confim").GetComponent<Button>().onClick.AddListener(() =>
        {
            Hide();
        });
    }


}
