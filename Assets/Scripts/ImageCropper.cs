using UnityEngine;

namespace TLF
{
    [ExecuteInEditMode]
    class ImageCropper : MonoBehaviour
    {
        [SerializeField] private RenderTexture input;
        [SerializeField] private RenderTexture output_L, output_R;
        [SerializeField] private ProjectHelper helper;
        [SerializeField] private Shader shader;

        private Material _mat;
        private Material mat
        {
            get
            {
                if (_mat == null)
                    _mat = new Material(shader);
                return _mat;
            }
        }

        private void Update()
        {
            mat.SetMatrix("_Homography", helper.GetHomography(QuadType.Left));
            Graphics.Blit(input, output_L, mat);

            mat.SetMatrix("_Homography", helper.GetHomography(QuadType.Right));
            Graphics.Blit(input, output_R, mat);
        }
    }
}