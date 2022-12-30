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

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            renderer = GetComponent<MeshRenderer>();
            block = new Block(renderer);
            this.seed = UnityEngine.Random.value;
            controller = CapsuleController.Instance;
        }

        public void AddForce(Vector3 pos, Vector3 vel)
        {
            var dir = transform.position - pos;
            rigidbody.AddForce(InteractiveEffect.Instance.GetForce(dir, vel));
        }

        private void Update()
        {
            transform.localScale = controller.GetScale(seed);
            if (!Application.isPlaying) return;

            rigidbody.mass = controller.mass;
            rigidbody.drag = controller.drag;
            rigidbody.angularDrag = controller.angularDrag;

            speed = Mathf.Lerp(rigidbody.velocity.magnitude, speed, controller.speedSmooth);

            block.SetFloat("_EmissiveIntensity", controller.GetEmissionIntensiy(speed));
            block.Apply();
        }
    }
}