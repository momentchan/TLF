using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace TLF
{
    /// <summary>
    /// TODO: refactoring
    /// </summary>
    public class CapsuleController : MonoBehaviour
    {
        [SerializeField] FoamTransformDataset foams;
        [SerializeField] private Capsule prefab;
        [SerializeField] public int length = 10;

        [SerializeField] public int count = 1024;
        [SerializeField] Bounds bounds;
        [SerializeField] private List<Capsule> capsules = new List<Capsule>();
        [SerializeField] private PhysicMaterial physicMat;
        [SerializeField] private Material renderMat;
        [SerializeField, Range(0, 1)] private float bounciness = 0.6f;

        [SerializeField] public float mass = 1f;
        [SerializeField] public float drag = 2f;
        [SerializeField] private float staticFriction = 0.6f;
        [SerializeField] private float dynamicFriction = 0.6f;

        [SerializeField] private Vector2 sizeRange = new Vector2(0.8f, 1f);
        [SerializeField] private Vector3 size = Vector3.one;
        public Vector3 GetScale(float seed) => size * Mathf.Lerp(sizeRange.x, sizeRange.y, seed);

        void Awake()
        {
            //CreateCapsules();
        }

        public float4x4 GetStickMatrix(float4x4 xform, int id)
        {
            var capsule = capsules[id];
            var m1 = float4x4.Translate(capsule.transform.localPosition);
            var m2 = float4x4.EulerXYZ(capsule.transform.localRotation.eulerAngles);
            return math.mul(math.mul(xform, m1), m2);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                Clear();

            physicMat.bounciness = bounciness;
            physicMat.staticFriction = staticFriction;
            physicMat.dynamicFriction = dynamicFriction;
        }

        private void CreateCapsules()
        {
            Clear();
            for (var i = 0; i < length; i++)
                for (var j = 0; j < length; j++)
                    for (var k = 0; k < length; k++)
                    {
                        var pos = bounds.center + new Vector3(
                            Mathf.Lerp(-0.5f, 0.5f, 1f * i / length) * bounds.size.x,
                            Mathf.Lerp(-0.5f, 0.5f, 1f * j / length) * bounds.size.y,
                            Mathf.Lerp(-0.5f, 0.5f, 1f * k / length) * bounds.size.z);

                        CreateCapsule(pos, UnityEngine.Random.rotation);
                    }
        }

        [ContextMenu("Generate")]
        private void CreateCapsulesFromDataset()
        {
            Clear();
            foreach (var d in foams.GetTransformData())
                CreateCapsule(d.localPosition, d.localRotation);
        }

        private void CreateCapsule(Vector3 pos, Quaternion rot)
        {
            var c = Instantiate(prefab, transform);
            c.transform.position = pos;
            c.transform.rotation = rot;
            c.Setup(this);
            capsules.Add(c);
        }

        [ContextMenu("Save")]
        void SaveTransform()
        {
            foams.SaveTransform(capsules.Select(c => c.transform).ToList());
        }

        void Clear()
        {
            foreach (var c in capsules)
                DestroyImmediate(c.gameObject);
            capsules.Clear();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}