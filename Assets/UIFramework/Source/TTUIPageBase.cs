namespace TinyTeam.UI
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.UI;

    /// <summary>
    /// Each Page Mean one UI 'window'
    /// 3 steps:
    /// instance ui > refresh ui by data > show
    /// 
    /// by chiuan
    /// 2015-09
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

        //path to load ui
        public string uiPath = string.Empty;

        //this ui's gameobject
        public GameObject gameObject;
        public Transform transform;

        //all pages with the union id
        public static Dictionary<System.Type, TTUIPageBase> allPages;

        //control 1>2>3>4>5 each page close will back show the previus page.
        public static Stack<TTUIPageBase> backPagesStack;

        /// <summary>
        /// 1:instance ui
        /// </summary>
        public virtual void InstanceUI()
        {
            if (this.gameObject == null)
            {
                GameObject go = GameObject.Instantiate(Resources.Load(uiPath)) as GameObject;
                AnchorUIGameObject(go);
                
                //after instance should awake init.
                Awake();
            }
        }

        public virtual void Show()
        {
            //THREE STEPS
            InstanceUI();

            if (this.gameObject == null) return;

            Refresh();
        }

        public virtual void Hide() { }

        public virtual void Awake() { }

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
        internal void AnchorUIGameObject(GameObject ui)
        {
            if (TTUIRoot.Instance == null || ui == null) return;

            this.gameObject = ui;
            this.transform = ui.transform;

            Vector3 anchorPos = ui.GetComponent<RectTransform>().anchoredPosition;
            Vector2 sizeDel = ui.GetComponent<RectTransform>().sizeDelta;

            if (windowType == UIWindowType.Fixed)
            {
                ui.transform.SetParent(TTUIRoot.Instance.fixedRoot);
            }
            else if(windowType == UIWindowType.Normal)
            {
                ui.transform.SetParent(TTUIRoot.Instance.normalRoot);
            }
            else if(windowType == UIWindowType.PopUp)
            {
                ui.transform.SetParent(TTUIRoot.Instance.popupRoot);
            }

            ui.GetComponent<RectTransform>().anchoredPosition = anchorPos;
            ui.GetComponent<RectTransform>().sizeDelta = sizeDel;
        }

        /// <summary>
        /// Show target page
        /// </summary>
        public static void ShowPage<T>() where T :TTUIPageBase
        {
            Type t = typeof(T);
            if (allPages.ContainsKey(t))
            {
                allPages[t].Show();
            }
            //allPages.Add(t,T.);
        }
    }
}