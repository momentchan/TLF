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

        private Vector3 pivotLeft;
        private Vector3 pivotRight;
        void Update()
        {
            coverLeft.transform.position = transform.position - scale.y * Vector3.right * 2;
            coverRight.transform.position = transform.position + scale.y * Vector3.right * 2;

            coverLeft.transform.localScale = scale;
            coverRight.transform.localScale = scale;
            coverLeft.transform.localRotation = Quaternion.Euler(ratio * 90, 90, 0);
            coverRight.transform.localRotation = Quaternion.Euler(ratio * 90, -90, 0);
        }
    }
}