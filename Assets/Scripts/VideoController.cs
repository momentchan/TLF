using System;
using System.Collections.Generic;
using UnityEngine;

namespace TLF
{
    public class VideoController : MonoBehaviour
    {
        [SerializeField] private VideoType type;
        [SerializeField] private List<VideoPlayer> players;

        private void Start()
        {
            PlayVideo(type);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
                ChangeVideo();
        }

        public void ChangeVideo()
        {
            type = (VideoType)(((int)type + 1) % Enum.GetValues(typeof(VideoType)).Length);
            PlayVideo(type);
        }

        public void PlayVideo(VideoType type) => players.ForEach(p => p.Play(type));
    }
}