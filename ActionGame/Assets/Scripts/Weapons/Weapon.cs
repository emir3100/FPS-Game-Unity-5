using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public int totalAmmo = 120;
    public float fireRate = 15f;
    public float damage = 25f;
    public float distance = 100f;
    public float force = 100f;
    public Transform gunEnd;
    public ParticleSystem muzzle;
    private Camera cam;
    public bool isShooting;

    private WaitForSeconds laserDuration = new WaitForSeconds(.07f);
    private LineRenderer laserLine;
    public AudioClip shotSound;
    public AudioClip emptySound;
    private float nextFire = 0f;

    public Text totalAmmoText;
    private RaycastHit hit;

    private void Start()
    {
        cam = GetComponentInParent<Camera>();
        laserLine = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        totalAmmoText.text = totalAmmo.ToString();

        if (Input.GetButton("Fire1") && Time.time >= nextFire && totalAmmo > 0)
        {
            nextFire = Time.time + 1f / fireRate;
            isShooting = true;
            Shoot();
        }
        else if (Input.GetButtonDown("Fire1") && totalAmmo == 0)
        {
            AudioSource.PlayClipAtPoint(emptySound, this.transform.position);
            isShooting = false;
        }
        else
        {
            isShooting = false;
        }
    }

    private void LateUpdate()
    {
        DrawLaser();
    }

    private void DrawLaser()
    {
        if (!isShooting) return;
     
        laserLine.SetPosition(0, gunEnd.position);
    }

    private void Shoot()
    {
        totalAmmo -= 1;
        muzzle.Play();
        AudioSource.PlayClipAtPoint(shotSound, this.transform.position);
        StartCoroutine(ShotEffect());

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance))
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
