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
    private Vector3 initScale;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<MeshRenderer>();
        initScale = transform.localScale;
    }
    public void SetMaterial(Material mat)
    {
        renderer.material = mat;
    }
}
