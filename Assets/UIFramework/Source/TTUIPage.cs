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

    public abstract class TTUIPage
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

        //all pages with the union type
        public static Dictionary<string, TTUIPage> allPages;

        //control 1>2>3>4>5 each page close will back show the previus page.
        public static Stack<TTUIPage> backPagesStack;

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

        private TTUIPage() { }

        public TTUIPage(UIWindowType type, UIWindowShowMode mod,UIWindowColliderMode col)
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
        public static void ShowPage<T>() where T :TTUIPage,new()
        {
            Type t = typeof(T);
            string pageName = t.ToString();

            if (allPages == null)
            {
                allPages = new Dictionary<string, TTUIPage>();
            }
            
            if (allPages.ContainsKey(pageName))
            {
                allPages[pageName].Show();
                return;
            }

            T instance = new T();
            allPages.Add(pageName, instance);
            instance.Show();
        }

        public static void ShowPage(string pageName,TTUIPage pageInstance)
        {
            if(string.IsNullOrEmpty(pageName) || pageInstance == null)
            {
                Debug.LogError("[UI] show page error with :" + pageName + " maybe null instance.");
                return;
            }

            if (allPages == null)
            {
                allPages = new Dictionary<string, TTUIPage>();
            }

            if (allPages.ContainsKey(pageName))
            {
                allPages[pageName].Show();
                return;
            }

            allPages.Add(pageName, pageInstance);
            pageInstance.Show();

        }
    }
}