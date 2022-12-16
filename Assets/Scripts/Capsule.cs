using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    public Rigidbody Rigidbody => rigidbody;
    public CapsuleCollider Collider => collider;

    CapsuleCollider collider;
    Rigidbody rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }
}
