using System.Collections;
using mj.gist;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace TLF
{
    public class Capsule : MonoBehaviour
    {
        [SerializeField] private float seed;
        [SerializeField] private MeshRenderer renderer;

        public float nrmT => life / InteractiveEffect.Instance.LifeTime;
        private CapsuleController controller
        {
            get
            {
                if (_controller == null)
                    _controller = GetComponentInParent<CapsuleController>();
                return _controller;
            }
        }
        private CapsuleController _controller;
        
        private InteractiveEffect effect => InteractiveEffect.Instance;

        new Rigidbody rigidbody;

        private Block block;
        private float speed;
        private float life = 0;

        private Color emissiveColor;
        private Vector3 initLocalPos;
        private quaternion initLocalRot;
        private bool exploded = false;

        void Start()
        {
            initLocalPos = transform.localPosition;
            initLocalRot = transform.localRotation;

            rigidbody = GetComponent<Rigidbody>();
            renderer = GetComponent<MeshRenderer>();
            block = new Block(renderer);
            seed = UnityEngine.Random.value;
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
            rigidbody.AddExplosionForce(effect.ExplosionForce, pos, effect.ExplosionRadius);
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
                transform.localScale = controller.GetScale(seed, nrmT);

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

        private IEnumerator Explode()
        {
            exploded = true;

            yield return null;
            var scale = transform.localScale;
            var t = 0f;
            var duration = effect.ExplodeDuration;

            while (t < duration)
            {
                t += Time.deltaTime;
                var ratio = t / duration;
                transform.localScale = scale * effect.ExplodeScaleCurve.Evaluate(ratio);

                emissiveColor = Color.Lerp(emissiveColor, Color.red, ratio);

                block.SetColor("_EmissiveColor", emissiveColor);
                block.SetFloat("_EmissiveIntensity", ratio * effect.ExplodeEmissionIntensity);
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