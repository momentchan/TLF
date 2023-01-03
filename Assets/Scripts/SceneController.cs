using System.Collections;
using UnityEngine;

namespace TLF
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private Transform sceneCam;
        [SerializeField] private BoxContainer container1;
        [SerializeField] private BoxContainer container2;
        [SerializeField] private ToyContainer toyContainer;

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
            container1.Setup(this);
            container2.Setup(this);
        }

        public void ScaleUp()
        {
            container1.ZoomIn();
        }

        public void MoveBoxes()
        {
            StartCoroutine(StartMove());
        }

        IEnumerator StartMove()
        {
            yield return null;

            var t = 0f;
            var initPos1 = container1.transform.localPosition;
            var initPos2 = container2.transform.localPosition;
            while (t < moveT)
            {
                t += Time.deltaTime;
                var dist = -Mathf.Lerp(0, offset, moveCurve.Evaluate(t / moveT));
                container1.transform.localPosition = initPos1 + Vector3.right * dist;
                container2.transform.localPosition = initPos2 + Vector3.right * dist;
                yield return null;
            }
            container1.transform.localPosition = Vector3.right * offset;
            container1.CopyConentTo(container2);
            container1.Reset();

            var temp = container1;
            container1 = container2;
            container2 = temp;

            toyContainer.SwitchToy();
        }
    }
}
