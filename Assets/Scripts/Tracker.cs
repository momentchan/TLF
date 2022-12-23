using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public int playerIndex;
    [SerializeField] private GameObject objectPrefab;

    public List<GameObject> trackObjects = new List<GameObject>();
    public void SetUp(int userId, int objectNum)
    {
        this.playerIndex = userId;
        for (var i = 0; i < objectNum; i++)
        {
            var go = Instantiate(objectPrefab, transform);
            go.SetActive(false);
            trackObjects.Add(go);
        }
    }
}