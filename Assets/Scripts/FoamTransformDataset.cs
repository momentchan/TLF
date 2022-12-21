using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "FoamTransform", menuName = "FoamTransform")]
public class FoamTransformDataset : ScriptableObject
{
    [SerializeField] private List<FoamTransformData> data;
    public List<FoamTransformData> GetTransformData() => data;
    public void SaveTransform(List<Transform> trans)
    {
#if UNITY_EDITOR
        data.Clear();
        data =
            trans.Select(t => new FoamTransformData()
            {
                localPosition = t.localPosition,
                localRotation = t.localRotation,
                localScale = t.localScale
            }).ToList();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
#endif
    }

    [System.Serializable]
    public class FoamTransformData
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
    }
}