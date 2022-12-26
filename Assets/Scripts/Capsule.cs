using System;
using mj.gist;
using UnityEngine;

namespace TLF
{
    public class Capsule : MonoBehaviour
    {
        new Rigidbody rigidbody;
        new MeshRenderer renderer;

        private Block block;
        private CapsuleController controller;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            renderer = GetComponent<MeshRenderer>();
            block = new Block(renderer);
        }
        public void Setup(CapsuleController controller)
        {
            this.controller = controller;
        }

        private void Update()
        {
            renderer.enabled = !controller.useDrawMesh;
            rigidbody.mass = controller.mass;
            rigidbody.drag = controller.drag;

            block.SetFloat("_Speed", rigidbody.velocity.magnitude);
            block.Apply();
        }
    }
}