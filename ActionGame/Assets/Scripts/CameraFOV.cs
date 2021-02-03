using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{
    private Camera Camera;
    private float TargetFov;
    private float Fov;
    private void Awake()
    {
        Camera = GetComponent<Camera>();
        TargetFov = Camera.fieldOfView;
        Fov = TargetFov;
    }

    private void Update()
    {
        float fovSpeed = 4f;
        Fov = Mathf.Lerp(Fov, TargetFov, Time.deltaTime * fovSpeed);
        Camera.fieldOfView = Fov;
    }

    public void SetCameraFov(float targetFov)
    {
        this.TargetFov = targetFov;
    }
}
