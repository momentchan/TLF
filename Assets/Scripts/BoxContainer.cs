using System.Collections;
using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]
    public class BoxContainer : MonoBehaviour
    {
        [SerializeField] private MeshRenderer box;

        private SceneController controller;
        public GameObject content;

        public void Setup(SceneController controller)
            => this.controller = controller;

        public void CopyConentTo(BoxContainer c2)
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
}