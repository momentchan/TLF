using System.Collections;
using System.Collections.Generic;
using Klak.Hap;
using UnityEngine;

namespace TLF
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private Transform sceneCam;
        [SerializeField] private ToyContainer toyContainer;
        [SerializeField] private Box box1;
        [SerializeField] private Box box2;

        [Header("Movement")]
        [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float moveT = 5f;
        [SerializeField] private float offset = 15f;

        [Header("Expansion")]
        [SerializeField] private AnimationCurve zoomInCurve;
        [SerializeField] private float zoomInT = 2f;
        [SerializeField] private float zoomInDistRatio = 0.1f;
        [SerializeField] private float zoomInHideRatio = 0.7f;
        public float ZoomInT => zoomInT;
        public float ZoomInHideRatio => zoomInHideRatio;
        public Vector3 GetZoomInPosition(Vector3 from, float t)
            => Vector3.Lerp(from, sceneCam.position, zoomInCurve.Evaluate(t / zoomInT) * zoomInDistRatio);

        private void Start()
        {
            box1.Setup(this);
            box2.Setup(this);
        }
       
        public void ScaleUp()
        {
            box1.ZoomIn();
        }

        public void MoveBoxes()
        {
            StartCoroutine(StartMove());
        }

        IEnumerator StartMove()
        {
            yield return null;

            var t = 0f;
            var initPos1 = box1.transform.localPosition;
            var initPos2 = box2.transform.localPosition;
            while (t < moveT)
            {
                t += Time.deltaTime;
                var dist = -Mathf.Lerp(0, offset, moveCurve.Evaluate(t / moveT));
                box1.transform.localPosition = initPos1 + Vector3.right * dist;
                box2.transform.localPosition = initPos2 + Vector3.right * dist;
                yield return null;
            }
            box1.transform.localPosition = Vector3.right * offset;
            box1.CopyConentTo(box2);
            box1.Reset();

            var temp = box1;
            box1 = box2;
            box2 = temp;

            toyContainer.SwitchToy();
        }
    }
}
