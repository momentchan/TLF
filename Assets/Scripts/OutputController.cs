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
    }
    public enum OutputMode
    {
        Prerender, Realtime
    }
}