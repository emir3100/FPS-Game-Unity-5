using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLegs : MonoBehaviour
{
    public Transform Legs;
    public Transform Camera;
    public Transform Player;
    private RaycastHit raycast;
    public float Distance;
    [HideInInspector]
    public float LegsY = 1.5f;
    private void Update()
    {
        Vector3 CameraAngle = new Vector3(0, Camera.eulerAngles.y, 0);

        Legs.transform.eulerAngles = CameraAngle;
        Physics.Raycast(this.transform.position, -this.transform.forward, out raycast, Distance);
        Vector3 legOrientation = this.transform.position + (-this.transform.forward.normalized * Distance);
        Vector3 position = new Vector3(legOrientation.x, Player.position.y-LegsY, legOrientation.z);
        Legs.transform.position = position;
        
        Debug.DrawRay(this.transform.position, -this.transform.forward, Color.green);
    }
}
