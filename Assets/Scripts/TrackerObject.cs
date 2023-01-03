using System.Collections;
using System.Collections.Generic;
using com.rfilkov.kinect;
using UnityEngine;

namespace TLF
{
    public class TrackerObject : MonoBehaviour
    {
        public float debugSize = 0.2f;

        [SerializeField] private KinectInterop.JointType jointType;
        [SerializeField] private int uniqueId;

        private Vector3 worldPos;
        private Vector3 projectedPos;
        private Vector3 normalizeProjectedPos;

        private TrackerController controller;
        private float idleT = Mathf.Infinity;
        private MeshRenderer renderer;

        private Color emissiveColor;
        private Color worldColor = Color.red;
        private Color projectColor = Color.green;

        public void UpdateData(int uniqueId, KinectInterop.JointType type, Vector3 wpos, Vector3 projPos, Vector3 nmlProjPos)
        {
            this.uniqueId = uniqueId;
            jointType = type;

            worldPos = wpos;
            projectedPos = projPos;
            normalizeProjectedPos = nmlProjPos;

            idleT = 0;

            RandomUtil.RandomState(() =>
            {
                emissiveColor = Random.ColorHSV(0, 1, 0.5f, 1f, 1f, 1f);
            }, uniqueId);
        }

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
                var colliders = Physics.OverlapSphere(transform.position, InteractiveEffect.Instance.Range);

                foreach (var collider in colliders)
                {
                    var c = collider.GetComponent<Capsule>();
                    if (c != null)
                    {
                        c.AddForce(transform.position, InteractiveEffect.Instance.GetVelocityFactor(velocity), emissiveColor);
                    }
                }
            }
        }

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