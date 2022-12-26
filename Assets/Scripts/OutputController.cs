using UnityEngine;

namespace TLF
{
    public class OutputController : MonoBehaviour
    {
        [SerializeField] private OutputMode mode = OutputMode.Realtime;
        [SerializeField] private Output outputL, outputR;

        public void StartRealtime()
        {
            Debug.Log("StartRealtime");
            outputL.StartRealtime();
            outputR.StartRealtime();
        }

        public void StartPrerender()
        {
            Debug.Log("StartPrerender");
            outputL.StartPrerender();
            outputR.StartPrerender();
        }


        [System.Serializable]
        public class Output
        {
            public Material mat;
            public RenderTexture realtimeTex;
            public Texture2D prerenderTex;
            public void StartRealtime()
            {
                mat.SetTexture("_MainTex", realtimeTex);
            }
            public void StartPrerender()
            {
                mat.SetTexture("_MainTex", prerenderTex);
            }
        }
    }
    public enum OutputMode
    {
        Prerender, Realtime
    }
}