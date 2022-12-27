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
        [SerializeField] private Capsule prefab;
        [SerializeField] private Vector3 center;
        [SerializeField] private Vector3 size = Vector3.one;
        [SerializeField] private List<Capsule> capsules = new List<Capsule>();
        [SerializeField] private GameObject obstacle;
        [SerializeField] private float radius = 1;
        [SerializeField] private float spawnRate = 0.2f;
        [SerializeField] public int maxCount = 100;
        [SerializeField] private PhysicMaterial physicMat;
        [SerializeField] private Material renderMat;
        [SerializeField, Range(0, 1)] private float bounciness = 0.6f;
        [SerializeField] public float mass = 1f;
        [SerializeField] public float drag = 2f;
        [SerializeField] private float staticFriction = 0.6f;
        [SerializeField] private float dynamicFriction = 0.6f;

        [SerializeField] FoamTransformDataset foams;
        public TransformAccessArray Trs => trs;
        protected TransformAccessArray trs;
        [SerializeField] public bool useDrawMesh = true;
        [SerializeField] ObjectRenderer r;
        public List<Matrix4x4> matricess = new List<Matrix4x4>();

        void Awake()
        {
            trs = new TransformAccessArray(0);
            for (var i = 0; i < maxCount; i++)
            {
                trs.Add(capsules[i].transform);
                matricess.Add(capsules[i].transform.localToWorldMatrix);
            }
            foreach(var c in capsules)
            {
                c.Setup(this);
            }
        }
        private void OnDestroy()
        {
            trs.Dispose();
        }
        public float4x4 GetStickMatrix(float4x4 xform, int id)
        {
            var capsule = capsules[id];
            var m1 = float4x4.Translate(capsule.transform.localPosition);
            var m2 = float4x4.EulerXYZ(capsule.transform.localRotation.eulerAngles);
            return math.mul(math.mul(xform, m1), m2);
        }
        IEnumerator G()
        {
            Clear();
            yield return null;
            while(capsules.Count< maxCount)
            {
                RandomGenerate();
                yield return new WaitForSeconds(0.05f);
            }
        }

        void Update()
        {
            r.enabled = useDrawMesh;

            if (Input.GetKeyDown(KeyCode.G))
                RandomGenerate();
            if (Input.GetKeyDown(KeyCode.C))
                Clear();

            physicMat.bounciness = bounciness;
            physicMat.staticFriction = staticFriction;
            physicMat.dynamicFriction = dynamicFriction;
        }

        void RandomGenerate()
        {
            var pos = Vector3.zero;
            for (var i = 0; i < 20; i++)
            {
                pos = center + new Vector3(
                    Mathf.Lerp(-0.5f, 0.5f, UnityEngine.Random.value) * size.x,
                    Mathf.Lerp(0.2f, 0.4f, UnityEngine.Random.value) * size.y,
                    Mathf.Lerp(-0.5f, 0.5f, UnityEngine.Random.value) * size.z);

                var op = obstacle.transform.position;
                if (Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(op.x, op.z)) > radius)
                    break;
            }

            var c = Instantiate(prefab, transform);
            c.transform.position = pos;
            c.transform.rotation = UnityEngine.Random.rotation;
            c.Setup(this);
            capsules.Add(c);
        }

        [ContextMenu("Save")]
        void SaveTransform()
        {
            foams.SaveTransform(capsules.Select(c => c.transform).ToList());
        }

        [ContextMenu("Generate")]
        void Generate()
        {
            Clear();
            foreach (var d in foams.GetTransformData())
            {
                var c = Instantiate(prefab, transform);
                c.transform.localPosition = d.localPosition;
                c.transform.localRotation = d.localRotation;
                c.transform.localScale = d.localScale;
                capsules.Add(c);
            }
        }

        void Clear()
        {
            foreach (var c in capsules)
                DestroyImmediate(c.gameObject);
            capsules.Clear();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(center, size);
            Gizmos.DrawWireSphere(obstacle.transform.position, radius);
        }
    }
}