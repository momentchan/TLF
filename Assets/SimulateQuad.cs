using System;
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

    public void SetCoordinates(Vector2 p00, Vector2 p01, Vector2 p10, Vector2 p11)
    {
        block.SetVector("_P00", p00);
        block.SetVector("_P01", p01);
        block.SetVector("_P10", p10);
        block.SetVector("_P11", p11);
        block.Apply();
    }
}
