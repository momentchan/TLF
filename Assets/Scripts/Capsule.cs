using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    public Rigidbody Rigidbody => rigidbody;
    public CapsuleCollider Collider => collider;

    CapsuleCollider collider;
    Rigidbody rigidbody;
    MeshRenderer renderer;
    private float seed;
    private Vector3 initScale;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<MeshRenderer>();
        seed = Random.value;
        initScale = transform.localScale;
    }
    public void SetSize(Vector2 range)
    {
        transform.localScale = initScale * Mathf.Lerp(range.x, range.y, seed);
    }
    public void SetMaterial(Material mat)
    {
        renderer.material = mat;
    }
}
