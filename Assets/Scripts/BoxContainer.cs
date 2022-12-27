using System.Collections;
using UnityEngine;

namespace TLF
{
    public class BoxContainer : MonoBehaviour
    {
        [SerializeField] private MeshRenderer outerBox;
        [SerializeField] private float scaleUpTime = 2f;
        [SerializeField] private float scaleUp = 1.05f;
        public GameObject content;

        public void CopyConentTo(BoxContainer c2)
        {
            content.transform.parent = c2.transform;
            content.transform.localPosition = Vector3.zero;

            c2.content = content;
            content = null;
        }

        public void ScaleUp()
        {
            StartCoroutine(StartScaleUp());
        }

        IEnumerator StartScaleUp()
        {
            yield return null;
            var t = 0f;
            while (t < scaleUpTime)
            {
                t += Time.deltaTime;
                outerBox.transform.localScale = Vector3.one * Mathf.Lerp(1, scaleUp, t / scaleUpTime);
                yield return null;
            }
            outerBox.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }

        public void Reset()
        {
            outerBox.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            outerBox.transform.localScale = Vector3.one;
        }
    }
}