using System.Collections;
using System.Collections.Generic;
using com.rfilkov.kinect;
using UnityEngine;

namespace TLF
{
    public class TrackerObject : MonoBehaviour
    {
        public float debugSize = 0.2f;

        [SerializeField] public KinectInterop.JointType jointType;
        [SerializeField] private Color worldColor = Color.red;
        [SerializeField] private Color projectColor = Color.green;

        public Vector3 worldPos;
        public Vector3 projectedPos;
        public Vector3 normalizeProjectedPos;

        private TrackerController controller;
        private float idleT = Mathf.Infinity;
        private Collider collider;
        private MeshRenderer renderer;

        private void Start()
        {
            controller = TrackerController.Instance;
            collider = GetComponent<Collider>();
            renderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            idleT += Time.deltaTime;
            SetActive(idleT < controller.MaxIdleTime);
        }

        public void Activate()
        {
            idleT = 0;
        }

        private void SetActive(bool active)
        {
            collider.enabled = active;
            renderer.enabled = active && controller.RenderObject;
        }


        private void OnDrawGizmos()
        {
            var plane = controller.ProjectPlane;

            Gizmos.color = worldColor;
            Gizmos.DrawSphere(worldPos, debugSize);
            Gizmos.color = projectColor;
            Gizmos.DrawSphere(plane.position + plane.right * projectedPos.x + plane.up * projectedPos.y, debugSize);
        }
    }
}