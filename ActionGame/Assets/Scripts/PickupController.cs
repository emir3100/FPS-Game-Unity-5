using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public Weapon GunScript;
    public bool IsPickedUp;
    public float PickUpRange;
    public Transform Player;
    public Transform WeaponShowVersion;
    public Transform WeaponFPSVersion;
    public Transform WeaponHolder;
    public Vector3 CurrentPlayerPos;
    public Rigidbody Rigidbody;
    public BoxCollider Collider;
    public Transform Armmesh;

    public Vector3 WeaponFPSPosition = new Vector3(-0.22f, 0f, -0.3200007f);

    void Start()
    {
    }

    private void Update()
    {
        CurrentPlayerPos = Player.position;
        Vector3 distanceToPlayer = Player.position - transform.position;

        if (!IsPickedUp && distanceToPlayer.magnitude <= PickUpRange && Input.GetKeyDown(KeyCode.E))
            Pickup();

        if (Input.GetKeyDown(KeyCode.G) && IsPickedUp)
            Drop();
    }

    private void Pickup()
    {
        IsPickedUp = true;
        Armmesh.gameObject.SetActive(true);
        transform.SetParent(WeaponHolder);
        transform.localPosition = Vector3.zero;
        //transform.position = WeaponFPSPosition;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        
        Rigidbody.isKinematic = true;
        Collider.isTrigger = true;
        
        WeaponShowVersion.gameObject.SetActive(false);
        GunScript.enabled = true;
        WeaponFPSVersion.gameObject.SetActive(true);
    }

    private void Drop()
    {
        IsPickedUp = false;
        transform.SetParent(null);
        Rigidbody.isKinematic = false;
        Collider.isTrigger = false;
        Armmesh.gameObject.SetActive(false);
        WeaponShowVersion.gameObject.SetActive(true);
        GunScript.enabled = false;
        transform.position = CurrentPlayerPos;
        WeaponFPSVersion.gameObject.SetActive(false);
    }
}
