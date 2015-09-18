using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TinyTeam.UI;

public class GameMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TTUIPageBase.ShowPage<UITopBar>();
        new UIMainPage().Show();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

}
