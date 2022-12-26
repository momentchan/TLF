using System.Collections;
using UnityEngine;

namespace TLF
{
    public class Container : MonoBehaviour
    {
        [SerializeField] private float duration = 5f;
        [SerializeField] private float moveDistance = -15f;

        public void SwitchContent()
        {
            StartCoroutine(StartMove());
        }

        IEnumerator StartMove()
        {
            yield return null;

            var lpos = transform.localPosition;
            var t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                transform.localPosition = lpos + Vector3.right * Mathf.Lerp(0, moveDistance, t / duration);
                yield return null;
            }
        }
    }
}