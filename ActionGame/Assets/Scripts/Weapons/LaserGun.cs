using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserGun : Weapon
{
    [Header("SubClass Variables")]
    public int TotalAmmo = 120;
    public float Damage = 25f;
    private WaitForSeconds laserDuration = new WaitForSeconds(.07f);
    private LineRenderer laserLine;
    public AudioClip EmptySound;


    public override void Start()
    {
        base.Start();
        laserLine = GetComponent<LineRenderer>();
    }

    public override void Update()
    {
        TotalAmmoText.text = TotalAmmo.ToString();

        if (Input.GetButton("Fire1") && Time.time >= base.nextFire && TotalAmmo > 0)
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
        AudioSource.PlayClipAtPoint(ShotSound, this.transform.position);
        StartCoroutine(ShotEffect());
        base.WeaponAnimationScript.ShootAnimation(ShootParam);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Distance))
        {
            laserLine.SetPosition(1, hit.point);
            Debug.Log(hit.collider.gameObject.name);
        }
        else
        {
            laserLine.SetPosition(1, cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)));
        }
    }

    private void LateUpdate()
    {
        DrawLaser();
    }

    private void DrawLaser()
    {
        if (!IsShooting) return;

        laserLine.SetPosition(0, GunEnd.position);
    }
    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return laserDuration;
        laserLine.enabled = false;
    }
}
