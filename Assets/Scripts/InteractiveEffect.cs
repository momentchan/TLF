using System.Collections;
using System.Collections.Generic;
using mj.gist;
using PrefsGUI;
using PrefsGUI.RapidGUI;
using UnityEngine;

namespace TLF
{
    public class InteractiveEffect : SingletonMonoBehaviour<InteractiveEffect>, IGUIUser
    {
        private bool enable = true;
        public bool Enable => enable;

        public float ColorBlend => colorBlend;
        public float InteractiveRange => interactiveRange;
        public float IdleTime => idleTime;

        #region gui
        private PrefsFloat forcePower = new PrefsFloat("ForcePower", 100f);
        private PrefsFloat pulsePower = new PrefsFloat("PulsePower", 300f);
        private PrefsVector2 pulseRandom = new PrefsVector2("PulseRandom", new Vector2(0.6f, 1f));
        private PrefsFloat pulseWarmUpT = new PrefsFloat("PulseWarmUpT", 2f);
        private PrefsFloat pulseCoolDownT = new PrefsFloat("PulseCoolDownT", 3f);

        private PrefsFloat velocityBlend = new PrefsFloat("VelocityBlend", 0.95f);

        private PrefsFloat maxSpeed = new PrefsFloat("MaxSpeed", 2f);
        private PrefsFloat speedPower = new PrefsFloat("SpeedPower", 200f);

        private PrefsFloat colorBlend = new PrefsFloat("ColorBlend", 0.5f);
        private PrefsFloat interactiveRange = new PrefsFloat("InteractiveRange", 1.25f);
        private PrefsFloat idleTime = new PrefsFloat("IdleTIme", 2f);

        public string GetName() => "Interaction";
        public float duration = 2f;
        public void ShowGUI()
        {
            forcePower.DoGUI();

            pulsePower.DoGUI();
            pulseRandom.DoGUI();
            pulseWarmUpT.DoGUI();
            pulseCoolDownT.DoGUI();

            velocityBlend.DoGUISlider(0, 1f);
            maxSpeed.DoGUI();
            speedPower.DoGUI();
            colorBlend.DoGUISlider(0, 1f);
            interactiveRange.DoGUI();
            idleTime.DoGUI();
        }
        #endregion

        public Vector3 GetVelocityFactor(Vector3 velocity)
            => Mathf.Clamp01(Mathf.Pow(Mathf.Clamp01(velocity.magnitude / maxSpeed), speedPower)) * velocity.normalized;

        public Vector3 GetForce(Vector3 dir, Vector3 vel)
            => Vector3.Lerp(dir, vel, velocityBlend) * forcePower;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                StartCoroutine(PulseEffect());
            }
        }

        private IEnumerator PulseEffect()
        {
            yield return null;
            enable = false;

            yield return new WaitForSeconds(pulseWarmUpT);
            CapsuleController.Instance.AddPulseForce(pulsePower * Vector3.up, pulseRandom, Random.ColorHSV(0, 1, 0.5f, 1f, 1f, 1f));

            yield return new WaitForSeconds(pulseCoolDownT);
            enable = true;
        }
    }
}