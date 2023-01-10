using System.Collections;
using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]
    public class Box : MonoBehaviour
    {
        [SerializeField] private MeshRenderer box;
        [SerializeField] private BoxType type;

        private SceneController controller;
        public GameObject content;

        public void Setup(SceneController controller)
            => this.controller = controller;

        public void CopyConentTo(Box c2)
        {
            content.transform.parent = c2.transform;
            content.transform.localPosition = Vector3.zero;

            c2.content = content;
            content = null;
        }

        private void Start()
        {
            box.gameObject.SetActive(true);
        }

        private void Update()
        {
            foreach(var m in box.sharedMaterials)
            {
                m.SetFloat("_TestPatternBlend", Projection.Instance.TestPatternBlend);
            }
        }

        public void ZoomIn()
        {
            StartCoroutine(StartZoomIn());
        }

        private IEnumerator StartZoomIn()
        {
            yield return null;
            var t = 0f;
            var initPos = transform.position;
            while (t < controller.ZoomInT)
            {
                t += Time.deltaTime;
                transform.position = controller.GetZoomInPosition(initPos, t);

                if (t / controller.ZoomInT > controller.ZoomInHideRatio)
                    box.gameObject.SetActive(false);

                yield return null;
            }
        }

        public void Reset()
        {
            box.gameObject.SetActive(true);
        }
    }

    public enum BoxType { FrontBox, BackBox }
}