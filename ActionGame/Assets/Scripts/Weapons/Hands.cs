using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : Weapon
{
    public float Damage = 25f;
    public GameObject AlienImpact;
    public GameObject AlienImpactHeadShot;
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
            Hit();
        }
    }

    private void Hit()
    {
        if (hit.collider.tag == "Alien")
        {
            EnemyAI ai = hit.transform.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.TakeDamage(Damage);
                Debug.Log("Body is hit");
                GameObject BloodImpact = Instantiate(AlienImpact, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(BloodImpact, 2f);
            }
        }
        else if (hit.collider.tag == "AlienHead")
        {
            EnemyAI ai = hit.transform.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.TakeDamage(100f);
                ai.HeadShot();
                Debug.Log("HeadShot");
                GameObject BloodImpact = Instantiate(AlienImpactHeadShot, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(BloodImpact, 2f);
            }
        }
    }
}
