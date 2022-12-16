using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ProjectController : MonoBehaviour
{
    [SerializeField] private KeyCode leftKey = KeyCode.Q;
    [SerializeField] private KeyCode rightKey = KeyCode.W;
    [SerializeField] private SimulateQuad leftQuad;
    [SerializeField] private SimulateQuad rightQuad;

    [SerializeField] Camera camera;
    [SerializeField] private Color color;
    [SerializeField] private List<Vector2> points;
    [SerializeField] private List<ControlPoint> controls;
    [SerializeField] private bool debug;

    void Update()
    {
        if (Input.GetKeyDown(leftKey))
        {
            leftQuad.gameObject.SetActive(!leftQuad.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(rightKey))
        {
            rightQuad.gameObject.SetActive(!rightQuad.gameObject.activeSelf);
        }

        leftQuad.SetCoordinates(GetPosition(ControlPointType.LeftBottom),
                                GetPosition(ControlPointType.LeftTop),
                                GetPosition(ControlPointType.MiddleTop),
                                GetPosition(ControlPointType.MiddleBottom));

        rightQuad.SetCoordinates(GetPosition(ControlPointType.MiddleBottom),
                                GetPosition(ControlPointType.MiddleTop),
                                GetPosition(ControlPointType.RightTop),
                                GetPosition(ControlPointType.RightBottom));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        for (var i = 0; i < points.Count; i++)
        {
            var p = points[i];
            var wpos = camera.ViewportToWorldPoint(new Vector3(p.x, p.y, camera.farClipPlane));
            Gizmos.DrawSphere(wpos, 0.5f);
            Handles.Label(wpos, i.ToString());
        }
    }


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
