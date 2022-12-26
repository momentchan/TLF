using mj.gist;
using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]
    public class SimulateQuad : MonoBehaviour
    {
        [SerializeField] private QuadType type;
        [SerializeField] private ProjectHelper helper;

        private Block _block;
        public Block block
        {
            get
            {
                if (_block == null)
                    _block = new Block(GetComponent<MeshRenderer>());
                return _block;
            }
        }
        private void Update()
        {
            block.SetMatrix("_Homography", helper.GetHomography(type));
            block.Apply();
        }
    }
    public enum QuadType
    {
        Left, Right
    }
}