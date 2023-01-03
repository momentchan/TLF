using System.Collections.Generic;
using BezierTools;
using com.rfilkov.kinect;
using mj.gist;
using Osc;
using UnityEngine;

namespace TLF
{
    public class TrackerController : SingletonMonoBehaviour<TrackerController>
    {
        public Transform ProjectPlane => projectPlane;

        [SerializeField] private Bezier bezier;
        [SerializeField] private Transform projectPlane;
        [SerializeField] private Tracker trackerPrefab;

        [SerializeField, Range(0, 1)] private float rate = 0;
        [SerializeField] private int maxTrackers = 10;
        [SerializeField] private Vector2 interactiveRange = new Vector2(0.5f, 0.8f);
        [SerializeField] private Vector2 heightRange;
        [SerializeField] private Vector3 projectRange = new Vector3(10, 2, 1);
        [SerializeField] private bool renderObject;
        public bool RenderObject => renderObject;
        public Vector3 ProjectRange => projectRange;

        public float ObjectScale => objectScale;

        [SerializeField] private float objectScale = 1f;
        [SerializeField] private float angle = 25f;
        [SerializeField] private float maxIdleTime = 2f;
        public float MaxIdleTime => maxIdleTime;


        [SerializeField] private List<Tracker> trackers = new List<Tracker>();

        public readonly int TRACK_OBJECT_NUM = 3;

        public Vector3 GetPositionOnCurve(Vector2 uv)
        {
            var pos = bezier.data.Position(uv.x);
            pos.y += Mathf.Lerp(heightRange.x, heightRange.y, interactiveRange.Interpolate(uv.y));
            return pos;
        }

        private void Start()
        {
            for (var i = 0; i < maxTrackers; i++)
            {
                var tracker = Instantiate(trackerPrefab, transform);
                tracker.Setup(i);
                trackers.Add(tracker);
            }
        }

        public virtual void OscMessageReceived(OscPort.Capsule e)
        {
            if (Main.Instance.Mode == Main.PlayMode.Static) return;

            var uniqueId = (int)e.message.data[0];
            var playerId = (int)e.message.data[1];
            var type = (KinectInterop.JointType)e.message.data[2];
            if (type == KinectInterop.JointType.Head) return;
            var jointId = (int)e.message.data[3];

            var x = (float)e.message.data[4];
            var y = (float)e.message.data[5];
            var z = (float)e.message.data[6];

            var trackObject = trackers[playerId].trackObjects[jointId];

            var pos = new Vector3(x, y, -z);
            var wpos = Quaternion.AngleAxis(angle, Vector3.up) * pos + projectPlane.position;
            var dir = Vector3.ProjectOnPlane(wpos, -projectPlane.forward);
            var projPos = new Vector3(Vector3.Dot(dir, projectPlane.right), Vector3.Dot(dir, projectPlane.up), 0);
            var nmlProjPos = Vector3.Scale(projPos, new Vector3(1 / ProjectRange.x, 1 / ProjectRange.y, 0)) + Vector3.right * 0.5f;

            trackObject.UpdateData(uniqueId, type, wpos, projPos, nmlProjPos);

            trackObject.transform.position = GetPositionOnCurve(nmlProjPos);
            trackObject.transform.localScale = ObjectScale * Vector3.one;
        }

        private void OnDrawGizmos()
        {
            var pos = bezier.data.Position(rate);
            Gizmos.DrawWireSphere(pos, 0.5f);
            Gizmos.DrawLine(pos + Vector3.up * heightRange.x, pos + Vector3.up * heightRange.y);
        }
    }
}
