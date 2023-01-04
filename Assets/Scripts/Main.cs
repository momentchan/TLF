using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;

namespace TLF {
    [ExecuteInEditMode]
    public class Main : SingletonMonoBehaviour<Main>
    {
        [Header("Cameras")]
        [SerializeField] private CameraMode mode;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Camera simulateCamera;
         private CameraMode preMode;

        public PlayMode Mode => playMode;
        [SerializeField] private PlayMode playMode;

        [Header("Timeline")]
        [SerializeField] private OutputController outputController;

        public void StartInteractive()
        {
            playMode = PlayMode.Interactive;
        }

        public void StartStatic()
        {
            playMode = PlayMode.Static;
        }

        void Update()
        {
            SwitchCameraMode();
        }

        private void SwitchCameraMode() {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mode = (CameraMode)(((int)mode + 1) % 3);
            }

            switch (mode)
            {
                case CameraMode.Scene:
                    sceneCamera.enabled = true;
                    simulateCamera.enabled = false;
                    break;
                case CameraMode.Simulation:
                    sceneCamera.enabled = false;
                    simulateCamera.enabled = true;
                    break;
            }

            if (preMode != mode)
            {
                preMode = mode;
            }
        }


        public enum CameraMode
        {
            Scene, Simulation
        }
        public enum PlayMode
        {
            Static, Interactive
        }
    }
}