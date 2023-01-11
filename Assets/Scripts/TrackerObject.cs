using UnityEngine;

namespace TLF
{
    public class TrackerObject : MonoBehaviour
    {
        [SerializeField] private int uniqueId;

        private TrackerController controller => TrackerController.Instance;
        private float idleT = Mathf.Infinity;
        private MeshRenderer renderer;

        [SerializeField] private Transform world;
        [SerializeField] private Transform projected;
        [SerializeField] private Transform interaction;

        private Vector3 prevPos;
        private Color emissiveColor;

        public void UpdatePosition(int uniqueId, Vector3 wpos, Vector3 projPos, Vector3 nmlProjPos)
        {
            this.uniqueId = uniqueId;
            idleT = 0;

            world.transform.position = wpos;

            var plane = controller.ProjectPlane;
            projected.transform.position = plane.position + plane.right * projPos.x + plane.up * projPos.y;

            interaction.transform.position = controller.GetPositionOnCurve(nmlProjPos);
            interaction.transform.localScale = InteractiveEffect.Instance.InteractiveRange * Vector3.one;

            RandomUtil.RandomState(() =>
            {
                emissiveColor = Random.ColorHSV(0, 1, 0.5f, 1f, 1f, 1f);
            }, uniqueId);
        }


        private void Start()
        {
            renderer = interaction.GetComponent<MeshRenderer>();
            prevPos = interaction.transform.position;
        }

        private void Update()
        {
            idleT += Time.deltaTime;

            var active = idleT < controller.MaxIdleTime;
            SetActive(active);

            var velocity = (interaction.transform.position - prevPos) / Time.deltaTime;
            prevPos = interaction.transform.position;

            if (active)
            {
                var colliders = Physics.OverlapBox(interaction.transform.position, controller.extent, Quaternion.Euler(controller.rot));
                //var colliders = Physics.OverlapSphere(transform.position, InteractiveEffect.Instance.InteractiveRange);

                foreach (var collider in colliders)
                {
                    var c = collider.GetComponent<Capsule>();
                    if (c != null)
                    {
                        c.AddForce(interaction.transform.position, InteractiveEffect.Instance.GetVelocityFactor(velocity), emissiveColor);
                    }
                }
            }
        }

        private void SetActive(bool active)
        {
            renderer.enabled = active && controller.RenderObject;
        }
    }
}