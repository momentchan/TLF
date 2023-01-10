using System;
using System.Collections.Generic;
using UnityEngine;

namespace TLF
{
    public class VideoController : MonoBehaviour
    {
        [SerializeField] private VideoType currentVideo;
        [SerializeField] private bool countDown;
        [SerializeField] private List<VideoPlayer> players;

        public void StartCountDown() => countDown = true;
        private int countDownIndex = 0;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
                ChangeVideo();
        }

        public void ChangeVideo()
        {
            if (countDown)
            {
                PlayVideo((VideoType)countDownIndex);
                countDownIndex = (countDownIndex + 1) % 3;
            }
            else
            {
                PlayVideo(VideoType.None);
            }
        }

        public void PlayVideo(VideoType type)
        {
            currentVideo = type;
            players.ForEach(p => p.Play(type));
        }
    }
}