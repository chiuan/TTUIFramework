namespace TinyTeam.UI
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

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

        static void InitRoot()
        {
            GameObject go = new GameObject("UIRoot");
            m_Instance = go.AddComponent<TTUIRoot>();
            go.AddComponent<RectTransform>();
            m_Instance.root = go.transform;

            Canvas can = go.AddComponent<Canvas>();
            can.renderMode = RenderMode.ScreenSpaceCamera;
            can.pixelPerfect = true;
            GameObject camObj = new GameObject("Camera");
            camObj.transform.parent = go.transform;
            camObj.transform.localPosition = new Vector3(0,0,-100f);
            Camera cam = camObj.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.Depth;
            cam.orthographic = true;
            cam.farClipPlane = 200f;
            can.worldCamera = cam;

            CanvasScaler cs = go.AddComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1136f, 640f);
            cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

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

        }

        static GameObject CreateSubCanvasForRoot(Transform root,int sort)
        {
            GameObject go = new GameObject("canvas");
            go.transform.parent = root;

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
    }
}