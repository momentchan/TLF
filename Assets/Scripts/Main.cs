using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;
using UnityEngine.Playables;

namespace TLF
{
    [ExecuteInEditMode]
    public class Main : SingletonMonoBehaviour<Main>
    {
        [Header("Cameras")]
        [SerializeField] private CameraMode mode;
        [SerializeField] private PlayMode playMode;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Camera simulateCamera;

        [Header("Timeline")]
        [SerializeField] private KeyCode timelineKey = KeyCode.P;
        [SerializeField] private PlayableAsset devTimeline;
        [SerializeField] private PlayableAsset prodTimeline;

        private PlayableDirector director;

        public PlayMode Mode => playMode;


        public void StartTimeLine()
        {
#if UNITY_EDITOR
            director.Play(devTimeline);
#else
            director.Play(prodTimeline);
#endif
        }

        public void StartInteractive()
        {
            playMode = PlayMode.Interactive;
        }

        public void StartStatic()
        {
            playMode = PlayMode.Static;
        }

        private void Start()
        {
            director = GetComponent<PlayableDirector>();
        }

        void Update()
        {
            SwitchCameraMode();

#if UNITY_EDITOR
            if (Input.GetKeyDown(timelineKey))
            {
                if (director.state == PlayState.Playing)
                    director.Pause();
                else if (director.state == PlayState.Paused)
                    director.Play();
            }
#endif
        }

        private void SwitchCameraMode()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mode = (CameraMode)(((int)mode + 1) % 2);
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