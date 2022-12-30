using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]
    public class CameraArrangeHelper : MonoBehaviour
    {
        [SerializeField] private Transform cam1;
        [SerializeField] private Transform cam2;
        [SerializeField] private Vector3 pivot;
        [SerializeField] private float distance = 2f;
        [SerializeField] private float angle = 0f;

        void Update()
        {
            cam1.transform.position = pivot + Vector3.left * distance;
            cam1.transform.rotation = Quaternion.identity * Quaternion.AngleAxis(angle, Vector3.up);

            cam2.transform.position = pivot + Vector3.back * distance;
            cam2.transform.rotation = Quaternion.Euler(0, 90, 0) * Quaternion.AngleAxis(-angle, Vector3.up);
        }
    }
}   