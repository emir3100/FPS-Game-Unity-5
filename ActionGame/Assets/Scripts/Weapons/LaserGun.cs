using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LaserGun : Weapon
{
    [Header("SubClass Variables")]
    public int TotalAmmo = 120;
    public float Damage = 25f;
    private WaitForSeconds laserDuration = new WaitForSeconds(.07f);
    private LineRenderer laserLine;
    private PlayerMovement playerMovementScript;
    private Crosshair crosshairScript;
    public float UpRecoil;
    public float SideRecoil;
    private float originalSideRecoil;
    private float originalUpRecoil;

    public override void Start()
    {
        originalSideRecoil = SideRecoil;
        originalUpRecoil = UpRecoil;
        base.Start();
        laserLine = GetComponent<LineRenderer>();
        playerMovementScript = GameObject.FindGameObjectsWithTag("Player").First().GetComponent<PlayerMovement>();
        crosshairScript = GameObject.FindGameObjectsWithTag("Crosshair").First().GetComponent<Crosshair>();
    }

    public override void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFire && TotalAmmo > 0)
        {
            nextFire = Time.time + 1f / FireRate;
            Shoot();
        }
        else if (Input.GetButtonDown("Fire1") && TotalAmmo == 0)
        {
            FindObjectOfType<AudioManager>().Play("EmptySound");
        }

        if (Input.GetButton("Fire1") && TotalAmmo > 0)
            IsShooting = true;
        else
            IsShooting = false;

        crosshairScript.IsShooting(IsShooting);

        if (IsShooting)
            StartCoroutine("AddRecoilSpread");
        else
        {
            SideRecoil = originalSideRecoil;
            UpRecoil = originalUpRecoil;
        }
    }

    private void LateUpdate()
    {
        DrawLaser();
    }

    public override void Shoot()
    {
        TotalAmmo -= 1;
        Muzzle.Play();
        FindObjectOfType<AudioManager>().Play("ShotSound");
        StartCoroutine(ShotEffect());
        base.WeaponAnimationScript.ShootAnimation(ShootParam);
        playerMovementScript.SetRecoil(UpRecoil/4f, SideRecoil/4f);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Distance))
        {
            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            Vector3 endPoint = cam.transform.position + (cam.transform.forward.normalized * Distance);
            laserLine.SetPosition(1, endPoint);
        }
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

    private IEnumerator AddRecoilSpread()
    {
        yield return new WaitForSeconds(0.5f);
        SideRecoil += Random.Range(-2.5f, 2.5f) * Time.deltaTime;
        UpRecoil += Random.Range(-2.5f, 2.5f) * Time.deltaTime;
    }
}
