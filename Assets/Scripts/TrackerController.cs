using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BezierTools;
using mj.gist.tracking;
using UnityEngine;

public class TrackerController : MonoBehaviour
{
    [SerializeField] Bezier bezier;
    [SerializeField, Range(0, 1)] private float rate = 0;
    [SerializeField] private int maxTrackers = 10;
    [SerializeField] private Tracker trackerPrefab;
    [SerializeField] private UserMatcher matcher;
    [SerializeField] private Vector2 interactiveRange = new Vector2(0.5f, 0.8f);
    [SerializeField] private Vector2 heightRange;
    [SerializeField] private List<Tracker> trackers = new List<Tracker>();
    [SerializeField] private float objectScale = 1f;

    private void Start()
    {
        for (var i = 0; i < maxTrackers; i++)
        {
            var tracker = Instantiate(trackerPrefab, transform);
            tracker.SetUp(i, matcher.avatarModel.JointsNumer);
            trackers.Add(tracker);
        }
    }

    void Update()
    {
        for (var i = 0; i < maxTrackers; i++)
        {
            var avatar = matcher.GetUser(i);
            var tracker = trackers[i];
            tracker.gameObject.SetActive(avatar != null);
            if (avatar != null)
            {
                for (var j = 0; j < avatar.Joints.Count; j++)
                {
                    var joint = avatar.Joints[j];
                    var trackObject = tracker.trackObjects[j];
                    var viewPos = joint.ViewPos;
                    if (joint.IsActive && interactiveRange.IsInside(viewPos.y))
                    {
                        var pos = bezier.data.Position(joint.ViewPos.x);
                        pos.y += Mathf.Lerp(heightRange.x, heightRange.y, interactiveRange.Interpolate(viewPos.y));
                        trackObject.transform.position = pos;
                        trackObject.transform.localScale = Vector3.one * objectScale;
                        trackObject.SetActive(true);
                    }
                    else
                    {
                        trackObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        var pos = bezier.data.Position(rate);
        Gizmos.DrawWireSphere(pos, 0.5f);
        Gizmos.DrawLine(pos + Vector3.up * heightRange.x, pos + Vector3.up * heightRange.y);
    }
}
