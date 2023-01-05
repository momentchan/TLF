using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLF
{
    public class Rabbit : MonoBehaviour
    {
        [SerializeField] private GameObject rabbit;

        public void ShowRabbit()
        {
            rabbit.SetActive(true);
        }

        public void HideRabbit()
        {
            rabbit.SetActive(false);
        }
    }
}