using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLF
{
    public class TrackerObject : MonoBehaviour
    {
        public float debugSize = 0.2f;

        [SerializeField] private Color worldColor = Color.red;
        [SerializeField] private Color projectColor = Color.green;

        public Vector3 worldPos;
        public Vector3 projectedPos;
        public Vector3 normalizeProjectedPos;


        private void OnDrawGizmos()
        {
            var plane = TrackerController.Instance.ProjectPlane;

            Gizmos.color = worldColor;
            Gizmos.DrawSphere(worldPos, debugSize);
            Gizmos.color = projectColor;
            Gizmos.DrawSphere(plane.position + plane.right * projectedPos.x + plane.up * projectedPos.y, debugSize);
        }
    }
}