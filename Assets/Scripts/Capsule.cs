using mj.gist;
using Unity.Mathematics;
using UnityEngine;
using static TLF.CapsuleController;

namespace TLF
{
    [ExecuteInEditMode]
    public class Capsule : MonoBehaviour
    {
        [SerializeField] private CapsuleController controller;
        [SerializeField] private CapsuleKind kind;
        [SerializeField] private float seed;

        new Rigidbody rigidbody;
        [SerializeField] private MeshRenderer renderer;

        private Block block;
        private float speed;

        private Color emissiveColor;
        private Vector3 initLocalPos;
        private quaternion initLocalRot;

        void Start()
        {
            initLocalPos = transform.localPosition;
            initLocalRot = transform.localRotation;

            rigidbody = GetComponent<Rigidbody>();
            renderer = GetComponent<MeshRenderer>();
            block = new Block(renderer);
            this.seed = UnityEngine.Random.value;
            controller = CapsuleController.Instance;
        }

        public void AddForce(Vector3 pos, Vector3 vel, Color color)
        {
            var dir = transform.position - pos;
            AddForce(InteractiveEffect.Instance.GetForce(dir, vel), Vector2.one, color);
        }

        public void AddForce(Vector3 force, Vector2 random, Color color, bool colorOverride = false)
        {
            force *= Mathf.Lerp(random.x, random.y, seed);
            rigidbody.AddForce(force);
            emissiveColor = colorOverride ? color : Color.Lerp(color, emissiveColor, InteractiveEffect.Instance.ColorBlend);
        }

        public void Reset()
        {
            transform.localPosition = initLocalPos;
            transform.localRotation = initLocalRot;
            emissiveColor = Color.black;
        }

        private void Update()
        {
            if (!Application.isPlaying) return;

            if (!controller.Bounds.Contains(transform.position))
                Reset();

            transform.localScale = controller.GetScale(seed, kind);

            rigidbody.drag = controller.Drag;
            rigidbody.angularDrag = controller.AngularDrag;

            speed = Mathf.Lerp(rigidbody.velocity.magnitude, speed, controller.SpeedSmooth);

            block.SetFloat("_EmissiveIntensity", controller.GetEmissionIntensiy(speed));
            block.SetColor("_EmissiveColor", emissiveColor);
            block.Apply();
        }
    }
}