using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public string Name;
    public int TotalAmmo = 120;
    public float FireRate = 15f;
    public float Damage = 25f;
    public float Distance = 100f;
    public float Force = 100f;
    public Transform GunEnd;
    public ParticleSystem Muzzle;
    private Camera cam;
    public bool IsShooting;

    private WaitForSeconds laserDuration = new WaitForSeconds(.07f);
    private LineRenderer laserLine;
    public AudioClip ShotSound;
    public AudioClip EmptySound;
    private float nextFire = 0f;

    public Text TotalAmmoText;
    private RaycastHit hit;
    private WeaponAnimation WeaponAnimationScript;
    public PlayerMovement PlayerMovementScript;

    [Header("Weapon Animator")]
    public string ShootParam;
    public string MovingParam;

    private void Start()
    {
        cam = GetComponentInParent<Camera>();
        laserLine = GetComponent<LineRenderer>();
        WeaponAnimationScript = this.GetComponent<WeaponAnimation>();
    }

    private void Update()
    {
        TotalAmmoText.text = TotalAmmo.ToString();

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

    private void LateUpdate()
    {
        DrawLaser();
    }

    private void DrawLaser()
    {
        if (!IsShooting) return;
     
        laserLine.SetPosition(0, GunEnd.position);
    }

    private void Shoot()
    {
        TotalAmmo -= 1;
        Muzzle.Play();
        AudioSource.PlayClipAtPoint(ShotSound, this.transform.position);
        StartCoroutine(ShotEffect());
        WeaponAnimationScript.ShootAnimation(ShootParam);
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

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return laserDuration;
        laserLine.enabled = false;
    }
}
