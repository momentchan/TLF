using System.Collections;
using System.Collections.Generic;
using System.Linq;
using mj.gist;
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
    public class CapsuleController : SingletonMonoBehaviour<CapsuleController>
    {
        [SerializeField] FoamTransformDataset foams;
        [SerializeField] private Capsule prefab;
        [SerializeField] private PhysicMaterial physicMat;
        [SerializeField] private bool generate;
        [SerializeField] private int length = 10;

        [SerializeField] Bounds bounds;

        [Header("Appearance")]
        [SerializeField] private Vector3 size = Vector3.one;
        [SerializeField] private Vector2 sizeRange = new Vector2(0.8f, 1f);
        [SerializeField] private Vector2 speedRange = new Vector2(0, 0.1f);

        [Header("Physics")]
        [SerializeField] public float mass = 1f;
        [SerializeField] public float drag = 2f;
        [SerializeField] public float angularDrag = 0.05f;
        [SerializeField] private float staticFriction = 0.6f;
        [SerializeField] private float dynamicFriction = 0.6f;
        [SerializeField, Range(0, 1)] private float bounciness = 0.6f;
        [SerializeField, Range(0, 1)] public float speedSmooth = 0.5f;
        [SerializeField] private List<Capsule> capsules = new List<Capsule>();

        
        public Vector3 GetScale(float seed) => size * Mathf.Lerp(sizeRange.x, sizeRange.y, seed);
        public float GetEmissionIntensiy(float speed) => math.remap(speedRange.x, speedRange.y, 0, 1, speed);

        void Awake()
        {
            if (generate)
                CreateCapsules();
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