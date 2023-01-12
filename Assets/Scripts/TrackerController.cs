using System.Collections.Generic;
using BezierTools;
using mj.gist;
using Osc;
using PrefsGUI;
using PrefsGUI.RapidGUI;
using Unity.Mathematics;
using UnityEngine;

namespace TLF
{
    public class TrackerController : SingletonMonoBehaviour<TrackerController>, IGUIUser
    {
        public Transform WalkAreaTrans => area.transform;

        [SerializeField] private Bezier bezier;
        [SerializeField] private WalkArea area;
        [SerializeField] private Tracker trackerPrefab;

        [SerializeField, Range(0, 1)] private float bezierDebugRate = 0;

        public float SensorAngle => sensorAngle;
        public Vector3 WalkArea => walkArea;

        private List<Tracker> trackers = new List<Tracker>();

        public readonly int TRACK_OBJECT_NUM = 3;

        #region GUI
        public bool DebugMode => trackerDebug;
        private PrefsInt maxTrackers = new PrefsInt("MaxTrackers", 20);
        private PrefsBool trackerDebug = new PrefsBool("TrackerDebug", false);
        private PrefsFloat sensorAngle = new PrefsFloat("SensorAngle", 45);
        private PrefsVector3 walkArea = new PrefsVector3("WalkArea", new Vector3(5f, 1.5f, 5));

        private PrefsVector4 curveRemapY = new PrefsVector4("CurveRemapY", new Vector4(0, 1f, -2f, 2f));
        private PrefsVector4 curveRemapZ = new PrefsVector4("CurveRemapZ", new Vector4(0, 1f, -0.5f, 0.5f));

        public string GetName() => "Tracker";

        public void ShowGUI()
        {
            maxTrackers.DoGUI();
            trackerDebug.DoGUI();
            sensorAngle.DoGUI();
            walkArea.DoGUI();

            curveRemapY.DoGUI();
            curveRemapZ.DoGUI();
        }
        #endregion

        public Vector3 GetPositionOnCurve(Vector3 uvw)
        {
            var pos = bezier.data.Position(uvw.x);

            // y
            pos.y += Mathf.Lerp(curveRemapY.Get().z, curveRemapY.Get().w, math.unlerp(curveRemapY.Get().x, curveRemapY.Get().y, uvw.y));

            // z
            var depth = Mathf.Lerp(curveRemapZ.Get().z, curveRemapZ.Get().w, math.unlerp(curveRemapZ.Get().x, curveRemapZ.Get().y, uvw.z));
            pos += WalkAreaTrans.forward * depth;

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
            if (Main.Instance.Mode == Main.PlayMode.Static || !InteractiveEffect.Instance.Enable) return;

            var uniqueId = (int)e.message.data[0];
            var playerId = (int)e.message.data[1];
            var type = (int)e.message.data[2];
            if (type == 4) return; // head

            var jointId = (int)e.message.data[3];

            var x = (float)e.message.data[4];
            var y = (float)e.message.data[5];
            var z = (float)e.message.data[6];
            var pos = new Vector3(x, y, -z);

            var tracker = trackers[playerId];
            var trackObject = tracker.trackObjects[jointId];
            trackObject.UpdatePosition(uniqueId, pos);
        }

        private void OnDrawGizmos()
        {
            var pos = bezier.data.Position(bezierDebugRate);
            Gizmos.DrawWireSphere(pos, 0.5f);
        }
    }
}
