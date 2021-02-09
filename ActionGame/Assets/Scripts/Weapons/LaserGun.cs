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
    public GameObject DefaultImpact;

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
            Hit();
        }
        else
        {
            Vector3 endPoint = cam.transform.position + (cam.transform.forward.normalized * Distance);
            laserLine.SetPosition(1, endPoint);
        }
    }

    private void Hit()
    {
        if (hit.collider.tag == "Enemy")
        {
            //AI ai = hit.transform.GetComponent<Ai>();
            //if (ai != null)
            //{
            //    ai.TakeDamage(Damage);
            //    Debug.Log("Body is hit");
            //    GameObject BI = Instantiate(BloodImpact, hit.point, Quaternion.LookRotation(hit.normal));
            //    Destroy(BI, 2f);
            //    GameObject BS = Instantiate(BloodSplatter, hit.point, Quaternion.LookRotation(hit.normal * -1f));
            //    Destroy(BS, 10f);
            //}
        }
        else if (hit.collider.tag == "Head")
        {
            //Ai ai = hit.transform.GetComponentInParent<Ai>();
            //ai.HeadShot();
            //Debug.Log("HeadShot");
            //GameObject HSI = Instantiate(HeadShotImpact, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(HSI, 2f);
        }
        else if (hit.collider.tag == "Untagged" || hit.collider.tag == "FlyRamp" || hit.collider.tag == "RunnableWall")
        {
            GameObject defaultImpact = Instantiate(DefaultImpact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(defaultImpact, 10f);
        }
        else if (hit.collider.tag == "MoveableObject")
        {
            //hit.rigidbody.AddForce(-hit.normal * ForceImpact);
            //GameObject DI = Instantiate(DefaultImpact, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(DI, 10f);
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
