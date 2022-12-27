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
        public void ZoomIn()
        {
            StartCoroutine(StartZoomIn());
        }

        private IEnumerator StartZoomIn()
        {
            yield return null;
            var t = 0f;
            var initPos = box.transform.position;
            while (t < controller.ZoomInT)
            {
                t += Time.deltaTime;
                box.transform.position = controller.GetZoomInPosition(initPos, t);
                yield return null;
            }
            box.gameObject.SetActive(false);
            box.transform.position = initPos;
        }

        public void Reset()
        {
            box.gameObject.SetActive(true);
            box.transform.localScale = Vector3.one;
        }
    }
}