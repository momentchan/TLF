using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Main : MonoBehaviour
{
    [SerializeField] private bool showSimulate;
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private Camera simulateCamera;

    void Update()
    {
        sceneCamera.gameObject.SetActive(!showSimulate);
        simulateCamera.gameObject.SetActive(showSimulate);
    }
}
