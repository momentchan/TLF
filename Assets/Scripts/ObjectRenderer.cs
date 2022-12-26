using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace TLF
{
    public class ObjectRenderer : MonoBehaviour
    {
        private NativeArray<Matrix4x4> matrices;
        [SerializeField] private Material mat;
        [SerializeField] private CapsuleController generator;
        private MaterialPropertyBlock matProps;
        [SerializeField] private Mesh mesh;
        void Start()
        {
            matrices = new NativeArray<Matrix4x4>
                (512, Allocator.Persistent,
                NativeArrayOptions.UninitializedMemory);
            matProps = new MaterialPropertyBlock();

        }
        private void OnDestroy()
        {
            matrices.Dispose();
        }

        void Update()
        {
            var job = new AudienceAnimationJob()
            {
                matrices = matrices,
            };
            job.Schedule(generator.Trs).Complete();


            var rparams = new RenderParams(mat) { matProps = matProps, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On };
            //Graphics.DrawMeshInstanced(mesh, 0, mat, generator.matricess.ToArray(), generator.maxCount);
            Graphics.RenderMeshInstanced
                (rparams, mesh, 0, matrices, generator.maxCount, 0);
        }



    }
    [BurstCompile]
    public struct AudienceAnimationJob : IJobParallelForTransform
    {
        // output
        public NativeSlice<Matrix4x4> matrices;

        public void Execute(int i, TransformAccess t)
        {
            matrices[i] = t.localToWorldMatrix;
        }
    }
}