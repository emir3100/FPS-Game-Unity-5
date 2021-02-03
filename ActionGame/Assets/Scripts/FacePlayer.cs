using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (camera != null)
        {
            Vector3 direction = this.transform.position - camera.transform.position;
            this.transform.rotation = Quaternion.LookRotation(direction, camera.transform.rotation * Vector3.up);
        }
    }
}
