using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private Capsule prefab;
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size = Vector3.one;
    [SerializeField] private List<Capsule> capsules = new List<Capsule>();
    [SerializeField] private GameObject obstacle;
    [SerializeField] private float radius = 1;
    [SerializeField] private float spawnRate = 0.2f;
    [SerializeField] private int maxCount = 100;
    [SerializeField] private float drag = 2f;
    [SerializeField] private PhysicMaterial physicMat;
    [SerializeField] private Material renderMat;
    [SerializeField, Range(0, 1)] private float bounciness = 0.6f;
    [SerializeField] private float mass = 1f;
    [SerializeField] private float staticFriction = 0.6f;
    [SerializeField] private float dynamicFriction = 0.6f;

    [SerializeField] FoamTransformDataset foams;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            RandomGenerate();
        if (Input.GetKeyDown(KeyCode.C))
            Clear();

        foreach (var c in capsules)
        {
            c.Rigidbody.mass = mass;
            c.Rigidbody.drag = drag;
            c.SetMaterial(renderMat);
        }

        physicMat.bounciness = bounciness;
        physicMat.staticFriction = staticFriction;
        physicMat.dynamicFriction = dynamicFriction;
    }

    void RandomGenerate()
    {
        var pos = Vector3.zero;
        for (var i = 0; i < 20; i++)
        {
            pos = center + new Vector3(
                Mathf.Lerp(-0.5f, 0.5f, Random.value) * size.x,
                Mathf.Lerp(0.2f, 0.4f, Random.value) * size.y,
                Mathf.Lerp(-0.5f, 0.5f, Random.value) * size.z);

            var op = obstacle.transform.position;
            if (Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(op.x, op.z)) > radius)
                break;
        }

        var c = Instantiate(prefab, transform);
        c.transform.position = pos;
        c.transform.rotation = Random.rotation;
        capsules.Add(c);
    }

    [ContextMenu("Save")]
    void SaveTransform()
    {
        foams.SaveTransform(capsules.Select(c => c.transform).ToList());
    }

    [ContextMenu("Generate")]
    void Generate()
    {
        Clear();
        foreach(var d in foams.GetTransformData())
        {
            var c = Instantiate(prefab, transform);
            c.transform.localPosition = d.localPosition;
            c.transform.localRotation = d.localRotation;
            c.transform.localScale = d.localScale;
            capsules.Add(c);
        }
    }

    void Clear()
    {
        foreach (var c in capsules)
            DestroyImmediate(c.gameObject);
        capsules.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(center, size);
        Gizmos.DrawWireSphere(obstacle.transform.position, radius);
    }
}
