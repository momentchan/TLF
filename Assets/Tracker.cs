using System.Collections;
using System.Collections.Generic;
using mj.gist.tracking.bodyPix;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] BodyPoseProvider provider;

    void Start()
    {
        
    }

    void Update()
    {
        var kp = provider.GetKeyPoint(KeypointID.LeftWrist);
        Debug.Log($"{kp.Score} {kp.Position}");
        
    }
}
