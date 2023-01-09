using System.Collections.Generic;
using System.Linq;
using Klak.Hap;
using UnityEngine;

namespace TLF
{
    public class VideoPlayer : MonoBehaviour
    {
        public RenderTexture target;
        public List<VideoData> data;
        private HapPlayer player;

        public void Play(VideoType type)
        {
            if (player != null)
                Destroy(player);
            player = gameObject.AddComponent<HapPlayer>();
            player.Open(data.FirstOrDefault(d => d.type == type).filePath, HapPlayer.PathMode.LocalFileSystem);
            player.targetTexture = target;
            player.loop = false;
        }

        [System.Serializable]
        public class VideoData
        {
            public VideoType type;
            public string filePath;
        }

    }
    public enum VideoType { Ten, Twenty, Thirty, None }
}