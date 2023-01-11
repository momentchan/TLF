using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]
    public class WalkArea : MonoBehaviour
    {
        private TrackerController controller => TrackerController.Instance;
        void Update()
        {
            transform.localScale = controller.WalkArea;
            transform.localRotation = Quaternion.Euler(0, controller.SensorAngle, 0);
        }
    }
}