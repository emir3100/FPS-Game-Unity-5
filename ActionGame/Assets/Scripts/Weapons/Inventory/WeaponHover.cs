using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHover : MonoBehaviour
{
    private Rigidbody rigidbody;
    public const int AmountPoints = 4;
    public Transform[] Spring = new Transform[AmountPoints];
    public RaycastHit[] hit = new RaycastHit[AmountPoints];
    public float HoverMultiplier = 2.5f;
    public float RotationSpeed = 100f;
    void Start()
    {
        rigidbody = this.transform.GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        for (int i = 0; i < AmountPoints; i++)
            ApplyForce(Spring[i], hit[i]);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0f , Time.deltaTime * RotationSpeed, 0f));
    }

    private void ApplyForce(Transform spring, RaycastHit hit)
    {
        if (Physics.Raycast(spring.position, spring.transform.TransformDirection(Vector3.down), out hit, 10f))
        {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - spring.transform.position.y));
            rigidbody.AddForceAtPosition(transform.up * force * HoverMultiplier, spring.transform.position, ForceMode.Acceleration);
        }
    }
}
