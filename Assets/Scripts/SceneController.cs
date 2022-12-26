using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLF
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private GameObject container1;
        [SerializeField] private GameObject container2;
        [SerializeField] private AnimationCurve curve;

        [SerializeField] private float duration = 5f;
        [SerializeField] private float offset = 15f;

        public void MoveBoxes()
        {
            StartCoroutine(StartMove());
        }

        IEnumerator StartMove()
        {
            yield return null;

            var t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                var dist = -Mathf.Lerp(0, offset, curve.Evaluate(t / duration));
                container1.transform.localPosition = Vector3.right * dist;
                container2.transform.localPosition = Vector3.right * (offset + dist);
                yield return null;
            }
            container1.transform.localPosition = Vector3.right * offset;

            var temp = container1;
            container1 = container2;
            container2 = temp;
        }
    }
}
