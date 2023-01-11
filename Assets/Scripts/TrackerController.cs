using System.Collections.Generic;
using BezierTools;
using mj.gist;
using Osc;
using PrefsGUI;
using PrefsGUI.RapidGUI;
using UnityEngine;

namespace TLF
{
    public class TrackerController : SingletonMonoBehaviour<TrackerController>, IGUIUser
    {
        public Transform ProjectPlane => projectPlane;

        [SerializeField] private Bezier bezier;
        [SerializeField] private Transform projectPlane;
        [SerializeField] private Tracker trackerPrefab;

        [SerializeField, Range(0, 1)] private float bezierDebugRate = 0;

        private PrefsInt maxTrackers = new PrefsInt("MaxTrackers", 20);

        [SerializeField] private Vector2 interactiveRangeY = new Vector2(0.3f, 0.8f);
        [SerializeField] private Vector2 interactiveRangeZ = new Vector2(0.3f, 0.5f);
        [SerializeField] private Vector3 projectRange = new Vector3(10, 2, 1);
        [SerializeField] private bool renderObject;
        public bool RenderObject => renderObject;
        public Vector3 ProjectRange => projectRange;

        [SerializeField] private float maxIdleTime = 2f;
        public float MaxIdleTime => maxIdleTime;

        public Vector3 extent = Vector3.one;
        public Vector3 rot;

        private List<Tracker> trackers = new List<Tracker>();

        public readonly int TRACK_OBJECT_NUM = 3;

        #region GUI
        public string GetName() => "Tracker";

        public void ShowGUI()
        {
            maxTrackers.DoGUI();
        }

        #endregion

        public Vector3 GetPositionOnCurve(Vector3 uvw)
        {
            var pos = bezier.data.Position(uvw.x);
            pos.y += Mathf.Lerp(-projectRange.y, projectRange.y, interactiveRangeY.Interpolate(uvw.y));
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
            var type = (int)e.message.data[2];
            if (type == 4) return; // head

            var jointId = (int)e.message.data[3];

            var x = (float)e.message.data[4];
            var y = (float)e.message.data[5];
            var z = (float)e.message.data[6];
            var pos = new Vector3(x, y, -z);

            var trackObject = trackers[playerId].trackObjects[jointId];
            var wpos = projectPlane.position + Quaternion.AngleAxis(projectPlane.rotation.eulerAngles.y, Vector3.up) * pos;
            var dir = Vector3.ProjectOnPlane(wpos, -projectPlane.forward);
            var projPos = new Vector3(Vector3.Dot(dir, projectPlane.right), Vector3.Dot(dir, projectPlane.up), 0);
            var nmlProjPos = Vector3.Scale(projPos, new Vector3(1 / ProjectRange.x, 1 / ProjectRange.y, 0)) + Vector3.right * 0.5f;

            trackObject.UpdatePosition(uniqueId, wpos, projPos, nmlProjPos);
        }

        private void OnDrawGizmos()
        {
            var pos = bezier.data.Position(bezierDebugRate);
            Gizmos.DrawWireSphere(pos, 0.5f);
            Gizmos.DrawRay(pos, projectPlane.transform.forward);
            Gizmos.DrawLine(pos + Vector3.down * projectRange.y, pos + Vector3.up * projectRange.y);
        }
    }
}
