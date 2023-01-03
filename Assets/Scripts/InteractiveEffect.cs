using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;

namespace TLF
{
    public class InteractiveEffect : SingletonMonoBehaviour<InteractiveEffect>
    {
        [SerializeField] private float forcePower = 100;
        [SerializeField, Range(0, 1)] private float velocityBlend = 0.5f;

        [Header("Speed")]
        [SerializeField] private float maxSpeed = 2;
        [SerializeField] private float speedPower = 1;

        [Header("Color")]
        [SerializeField] public float colorBlendRate = 0.5f;

        public Vector3 GetVelocityFactor(Vector3 velocity) 
            => Mathf.Pow(Mathf.Clamp01(velocity.magnitude / maxSpeed), speedPower) * velocity.normalized;
        
        public Vector3 GetForce(Vector3 dir, Vector3 vel)
        {
            return Vector3.Lerp(dir, vel, velocityBlend) * forcePower;
        }
    }
}