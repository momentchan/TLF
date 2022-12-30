using mj.gist;
using Unity.Mathematics;
using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]
    public class Capsule : MonoBehaviour
    {
        [SerializeField] private CapsuleController controller;
        [SerializeField] private float seed;

        new Rigidbody rigidbody;
        [SerializeField] private MeshRenderer renderer;

        private Block block;
        private float speed;

        private Color emissiveColor;
        private float colorBlend = 0;

        private float idleTimer = 0;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            renderer = GetComponent<MeshRenderer>();
            block = new Block(renderer);
            this.seed = UnityEngine.Random.value;
            controller = CapsuleController.Instance;
        }

        public void AddForce(Vector3 pos, Vector3 vel, Color color)
        {
            var dir = transform.position - pos;
            rigidbody.AddForce(InteractiveEffect.Instance.GetForce(dir, vel));

            colorBlend += InteractiveEffect.Instance.colorBlendRate * 2;
            emissiveColor = color;
        }

        private void Update()
        {
            if (!Application.isPlaying) return;

            colorBlend -= InteractiveEffect.Instance.colorBlendRate;

            colorBlend = Mathf.Clamp01(colorBlend);
            
            transform.localScale = controller.GetScale(seed);

            rigidbody.mass = controller.mass;
            rigidbody.drag = controller.drag;
            rigidbody.angularDrag = controller.angularDrag;

            speed = Mathf.Lerp(rigidbody.velocity.magnitude, speed, controller.speedSmooth);



            block.SetFloat("_EmissiveIntensity", controller.GetEmissionIntensiy(speed));
            block.SetColor("_EmissiveColor", Color.Lerp(controller.GetEmissiveColor(), emissiveColor, colorBlend));
            block.Apply();
        }
    }
}