using System.Collections;
using mj.gist;
using PrefsGUI;
using PrefsGUI.RapidGUI;
using UnityEngine;

namespace TLF
{
    public class InteractiveEffect : SingletonMonoBehaviour<InteractiveEffect>, IGUIUser
    {
        [SerializeField] private CapsuleController capsuleController;
        public bool Enable => enable;
        private bool enable = true;

        public float GetTouchScaleMultiplier(float nrmT) => Mathf.Lerp(1, TouchedScaleMultiplier, nrmT);
        public Vector3 GetVelocityFactor(Vector3 velocity)
            => Mathf.Clamp01(Mathf.Pow(Mathf.Clamp01(velocity.magnitude / MaxSpeed), SpeedPower)) * velocity.normalized;
        public Vector3 GetForce(Vector3 dir, Vector3 vel)
            => Vector3.Lerp(dir, vel, VelocityBlend) * ForcePower;

        #region gui
        [HideInInspector] public PrefsFloat MaxSpeed = new PrefsFloat("MaxSpeed", 2f);
        [HideInInspector] public PrefsFloat SpeedPower = new PrefsFloat("SpeedPower", 200f);
        [HideInInspector] public PrefsFloat SpeedThreshold = new PrefsFloat("SpeedThreshold", 0.2f);

        [HideInInspector] public PrefsFloat ForcePower = new PrefsFloat("ForcePower", 100f);
        [HideInInspector] public PrefsFloat PulsePower = new PrefsFloat("PulsePower", 300f);
        [HideInInspector] public PrefsVector2 PulseRandom = new PrefsVector2("PulseRandom", new Vector2(0.6f, 1f));

        [HideInInspector] public PrefsFloat PulseWarmUpT = new PrefsFloat("PulseWarmUpT", 2f);
        [HideInInspector] public PrefsFloat PulseCoolDownT = new PrefsFloat("PulseCoolDownT", 3f);
        [HideInInspector] public PrefsInt PulseTriggerNum = new PrefsInt("PulseTriggerNum", 10);
        [HideInInspector] public PrefsFloat PulsePeriod = new PrefsFloat("PulsePeriod", 120f);

        [HideInInspector] public PrefsFloat VelocityBlend = new PrefsFloat("VelocityBlend", 0.95f);

        [HideInInspector] public PrefsFloat ColorBlend = new PrefsFloat("ColorBlend", 0.5f);
        [HideInInspector] public PrefsFloat InteractiveRange = new PrefsFloat("InteractiveRange", 1.25f);
        [HideInInspector] public PrefsFloat IdleTime = new PrefsFloat("IdleTIme", 2f);

        [HideInInspector] public PrefsFloat LifeTime = new PrefsFloat("Lifetime", 2f);
        [HideInInspector] public PrefsFloat TouchedThreshold = new PrefsFloat("touchedThreshold", 1f);
        [HideInInspector] public PrefsFloat TouchedScaleMultiplier = new PrefsFloat("TouchedScaleMultiplier", 3);

        [HideInInspector] public PrefsFloat ExplosionForce = new PrefsFloat("ExplosionForce", 200f);
        [HideInInspector] public PrefsFloat ExplosionRadius = new PrefsFloat("ExplosionRadius", 2f);
        [HideInInspector] public PrefsFloat ExplodeDuration = new PrefsFloat("ExplodeDuration", 0.5f);
        [HideInInspector] public PrefsFloat ExplodeEmissionIntensity = new PrefsFloat("ExplodeEmissionIntensity", 2f);
        [SerializeField] private AnimationCurve explodeScaleCurve;
        public AnimationCurve ExplodeScaleCurve => explodeScaleCurve;

        public string GetName() => "Interaction";
        public float duration = 2f;
        public void ShowGUI()
        {
            ForcePower.DoGUI();

            PulsePower.DoGUI();
            PulseRandom.DoGUI();
            PulseWarmUpT.DoGUI();
            PulseCoolDownT.DoGUI();
            PulseTriggerNum.DoGUI();
            PulsePeriod.DoGUI();

            VelocityBlend.DoGUISlider(0, 1f);
            MaxSpeed.DoGUI();
            SpeedPower.DoGUI();
            SpeedThreshold.DoGUI();

            ColorBlend.DoGUISlider(0, 1f);
            InteractiveRange.DoGUI();
            IdleTime.DoGUI();

            LifeTime.DoGUI();
            TouchedThreshold.DoGUI();
            TouchedScaleMultiplier.DoGUI();

            ExplosionForce.DoGUI();
            ExplosionRadius.DoGUI();
        }
        #endregion
        void Start()
        {
            StartCoroutine(PulseCoroutine());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
                StartPulseEffect();
        }

        private IEnumerator PulseCoroutine()
        {
            yield return null;
            while (true)
            {
                yield return null;
                if (Main.Instance.Mode == Main.PlayMode.Interactive &&
                    TrackerController.Instance.ActiveTrackerNum > PulseTriggerNum)
                {
                    StartPulseEffect();
                    yield return new WaitForSeconds(PulsePeriod);
                }
            }
        }

        public void StartPulseEffect()
        {
            StopAllCoroutines();
            StartCoroutine(PulseEffect());
        }

        private IEnumerator PulseEffect()
        {
            yield return null;
            enable = false;

            yield return new WaitForSeconds(PulseWarmUpT);

            capsuleController.AddPulseForce(PulsePower * Vector3.up, PulseRandom, Random.ColorHSV(0, 1, 0.5f, 1f, 1f, 1f));

            yield return new WaitForSeconds(PulseCoolDownT);
            enable = true;
        }
    }
}