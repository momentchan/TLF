using System.Collections;
using System.Collections.Generic;
using System.Linq;
using mj.gist;
using PrefsGUI;
using PrefsGUI.RapidGUI;
using Unity.Mathematics;
using UnityEngine;

namespace TLF
{
    public class CapsuleController : SingletonMonoBehaviour<CapsuleController>, IGUIUser
    {
        [SerializeField] FoamTransformDataset foams;
        [SerializeField] private Capsule prefab;
        [SerializeField] private PhysicMaterial physicMat;
        [SerializeField] private bool generate;
        [SerializeField] private int length = 10;
        [SerializeField] private Bound bounds;

        [Header("Appearance")]
        [SerializeField] private Vector3 normalSize = Vector3.one;
        [SerializeField] private Vector3 specialSize = Vector3.one;
        [SerializeField] private Vector2 speedRange = new Vector2(0, 0.1f);
        [SerializeField] private Vector2 sizeRange = new Vector2(0.8f, 1f);

        private PrefsFloat drag = new PrefsFloat("Drag", 3f);
        private PrefsFloat angularDrag = new PrefsFloat("AngularDrag", 0.2f);
        private PrefsFloat staticFriction = new PrefsFloat("StaticFriction", 0.6f);
        private PrefsFloat dynamicFriction = new PrefsFloat("DynamicFriction", 0.6f);
        private PrefsFloat bounciness = new PrefsFloat("Bounciness", 0.6f);
        private PrefsFloat speedSmooth = new PrefsFloat("SpeedSmooth", 0.6f);

        public Bound Bounds => bounds;
        public float Drag => drag;
        public float AngularDrag => angularDrag;
        public float SpeedSmooth => speedSmooth;

        #region gui
        public string GetName() => "Capsules";

        public void ShowGUI()
        {
            drag.DoGUI();
            angularDrag.DoGUI();
            staticFriction.DoGUI();
            dynamicFriction.DoGUI();
            bounciness.DoGUISlider(0, 1);
            speedSmooth.DoGUISlider(0, 1);
        }
        #endregion

        [SerializeField] private List<Capsule> capsules = new List<Capsule>();

        
        public Vector3 Size(CapsuleKind kind) => kind == CapsuleKind.Normal ? normalSize : specialSize;
        public Vector3 GetScale(float seed, CapsuleKind kind) => 
            Size(kind) * Mathf.Lerp(sizeRange.x, sizeRange.y, seed);

        public float GetEmissionIntensiy(float speed) => math.remap(speedRange.x, speedRange.y, 0, 1, speed);

        protected override void Awake()
        {
            if (generate)
                CreateCapsules();
            capsules = GetComponentsInChildren<Capsule>().ToList();
        }

        public void Reset()
        {
            foreach (var c in capsules)
                c.Reset();
        }

        void Update()
        {
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
                        var pos = bounds.Center + new Vector3(
                            Mathf.Lerp(-0.5f, 0.5f, 1f * i / length) * bounds.Size.x,
                            Mathf.Lerp(-0.5f, 0.5f, 1f * j / length) * bounds.Size.y,
                            Mathf.Lerp(-0.5f, 0.5f, 1f * k / length) * bounds.Size.z);

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
            bounds.DrawGizmos();
        }

       

        public enum CapsuleKind { Normal, Special }
    }
}