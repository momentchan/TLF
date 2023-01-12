using UnityEngine;

namespace TLF
{
    public class TrackerObject : MonoBehaviour
    {
        [SerializeField] private int uniqueId;
        [SerializeField] private Transform world;
        [SerializeField] private Transform projected;
        [SerializeField] private Transform interaction;

        private TrackerController controller => TrackerController.Instance;
        private InteractiveEffect effect => InteractiveEffect.Instance;

        private float idleT = Mathf.Infinity;
        private MeshRenderer renderer;

        private Vector3 prevPos;
        private Color emissiveColor;

        public void UpdatePosition(int uniqueId, Vector3 sensorPos)
        {
            this.uniqueId = uniqueId;
            idleT = 0;

            var plane = controller.WalkAreaTrans;

            var wpos = plane.position + Quaternion.AngleAxis(plane.localEulerAngles.y, Vector3.up) * sensorPos;

            var dir = Vector3.ProjectOnPlane(wpos, -plane.forward);
            var projPos = new Vector3(Vector3.Dot(dir, plane.right), Vector3.Dot(dir, plane.up), Mathf.Abs(sensorPos.z));
            var nmlProjPos = Vector3.Scale(projPos, controller.WalkArea.Invert()) + Vector3.right * 0.5f;

            world.transform.position = wpos;
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

            var active = idleT < effect.IdleTime;
            renderer.enabled = active && controller.DebugMode;

            var velocity = (interaction.transform.position - prevPos) / Time.deltaTime;
            var speed = velocity.magnitude;
            var f = speed > effect.SpeedThreshold;
            renderer.material.SetColor("_UnlitColor", f ? Color.red : Color.blue);
            prevPos = interaction.transform.position;

            if (active && f)
            {
                var colliders = Physics.OverlapSphere(interaction.transform.position, effect.InteractiveRange);

                foreach (var collider in colliders)
                {
                    var c = collider.GetComponent<Capsule>();
                    if (c != null)
                    {
                        c.AddForce(interaction.transform.position, effect.GetVelocityFactor(velocity), emissiveColor);
                    }
                }
            }
        }
    }
}