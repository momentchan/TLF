using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLF
{
    public class RabbitGod : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float moveStrength = 1f;
        [SerializeField] private float rotSpeed = 1f;
        [SerializeField] private float rotAngle = 10f;

        private Vector3 initLocalPos;
        private Quaternion initLocalRot;

        void Start()
        {
            initLocalPos = transform.localPosition;
            initLocalRot = transform.localRotation;
        }

        void Update()
        {
            transform.localPosition = initLocalPos + Vector3.Lerp(Vector3.up, Vector3.down, Mathf.PerlinNoise(Time.time * moveSpeed, 0)) * moveStrength;
            transform.localRotation = initLocalRot * Quaternion.Euler(0, Mathf.Lerp(-rotAngle, rotAngle, Mathf.PerlinNoise(Time.time * rotSpeed, 0.5f)), 0);
        }
    }
}