using System.Collections;
using System.Collections.Generic;
using BezierTools;
using mj.gist.tracking.bodyPix;
using UnityEngine;

public class TrackerController : MonoBehaviour
{
    [SerializeField] Bezier bezier;
    [SerializeField, Range(0,1)] private float rate = 0;
    [SerializeField] BodyPoseProvider provider;

    void Update()
    {

        var kp = provider.GetKeyPoint(KeypointID.LeftWrist);
        Debug.Log($"{kp.Score} {kp.Position}");
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bezier.data.Position(rate), 0.5f);
    }
}
