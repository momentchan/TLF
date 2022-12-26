using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;

[ExecuteInEditMode]
public class Main : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private Camera simulateCamera;
    [SerializeField] private Camera outputCamera;
    [SerializeField] private CameraMode mode;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mode = (CameraMode)((int)mode + 1 % 3);
        }

        switch (mode)
        {
            case CameraMode.Scene:
                sceneCamera.enabled = true;
                simulateCamera.enabled = false;
                outputCamera.enabled = false;
                break;
            case CameraMode.Simulation:
                sceneCamera.enabled = false;
                simulateCamera.enabled = true;
                outputCamera.enabled = false;
                break;
            case CameraMode.Output:
                sceneCamera.enabled = false;
                simulateCamera.enabled = false;
                outputCamera.enabled = true;
                Screen.SetResolution(4096, 1792, false);
                break;
        }

#if UNITY_EDITOR
        GameViewUtility.TrySetSize(mode == CameraMode.Output ? "TLF Out" : "TLF Scene");
#endif
    }


    public enum CameraMode
    {
        Scene, Simulation, Output
    }
}
