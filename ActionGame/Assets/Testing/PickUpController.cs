using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Weapon GunScript;
    public Rigidbody Rigidbody;
    public BoxCollider Collider;
    public Transform Player, GunContainer, FpsCam;

    public float PickUpRange;
    public float DropForwardForce, DropUpwardForce;

    public bool Equipped;
    public static bool SlotFull;

    private void Start()
    {
        if (!Equipped)
        {
            GunScript.enabled = false;
            Rigidbody.isKinematic = false;
            Collider.isTrigger = false;
        }
        if (Equipped)
        {
            GunScript.enabled = true;
            Rigidbody.isKinematic = true;
            Collider.isTrigger = true;
            SlotFull = true;
        }
    }

    private void Update()
    {
        Vector3 distanceToPlayer = Player.position - transform.position;
        if (!Equipped && distanceToPlayer.magnitude <= PickUpRange && Input.GetKeyDown(KeyCode.E) && !SlotFull) PickUp();

        if (Equipped && Input.GetKeyDown(KeyCode.Q)) Drop();
    }

    private void PickUp()
    {
        Equipped = true;
        SlotFull = true;

        transform.SetParent(GunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        Rigidbody.isKinematic = true;
        Collider.isTrigger = true;

        GunScript.enabled = true;
    }

    private void Drop()
    {
        Equipped = false;
        SlotFull = false;

        transform.SetParent(null);

        Rigidbody.isKinematic = false;
        Collider.isTrigger = false;

        Rigidbody.velocity = Player.GetComponent<Rigidbody>().velocity;

        Rigidbody.AddForce(FpsCam.forward * DropForwardForce, ForceMode.Impulse);
        Rigidbody.AddForce(FpsCam.up * DropUpwardForce, ForceMode.Impulse);
        float random = Random.Range(-1f, 1f);
        Rigidbody.AddTorque(new Vector3(random, random, random) * 10);

        GunScript.enabled = false;
    }
}
