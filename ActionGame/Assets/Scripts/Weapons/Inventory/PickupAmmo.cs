using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : PickupWeapon
{
    public int IDWeapon;
    public int Amount;
    public override void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

        if (WeaponManagerScript == null)
            WeaponManagerScript = GameObject.FindWithTag("WeaponManager").GetComponent<WeaponManager>();

        SphereCollider = this.gameObject.GetComponent<SphereCollider>();
    }

    public override void Update()
    {
        SphereCollider.radius = PickUpRange;

        if (IsInTrigger && Input.GetKeyDown(KeyCode.F))
            WeaponManagerScript.AddAmmo(IDWeapon, Amount, this.gameObject);
    }
}
