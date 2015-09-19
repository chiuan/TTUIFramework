using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TinyTeam.UI;

public class GameMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TTUIPage.ShowPage<UITopBar>();
        TTUIPage.ShowPage<UIMainPage>();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

}
