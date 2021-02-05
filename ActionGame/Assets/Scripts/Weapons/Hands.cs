using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : Weapon
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + 1f / FireRate;
            IsShooting = true;
            Shoot();
        }
    }

    public override void Shoot()
    {
        //Muzzle.Play();
        FindObjectOfType<AudioManager>().Play("Punch");
        base.WeaponAnimationScript.ShootAnimation(ShootParam);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Distance))
        {
            Debug.Log(hit.collider.gameObject.name);
            Vector3 direction = hit.point;
        }
    }
}
