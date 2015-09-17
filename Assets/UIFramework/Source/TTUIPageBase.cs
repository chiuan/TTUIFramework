
namespace TinyTeam.UI
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    /// <summary>
    /// Each Page Mean one UI 'window'
    /// 
    /// </summary>

    public abstract class TTUIPageBase
    {
        //this window's type
        public UIWindowType windowType = UIWindowType.Normal;

        //how to show this window.
        public UIWindowShowMode showMode = UIWindowShowMode.DoNothing;

        //the background collider mode
        public UIWindowColliderMode colliderMode = UIWindowColliderMode.None;

        //this ui gameobject
        public GameObject gameObject;

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

    }
}