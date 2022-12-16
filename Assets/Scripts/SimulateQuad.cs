using mj.gist;
using UnityEngine;

[ExecuteInEditMode]
public class SimulateQuad : MonoBehaviour
{
    [SerializeField] private QuadType type;
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

    public enum QuadType
    {
        Left, Right
    }
    
    public void SetCoordinates(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        block.SetMatrix("_Homography", mj.gist.Transform.ComputeHomography(p0, p1, p2, p3));
        block.Apply();
    }
}
