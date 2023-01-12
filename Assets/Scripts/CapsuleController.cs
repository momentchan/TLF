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
    public class CapsuleController : MonoBehaviour, IGUIUser
    {
        [SerializeField] FoamTransformDataset foams;
        [SerializeField] private Capsule prefab;
        [SerializeField] private PhysicMaterial physicMat;
        [SerializeField] private bool generate;
        [SerializeField] private int length = 10;
        [SerializeField] private Bound bounds;
        [SerializeField] private List<Capsule> capsules = new List<Capsule>();

        public Bound Bounds => bounds;
        public float Drag => drag;
        public float AngularDrag => angularDrag;
        public float SpeedSmooth => speedSmooth;
        public Vector3 GetScale(float seed, float lifetime) =>
            normalSize.Get() * Mathf.Lerp(sizeRange.Get().x, sizeRange.Get().y, seed)
                       * InteractiveEffect.Instance.GetTouchScaleMultiplier(lifetime);

        public float GetEmissionIntensiy(float lifeTime, float speed) 
            => math.remap(speedRange.Get().x, speedRange.Get().y, 0, 1, speed);

        #region gui
        private PrefsVector3 normalSize = new PrefsVector3("NormalSize", new Vector3(0.4f, 0.2f, 0.4f));
        private PrefsVector3 specialSize = new PrefsVector3("SpecialSize", new Vector3(1.5f, 1.5f, 1.5f));
        private PrefsVector2 speedRange = new PrefsVector2("SpeedRange", new Vector2(0f, 2f));
        private PrefsVector2 sizeRange = new PrefsVector2("SizeRange", new Vector2(0.8f, 1f));
        private PrefsFloat drag = new PrefsFloat("Drag", 3f);
        private PrefsFloat angularDrag = new PrefsFloat("AngularDrag", 0.2f);
        private PrefsFloat staticFriction = new PrefsFloat("StaticFriction", 0.6f);
        private PrefsFloat dynamicFriction = new PrefsFloat("DynamicFriction", 0.6f);
        private PrefsFloat bounciness = new PrefsFloat("Bounciness", 0.6f);
        private PrefsFloat speedSmooth = new PrefsFloat("SpeedSmooth", 0.6f);
        public string GetName() => "Capsules";

        public void ShowGUI()
        {
            normalSize.DoGUI();
            specialSize.DoGUI();
            speedRange.DoGUI();
            sizeRange.DoGUI();

            drag.DoGUI();
            angularDrag.DoGUI();
            staticFriction.DoGUI();
            dynamicFriction.DoGUI();
            bounciness.DoGUISlider(0, 1);
            speedSmooth.DoGUISlider(0, 1);
        }
        #endregion

        void Awake()
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

            if (Input.GetKeyDown(KeyCode.R))
                foreach (var c in capsules)
                    c.Reset();
        }

        public void AddPulseForce(Vector3 force, Vector2 random, Color color)
        {
            foreach (var c in capsules)
                c.AddForce(force, random, color, true);
        }

        public void AddExplode(Vector3 pos)
        {
            foreach (var c in capsules)
                c.AddExplode(pos);
        }
        #region generate
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
        #endregion

        private void OnDrawGizmos()
        {
            bounds.DrawGizmos();
        }
    }
}