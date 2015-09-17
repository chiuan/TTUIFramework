
namespace TinyTeam.UI
{
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.UI;

    /// <summary>
    /// Each Page Mean one UI 'window'
    /// 
    /// </summary>

    public abstract class TTUIPageBase
    {
        public string windowName = string.Empty;

        //this page's id
        public int windowID = -1;

        //this page's type
        public UIWindowType windowType = UIWindowType.Normal;

        //how to show this page.
        public UIWindowShowMode showMode = UIWindowShowMode.DoNothing;

        //the background collider mode
        public UIWindowColliderMode colliderMode = UIWindowColliderMode.None;

        //this ui gameobject
        public GameObject gameObject;

        public static Dictionary<int, TTUIPageBase> allPages;

        public virtual void Show() { }

        public virtual void Hide() { }

        public virtual void Refresh() { }

        private TTUIPageBase() { }

        public TTUIPageBase(UIWindowType type, UIWindowShowMode mod,UIWindowColliderMode col)
        {
            windowType = type;
            showMode = mod;
            colliderMode = col;
        }

        internal bool CheckIfNeedBack()
        {
            if (windowType == UIWindowType.Fixed || windowType == UIWindowType.PopUp) return false;
            else if (showMode == UIWindowShowMode.NoNeedBack) return false;
            return true;
        }

        /// <summary>
        /// push this page's ui gameobject to anchor
        /// </summary>
        internal void PushUIGameObject(GameObject ui)
        {
            if (TTUIRoot.Instance == null || ui == null) return;

            this.gameObject = ui;

            Vector3 anchorPos = ui.GetComponent<RectTransform>().anchoredPosition;
            Vector2 sizeDel = ui.GetComponent<RectTransform>().sizeDelta;

            if (windowType == UIWindowType.Fixed)
            {
                ui.transform.parent = TTUIRoot.Instance.fixedRoot;
            }
            else if(windowType == UIWindowType.Normal)
            {
                ui.transform.parent = TTUIRoot.Instance.normalRoot;
            }
            else if(windowType == UIWindowType.PopUp)
            {
                ui.transform.parent = TTUIRoot.Instance.popupRoot;
            }

            ui.GetComponent<RectTransform>().anchoredPosition = anchorPos;
            ui.GetComponent<RectTransform>().sizeDelta = sizeDel;
        }

        internal void ShowUIGameObject()
        {
            if(this.gameObject == null)
            {
                Debug.LogError("wanna show ui:" + windowName + " is not exist ui gameobject!");
            }

            //check this kind of ui should cached
            if (CheckIfNeedBack())
            {

            }

        }

    }
}