using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]

    public class CoverController : MonoBehaviour
    {
        [SerializeField] private Transform coverLeft;
        [SerializeField] private Transform coverRight;
        [SerializeField, Range(-1, 1)] private float ratio;
        [SerializeField] private Vector3 scale;
        [SerializeField] private float closeT = 2f;
        [SerializeField] private AnimationCurve closeCurve;

        private Vector3 pivotLeft;
        private Vector3 pivotRight;

        public void ShowCover()
        {
            ratio = -1;
            coverLeft.gameObject.SetActive(true);
            coverRight.gameObject.SetActive(true);
        }

        public void CloseCover()
        {
            StartCoroutine(Close());
        }

        public void HideCover()
        {
            coverLeft.gameObject.SetActive(false);
            coverRight.gameObject.SetActive(false);
        }

        private void Start()
        {
            HideCover();
        }

        IEnumerator Close()
        {
            yield return null;
            var t = 0f;
            while (t < closeT)
            {
                t += Time.deltaTime;
                ratio = Mathf.Lerp(-1, 1, closeCurve.Evaluate(t / closeT));
                yield return null;
            }
        }

        void Update()
        {
            coverLeft.position = transform.position - scale.y * Vector3.right * 2;
            coverRight.position = transform.position + scale.y * Vector3.right * 2;

            coverLeft.localScale = scale;
            coverRight.localScale = scale;
            coverLeft.localRotation = Quaternion.Euler(ratio * 90, 90, 0);
            coverRight.localRotation = Quaternion.Euler(ratio * 90, -90, 0);
        }
    }
}