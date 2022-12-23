using System.Collections;
using System.Collections.Generic;
using System.Linq;
using mj.gist;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ProjectController : MonoBehaviour
{
    [SerializeField] private SimulateQuad leftQuad;
    [SerializeField] private SimulateQuad rightQuad;

    [SerializeField] Camera camera;
    [SerializeField] private Color color;
    [SerializeField] private List<ControlPoint> controls;

    void Update()
    {
        leftQuad.SetCoordinates(GetPosition(ControlPointType.LeftBottom),
                                GetPosition(ControlPointType.LeftTop),
                                GetPosition(ControlPointType.MiddleTop),
                                GetPosition(ControlPointType.MiddleBottom));

        rightQuad.SetCoordinates(GetPosition(ControlPointType.MiddleBottom),
                                GetPosition(ControlPointType.MiddleTop),
                                GetPosition(ControlPointType.RightTop),
                                GetPosition(ControlPointType.RightBottom));
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        for (var i = 0; i < controls.Count; i++)
        {
            var p = controls[i].position;
            var wpos = camera.ViewportToWorldPoint(new Vector3(p.x, p.y, camera.nearClipPlane));
            GizmosUtil.DrawCross(wpos, 1e-3f);
            Handles.Label(wpos, i.ToString());
        }
    }
#endif

    public Vector2 GetPosition(ControlPointType type) => controls.FirstOrDefault(c => c.type == type).position;

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
