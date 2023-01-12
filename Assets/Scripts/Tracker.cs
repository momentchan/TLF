using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLF
{
    public class Tracker : MonoBehaviour
    {
        [SerializeField] private TrackerObject objectPrefab;
        public int playerIndex;
        public List<TrackerObject> trackObjects = new List<TrackerObject>();

        public bool IsActive => idleT < InteractiveEffect.Instance.IdleTime;
        private float idleT = 0;
        public void Setup(int userId)
        {
            this.playerIndex = userId;
        }

        private void Start()
        {
            for (var i = 0; i < TrackerController.Instance.TRACK_OBJECT_NUM; i++)
            {
                var o = Instantiate(objectPrefab, transform);
                trackObjects.Add(o);
            }
        }

        private void Update()
        {
            idleT += Time.deltaTime;
        }

        public void Activate()
        {
            idleT = 0;
        }
    }
}