using System.Collections;
using mj.gist;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using static TLF.CapsuleController;

namespace TLF
{
    public class Capsule : MonoBehaviour
    {
        InteractiveEffect effect => InteractiveEffect.Instance;

        private CapsuleController controller => CapsuleController.Instance;
        [SerializeField] private CapsuleKind kind;
        [SerializeField] private float seed;

        new Rigidbody rigidbody;
        [SerializeField] private MeshRenderer renderer;

        private Block block;
        private float speed;
        private float life = 0;
        public float nrmT => life / InteractiveEffect.Instance.LIfeTime;

        private Color emissiveColor;
        private Vector3 initLocalPos;
        private quaternion initLocalRot;

        void Start()
        {
            initLocalPos = transform.localPosition;
            initLocalRot = transform.localRotation;

            rigidbody = GetComponent<Rigidbody>();
            renderer = GetComponent<MeshRenderer>();
            block = new Block(renderer);
            this.seed = UnityEngine.Random.value;

        }

        public void AddForce(Vector3 pos, Vector3 vel, Color color)
        {
            if (vel.magnitude > InteractiveEffect.Instance.TouchedThreshold)
                life += Time.deltaTime;

            var dir = transform.position - pos;
            AddForce(InteractiveEffect.Instance.GetForce(dir, vel), Vector2.one, color);
        }

        public void AddExplode(Vector3 pos)
        {
            rigidbody.AddExplosionForce(effect.explosionForce, pos, effect.explosionRadius);
        }

        public void AddForce(Vector3 force, Vector2 random, Color color, bool colorOverride = false)
        {
            if (exploded) return;

            force *= Mathf.Lerp(random.x, random.y, seed);
            rigidbody.AddForce(force);
            emissiveColor = colorOverride ? color : Color.Lerp(color, emissiveColor, InteractiveEffect.Instance.ColorBlend);
        }

        public void Reset()
        {
            transform.localPosition = initLocalPos;
            transform.localRotation = initLocalRot;
            emissiveColor = Color.black;
            life = 0;
            exploded = false;
        }

        private void Update()
        {
            if (!controller.Bounds.Contains(transform.position))
                Reset();

            rigidbody.drag = controller.Drag;
            rigidbody.angularDrag = controller.AngularDrag;
            speed = Mathf.Lerp(rigidbody.velocity.magnitude, speed, controller.SpeedSmooth);

            block.SetFloat("_LifeTime", nrmT);
            block.SetFloat("_Seed", seed);

            if (nrmT < 1)
            {
                transform.localScale = controller.GetScale(seed, nrmT, kind);

                block.SetColor("_EmissiveColor", emissiveColor);
                block.SetFloat("_EmissiveIntensity", controller.GetEmissionIntensiy(nrmT, speed));
            }
            else
            {
                if (!exploded)
                    StartCoroutine(Explode());
            }

            block.Apply();
        }
        private bool exploded = false;

        IEnumerator Explode()
        {
            exploded = true;

            yield return null;
            var scale = transform.localScale;
            var t1 = 0f;
            var duration = controller.ExplodeDuration;

            while (t1 < duration)
            {
                t1 += Time.deltaTime;
                var ratio = t1 / duration;
                transform.localScale = scale * controller.ExplodeScaleCurve.Evaluate(ratio);

                emissiveColor = Color.Lerp(emissiveColor, Color.red, ratio);

                block.SetColor("_EmissiveColor", emissiveColor);
                block.SetFloat("_EmissiveIntensity", ratio * controller.ExplodeStrength);
                yield return null;
            }
            controller.AddExplode(transform.position);
            Reset();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.Label(transform.position, nrmT.ToString());
        }
#endif
    }
}