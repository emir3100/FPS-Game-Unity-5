using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpGun : Weapon
{
    [Header("SubClass Variables")]
    public Rigidbody PlayerRigidbody;

    public override void Start()
    {
        base.Start();
        TotalAmmoText.text = "∞";
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
        Muzzle.Play();
        FindObjectOfType<AudioManager>().Play("JumpGun");
        base.WeaponAnimationScript.ShootAnimation(ShootParam);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Distance))
        {
            Debug.Log(hit.collider.gameObject.name);
            Vector3 direction = hit.point;
            AddForce(-direction);
        }
    }

    private void AddForce(Vector3 direction)
    {
        PlayerRigidbody.AddForce(direction * Force, ForceMode.Acceleration);
    }
}
