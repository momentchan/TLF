using System;
using UnityEngine;

[ExecuteInEditMode]
public class SimulateQuad : MonoBehaviour
{
    [SerializeField] private QuadType type;
    private Material _material;

    public Material material
    {
        get
        {
            if (_material)
                _material = GetComponent<MeshRenderer>().sharedMaterial;
            return _material;
        }
    } 

    public enum QuadType
    {
        Left, Right
    }

    internal void SetCoordinates(Vector2 vector2, Vector2 vector21, Vector2 vector22, Vector2 vector23)
    {
    }
}
