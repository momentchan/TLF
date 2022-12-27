using mj.gist;
using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]
    public class Capsule : MonoBehaviour
    {
        [SerializeField] private CapsuleController controller;
        [SerializeField] private float seed; 

        new Rigidbody rigidbody;
        new MeshRenderer renderer;

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
            this.seed = Random.value;
        }

        private void Update()
        {
            transform.localScale = controller.GetScale(seed);

            rigidbody.mass = controller.mass;
            rigidbody.drag = controller.drag;

            block.SetFloat("_Speed", rigidbody.velocity.magnitude);
            block.Apply();
        }
    }
}