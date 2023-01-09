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