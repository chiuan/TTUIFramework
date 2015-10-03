namespace TinyTeam.UI
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Init The UI Root
    /// 
    /// UIRoot
    /// -Canvas
    /// --FixedRoot
    /// --NormalRoot
    /// --PopupRoot
    /// -Camera
    /// </summary>
    public class TTUIRoot : MonoBehaviour
    {
        private static TTUIRoot m_Instance = null;
        public static TTUIRoot Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    InitRoot();
                }
                return m_Instance;
            }
        }

        public Transform root;
        public Transform fixedRoot;
        public Transform normalRoot;
        public Transform popupRoot;
        public Camera uiCamera;

        static void InitRoot()
        {
            GameObject go = new GameObject("UIRoot");
            go.layer = LayerMask.NameToLayer("UI");
            m_Instance = go.AddComponent<TTUIRoot>();
            go.AddComponent<RectTransform>();
            m_Instance.root = go.transform;

            Canvas can = go.AddComponent<Canvas>();
            can.renderMode = RenderMode.ScreenSpaceCamera;
            can.pixelPerfect = true;
            GameObject camObj = new GameObject("UICamera");
            camObj.layer = LayerMask.NameToLayer("UI");
            camObj.transform.parent = go.transform;
            camObj.transform.localPosition = new Vector3(0,0,-100f);
            Camera cam = camObj.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.Depth;
            cam.orthographic = true;
            cam.farClipPlane = 200f;
            can.worldCamera = cam;
            m_Instance.uiCamera = cam;
            cam.cullingMask = 1<<5;
            cam.nearClipPlane = -50f;
            cam.farClipPlane = 50f;

            //add audio listener
            camObj.AddComponent<AudioListener>();
            camObj.AddComponent<GUILayer>();

            CanvasScaler cs = go.AddComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1136f, 640f);
            cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            ////add auto scale camera fix size.
            //TTCameraScaler tcs = go.AddComponent<TTCameraScaler>();
            //tcs.scaler = cs;

            //set the raycaster
            //GraphicRaycaster gr = go.AddComponent<GraphicRaycaster>();

            GameObject subRoot = CreateSubCanvasForRoot(go.transform,250);
            subRoot.name = "FixedRoot";
            m_Instance.fixedRoot = subRoot.transform;

            subRoot = CreateSubCanvasForRoot(go.transform,0);
            subRoot.name = "NormalRoot";
            m_Instance.normalRoot = subRoot.transform;

            subRoot = CreateSubCanvasForRoot(go.transform,500);
            subRoot.name = "PopupRoot";
            m_Instance.popupRoot = subRoot.transform;

            //add Event System
            GameObject esObj = GameObject.Find("EventSystem");
            if(esObj != null)
            {
                GameObject.DestroyImmediate(esObj);
            }

            GameObject eventObj = new GameObject("EventSystem");
            eventObj.layer = LayerMask.NameToLayer("UI");
            eventObj.transform.SetParent(go.transform);
            eventObj.AddComponent<EventSystem>();
            if (!Application.isMobilePlatform || Application.isEditor)
            {
                eventObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
            else
            {
                eventObj.AddComponent<UnityEngine.EventSystems.TouchInputModule>();
            }

            
        }

        static GameObject CreateSubCanvasForRoot(Transform root,int sort)
        {
            GameObject go = new GameObject("canvas");
            go.transform.parent = root;
            go.layer = LayerMask.NameToLayer("UI");

            Canvas can = go.AddComponent<Canvas>();
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;

            can.overrideSorting = true;
            can.sortingOrder = sort;

            go.AddComponent<GraphicRaycaster>();

            return go;
        }

        void OnDestroy()
        {
            m_Instance = null;
        }
    }
}