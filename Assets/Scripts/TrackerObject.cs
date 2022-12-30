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
        private MeshRenderer renderer;
        private Tracker tracker;
        public void Setup(Tracker tracker) => this.tracker = tracker;

        private Vector3 prevPos;

        private void Start()
        {
            controller = TrackerController.Instance;
            renderer = GetComponent<MeshRenderer>();
            prevPos = transform.position;
        }

        private void Update()
        {
            idleT += Time.deltaTime;

            var active = idleT < controller.MaxIdleTime;
            SetActive(active);

            var velocity = (transform.position - prevPos) / Time.deltaTime;
            prevPos = transform.position;

            if (active)
            {
                var colliders = Physics.OverlapSphere(transform.position, controller.ObjectScale);

                foreach (var collider in colliders)
                {
                    var c = collider.GetComponent<Capsule>();
                    if (c != null)
                    {
                        c.AddForce(transform.position, InteractiveEffect.Instance.GetVelocityFactor(velocity), tracker.color);
                    }
                }
            }
        }

        public void Activate() => idleT = 0;

        private void SetActive(bool active)
        {
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