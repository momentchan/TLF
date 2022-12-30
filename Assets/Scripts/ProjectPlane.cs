using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]
    public class ProjectPlane : MonoBehaviour
    {
        void Start()
        {
        }

        void Update()
        {
            transform.localScale = TrackerController.Instance.ProjectRange;
        }
    }
}