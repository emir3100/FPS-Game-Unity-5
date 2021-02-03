using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    public string Name;
    public float FireRate = 15f;
    public float Distance = 100f;
    public float Force = 100f;
    public Transform GunEnd;
    public ParticleSystem Muzzle;
    public Camera cam;
    public bool IsShooting;
    [HideInInspector]
    public float nextFire = 0f;

    public Text TotalAmmoText;
    public RaycastHit hit;
    public WeaponAnimation WeaponAnimationScript;

    [Header("Weapon Animator")]
    public string ShootParam;
    public string MovingParam;

    public virtual void Start()
    {
        cam = GetComponentInParent<Camera>();
        WeaponAnimationScript = this.GetComponent<WeaponAnimation>();
    }

    public virtual void Update()
    {
    }

    public virtual void Shoot()
    {
        
    }
}
