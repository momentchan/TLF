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
        private PrefsFloat forcePower = new PrefsFloat("ForcePower", 100f);
        private PrefsFloat velocityBlend = new PrefsFloat("VelocityBlend", 0.95f);

        private PrefsFloat maxSpeed = new PrefsFloat("MaxSpeed", 2f);
        private PrefsFloat speedPower = new PrefsFloat("SpeedPower", 200f);

        private PrefsFloat colorBlend = new PrefsFloat("ColorBlend", 0.5f);
        private PrefsFloat interactiveRange = new PrefsFloat("InteractiveRange", 1.25f);

        public float ColorBlend => colorBlend;
        public float InteractiveRange => interactiveRange;

        #region GUI
        public string GetName() => "Interaction";

        public void ShowGUI()
        {
            forcePower.DoGUI();
            velocityBlend.DoGUISlider(0, 1f);
            maxSpeed.DoGUI();
            speedPower.DoGUI();
            colorBlend.DoGUISlider(0, 1f);
            interactiveRange.DoGUI();
        }
        #endregion

        public Vector3 GetVelocityFactor(Vector3 velocity)
            => Mathf.Clamp01(Mathf.Pow(Mathf.Clamp01(velocity.magnitude / maxSpeed), speedPower)) * velocity.normalized;

        public Vector3 GetForce(Vector3 dir, Vector3 vel) 
            => Vector3.Lerp(dir, vel, velocityBlend) * forcePower;
    }
}