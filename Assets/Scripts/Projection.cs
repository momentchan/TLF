using System.Collections.Generic;
using System.Linq;
using mj.gist;
using PrefsGUI;
using PrefsGUI.RapidGUI;
using UnityEditor;
using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]
    public class Projection : SingletonMonoBehaviour<Projection>, IGUIUser
    {
        [SerializeField] Camera camera;
        [SerializeField] private Color debugColor = Color.red;

        private PrefsVector2 leftBottom = new PrefsVector2("LeftBottom");
        private PrefsVector2 leftTop = new PrefsVector2("LeftTop");
        private PrefsVector2 middleBottom = new PrefsVector2("MiddleBottom");
        private PrefsVector2 middleTop = new PrefsVector2("MiddleTop");
        private PrefsVector2 rightBottom = new PrefsVector2("RightBottom");
        private PrefsVector2 rightTop = new PrefsVector2("RightTop");

        private PrefsFloat testPatternBlend = new PrefsFloat("TestPatternBlend");
        public float TestPatternBlend => testPatternBlend;

        #region GUI
        public string GetName() => "Projection";

        public void ShowGUI()
        {
            testPatternBlend.DoGUISlider(0, 1);
            leftBottom.DoGUI();
            leftTop.DoGUI();
            middleBottom.DoGUI();
            middleTop.DoGUI();
            rightBottom.DoGUI();
            rightTop.DoGUI();
        }
        #endregion

        public Matrix4x4 GetHomography(QuadType type)
        {
            var (p0, p1, p2, p3) =
                type == QuadType.Left ?

                (leftBottom,
                leftTop,
                middleTop,
                middleBottom) :

                (middleBottom,
                 middleTop,
                 rightTop,
                 rightBottom)
                ;

            return TransformHelper.ComputeHomography(p0, p1, p2, p3);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = debugColor;

            DrawControlPoint(leftBottom);
            DrawControlPoint(leftTop);
            DrawControlPoint(middleBottom);
            DrawControlPoint(middleTop);
            DrawControlPoint(rightBottom);
            DrawControlPoint(rightTop);
        }

        private void DrawControlPoint(Vector2 p)
        {
            var wpos = camera.ViewportToWorldPoint(new Vector3(p.x, p.y, camera.nearClipPlane));
            GizmosUtil.DrawCross(wpos, 1e-3f);
        }
#endif
    }

    public enum QuadType
    {
        Left, Right
    }
}