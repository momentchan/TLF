using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceTest : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material mat;
    Matrix4x4[] matrices;
    [SerializeField] private int count = 100;
    [SerializeField] Bounds bounds;
    [SerializeField] private float size = 1f;
    void Start()
    {
        matrices = new Matrix4x4[count];
        for (var i = 0; i < count; i++)
        {
            var mat = new Matrix4x4();
            mat.SetTRS(bounds.center + new Vector3(bounds.extents.x * Random.Range(-1f, 1f),
                                                   bounds.extents.y * Random.Range(-1f, 1f),
                                                   bounds.extents.z * Random.Range(-1f, 1f)),
                        Quaternion.identity,
                        Vector3.one * size);
            matrices[i] = mat;
        }

    }


    void Update()
    {
        Graphics.DrawMeshInstanced(mesh, 0, mat, matrices);

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
