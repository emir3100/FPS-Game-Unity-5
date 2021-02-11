using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyWeapon : Weapon
{
    public float Damage = 25f;
    [Range(0f,1f)]
    public float Accuracy;
    private float change;
    public GameObject DefaultImpact;

    private WaitForSeconds laserDuration = new WaitForSeconds(.07f);
    private LineRenderer laserLine;
    private GameObject player;
    public Vector3 target;

    public override void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player").First();
        laserLine = GetComponent<LineRenderer>();
    }

    public override void Update()
    {
        change = Random.Range(Accuracy - 1f, Accuracy + 1f);
        target = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }

    public override void Shoot()
    {
        var rigbone = transform.Find("Armature").Find("mixamorig1:Hips").transform;
        Muzzle.Play();
        FindObjectOfType<AudioManager>().Play("ShotSound");
        StartCoroutine(ShotEffect());
        Physics.Raycast(new Vector3(rigbone.position.x-3f, rigbone.position.y+3f, rigbone.position.z+3f), target, out hit, Distance);
        laserLine.SetPosition(0, GunEnd.position);
        laserLine.SetPosition(1, target);

        Hit();
    }

    private void FirerateShoot()
    {
        if(Time.time >= nextFire)
        {
            nextFire = Time.time + 1f / FireRate;
            Shoot();
        }
    }

    public IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        FirerateShoot();
    }

    private void Hit()
    {
        if (hit.collider.tag == "Player")
        {
            PlayerHealth player = hit.transform.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(Damage);
            }
        }
        else
        {
            GameObject defaultImpact = Instantiate(DefaultImpact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(defaultImpact, 10f);
        }
    }

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return laserDuration;
        laserLine.enabled = false;
    }
}
