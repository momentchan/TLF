using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace TLF
{
    public class CoordinateTransformer : MonoBehaviour
    {
        [SerializeField] private Color color;
        [SerializeField] private List<Vector2> points;

        Camera _camera;

        Camera Camera
        {
            get
            {
                if (_camera == null)
                    _camera = GetComponent<Camera>();
                return _camera;
            }
        }

        void Start()
        {

        }


        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            for (var i = 0; i < points.Count; i++) 
            {
                var p = points[i];
                var wpos = Camera.ViewportToWorldPoint(new Vector3(p.x, p.y, Camera.farClipPlane));
                Gizmos.DrawSphere(wpos, 0.5f);
                Handles.Label(wpos, i.ToString());
            }
        }
        void Update()
        {

        }
    }
}