namespace TinyTeam.UI
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Each Page Mean one UI 'window'
    /// 3 steps:
    /// instance ui > refresh ui by data > show
    /// 
    /// by chiuan
    /// 2015-09
    /// </summary>

    #region define

    public enum UIWindowType
    {
        Normal,    // 可推出界面(UIMainMenu,UIRank等)
        Fixed,     // 固定窗口(UITopBar等)
        PopUp,     // 模式窗口
    }

    public enum UIWindowShowMode
    {
        DoNothing,
        HideOther,     // 闭其他界面
        NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
        NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
    }

    public enum UIWindowColliderMode
    {
        None,      // 显示该界面不包含碰撞背景
        Normal,    // 碰撞透明背景
        WithBg,    // 碰撞非透明背景
    }
    #endregion

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
        private static Dictionary<string, TTUIPage> m_allPages;
        public static Dictionary<string, TTUIPage> allPages
        {
            get
            {
                return m_allPages;
            }
        }

        //control 1>2>3>4>5 each page close will back show the previus page.
        private static List<TTUIPage> m_currentPageNodes;
        public static List<TTUIPage> currentPageNodes
        {
            get
            {
                return m_currentPageNodes;
            }
        }

        #region virtual api

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
            //1:instance ui
            InstanceUI();

            //protected.
            if (this.gameObject == null) return;

            //2:refresh ui component.
            Refresh();

            //3:animation active.
            Active();
        }

        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public virtual void Awake() { }

        public virtual void Refresh() { }

        public virtual void Active()
        {
            this.gameObject.SetActive(true);
        }

        #endregion

        #region internal api

        private TTUIPage() { }

        public TTUIPage(UIWindowType type, UIWindowShowMode mod,UIWindowColliderMode col)
        {
            windowType = type;
            showMode = mod;
            colliderMode = col;
            windowName = this.GetType().ToString();

            //Debug.LogWarning("[UI] create page:" + ToString());
        }

        internal bool CheckIfNeedBack()
        {
            if (windowType == UIWindowType.Fixed || windowType == UIWindowType.PopUp) return false;
            else if (showMode == UIWindowShowMode.NoNeedBack) return false;
            return true;
        }

        internal void AnchorUIGameObject(GameObject ui)
        {
            if (TTUIRoot.Instance == null || ui == null) return;

            this.gameObject = ui;
            this.transform = ui.transform;

            Vector3 anchorPos = ui.GetComponent<RectTransform>().anchoredPosition;
            Vector2 sizeDel = ui.GetComponent<RectTransform>().sizeDelta;
            Vector3 scale = ui.GetComponent<RectTransform>().localScale;

            //Debug.Log("anchorPos:" + anchorPos + "|sizeDel:" + sizeDel);

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
            ui.GetComponent<RectTransform>().localScale = scale;
        }

        public override string ToString()
        {
            return ">Name:" + windowName + ",ID:" + windowID + ",Type:" + windowType.ToString() + ",ShowMode:" + showMode.ToString() + ",Collider:" + colliderMode.ToString();
        }

        public bool isActive()
        {
            return gameObject != null && gameObject.activeSelf;
        }

        #endregion

        #region static api

        private static bool CheckIfNeedBack(TTUIPage page)
        {
            return page != null && page.CheckIfNeedBack();
        }

        private static void PopNode(TTUIPage page)
        {
            if (m_currentPageNodes == null)
            {
                m_currentPageNodes = new List<TTUIPage>();
            }

            if(page == null)
            {
                Debug.LogError("[UI] page popup is null.");
                return;
            }

            //sub pages should not need back.
            if(CheckIfNeedBack(page) == false)
            {
                return;
            }

            for(int i=0; i < m_currentPageNodes.Count; i++)
            {
                if (m_currentPageNodes[i].Equals(page))
                {
                    m_currentPageNodes.RemoveAt(i);
                    m_currentPageNodes.Add(page);
                    return;
                }
            }

            m_currentPageNodes.Add(page);

            //after pop should hide the old node if need.
            HideOldNodes();
        }

        private static void HideOldNodes()
        {
            if (m_currentPageNodes.Count < 0) return;
            TTUIPage topPage = m_currentPageNodes[m_currentPageNodes.Count-1];
            if(topPage.showMode == UIWindowShowMode.HideOther)
            {
                //form bottm to top.
                for(int i=m_currentPageNodes.Count -2; i >= 0; i--)
                {
                    m_currentPageNodes[i].Hide();
                }
            }
        }

        public static void ShowPage<T>() where T :TTUIPage,new()
        {
            Type t = typeof(T);
            string pageName = t.ToString();

            if (m_allPages != null && m_allPages.ContainsKey(pageName))
            {
                ShowPage(pageName, m_allPages[pageName]);
            }
            else
            {
                T instance = new T();
                ShowPage(pageName, instance);
            }
        }

        public static void ShowPage(string pageName,TTUIPage pageInstance)
        {
            if(string.IsNullOrEmpty(pageName) || pageInstance == null)
            {
                Debug.LogError("[UI] show page error with :" + pageName + " maybe null instance.");
                return;
            }

            if (m_allPages == null)
            {
                m_allPages = new Dictionary<string, TTUIPage>();
            }

            TTUIPage page = null;
            if (m_allPages.ContainsKey(pageName))
            {
                //if sub page is active before.
                if (m_allPages[pageName].isActive() == false)
                {
                    m_allPages[pageName].Show();
                }
                page = m_allPages[pageName];
            }
            else
            {
                m_allPages.Add(pageName, pageInstance);
                pageInstance.Show();
                page = pageInstance;
            }

            PopNode(page);
        }

        /// <summary>
        /// close current page in the "top" node.
        /// </summary>
        public static void ClosePage()
        {
            Debug.Log("Back&Close PageNodes Count:" + m_currentPageNodes.Count);

            if (m_currentPageNodes == null || m_currentPageNodes.Count <= 1) return;

            TTUIPage closePage = m_currentPageNodes[m_currentPageNodes.Count - 1];
            m_currentPageNodes.RemoveAt(m_currentPageNodes.Count - 1);
            closePage.Hide();

            //show older page.
            //TODO:Sub pages.belong to root node.
            if(m_currentPageNodes.Count > 0)
            {
                TTUIPage page = m_currentPageNodes[m_currentPageNodes.Count - 1];
                ShowPage(page.windowName, page);
            }
        }

        #endregion
    }
}