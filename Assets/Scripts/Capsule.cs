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

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            renderer = GetComponent<MeshRenderer>();
            block = new Block(renderer);
        }
        public void Setup(CapsuleController controller)
        {
            this.controller = controller;
            this.seed = UnityEngine.Random.value;
        }

        private void Update()
        {
            transform.localScale = controller.GetScale(seed);
            if (!Application.isPlaying) return;

            rigidbody.mass = controller.mass;
            rigidbody.drag = controller.drag;

            block.SetFloat("_EmissiveIntensity", controller.GetEmissionIntensiy(rigidbody.velocity.magnitude));
            block.Apply();
        }
    }
}