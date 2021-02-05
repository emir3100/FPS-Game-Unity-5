using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RocketLauncher : Weapon
{
    //[Header("SubClass Variables")]
    public Rigidbody MissilePrefab;
    public int TotalAmmo = 120;
    public AudioClip EmptySound;
    public Transform Player;

    public override void Start()
    {
        base.Start();
        if (Player == null)
            Player = GameObject.FindGameObjectsWithTag("Player").First().transform;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFire && TotalAmmo > 0)
        {
            nextFire = Time.time + 1f / FireRate;
            IsShooting = true;
            Shoot();
        }
        else if (Input.GetButtonDown("Fire1") && TotalAmmo == 0)
        {
            AudioSource.PlayClipAtPoint(EmptySound, this.transform.position);
            IsShooting = false;
        }
        else
        {
            IsShooting = false;
        }
    }

    public override void Shoot()
    {
        TotalAmmo -= 1;
        Muzzle.Play();
        FindObjectOfType<AudioManager>().Play("RocketShoot");
        base.WeaponAnimationScript.ShootAnimation(ShootParam);

        Rigidbody clone;
        clone = Instantiate(MissilePrefab, GunEnd.transform.position, Player.rotation);
        clone.velocity = transform.TransformDirection(Vector3.forward * Distance);
    }
}
