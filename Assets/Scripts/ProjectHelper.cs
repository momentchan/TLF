using System.Collections.Generic;
using System.Linq;
using mj.gist;
using UnityEditor;
using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]
    public class ProjectHelper : MonoBehaviour
    {
        [SerializeField] Camera camera;
        [SerializeField] private Color debugColor = Color.red;
        [SerializeField] private List<ControlPoint> controls;
        private Vector2 GetPosition(ControlPointType type)
            => controls.FirstOrDefault(c => c.type == type).position;

        public Matrix4x4 GetHomography(QuadType type)
        {
            var (p0, p1, p2, p3) =
                type == QuadType.Left ?

                (GetPosition(ControlPointType.LeftBottom),
                GetPosition(ControlPointType.LeftTop),
                GetPosition(ControlPointType.MiddleTop),
                GetPosition(ControlPointType.MiddleBottom)) :

                (GetPosition(ControlPointType.MiddleBottom),
                 GetPosition(ControlPointType.MiddleTop),
                 GetPosition(ControlPointType.RightTop),
                 GetPosition(ControlPointType.RightBottom))
                ;

            return TransformHelper.ComputeHomography(p0, p1, p2, p3);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = debugColor;
            for (var i = 0; i < controls.Count; i++)
            {
                var p = controls[i].position;
                var wpos = camera.ViewportToWorldPoint(new Vector3(p.x, p.y, camera.nearClipPlane));
                GizmosUtil.DrawCross(wpos, 1e-3f);
                Handles.Label(wpos, i.ToString());
            }
        }
#endif


        [System.Serializable]
        public class ControlPoint
        {
            public ControlPointType type;
            public Vector2 position;
        }

        public enum ControlPointType
        {
            LeftBottom,
            LeftTop,

            MiddleBottom,
            MiddleTop,

            RightBottom,
            RightTop
        }
    }
}