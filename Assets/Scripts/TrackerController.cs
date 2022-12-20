using System.Collections;
using System.Collections.Generic;
using BezierTools;
using UnityEngine;

public class TrackerController : MonoBehaviour
{
    [SerializeField] Bezier bezier;
    [SerializeField, Range(0,1)] private float rate = 0;

    void Update()
    {
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bezier.data.Position(rate), 0.5f);
    }
}
