using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TLF
{
    public class ToyContainer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> toys;
        private int currentId = -1;

        void Start()
        {
            SwitchToy();
        }

        public void SwitchToy()
        {
            currentId = (currentId + 1) % toys.Count;
            for (var i = 0; i < toys.Count; i++)
                toys[i].SetActive(i == currentId);
        }
    }
}