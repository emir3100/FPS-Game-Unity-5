using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [HideInInspector]
    public WeaponHolder CurrentWeapon;

    public List<WeaponHolder> AllWeapons;
    public List<WeaponHolder> AllWeaponsInInventory;

    public Camera cam;

    public float pickupDistance;
    public float dropForwardForce, dropUpwardForce;
    public int MaxWeapons = 3;
    RaycastHit hit;

    void Start()
    {
        CheckCurrentWeapon();
    }

    void Update()
    {
        

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickupDistance) && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }


    }

    private void CheckCurrentWeapon()
    {
        if (AllWeapons.Count > 0)
        {
            foreach (var weapon in AllWeapons)
            {
                if (weapon.isCurrentWeapon)
                {
                    CurrentWeapon = weapon;
                }
            }
        }
    }

    private void PickUp()
    {
        if (hit.collider.tag == "PickableWeapon" && AllWeaponsInInventory.Count < MaxWeapons)
        {
            foreach (var weapon in AllWeapons)
            {
                if(hit.collider.GetComponent<Weapon>().Name != weapon.Name)
                {
                    foreach (var weaponsAll in AllWeapons)
                    {
                        if (weaponsAll.Name == hit.collider.GetComponent<Weapon>().Name)
                        {
                            AllWeaponsInInventory.Add(weaponsAll);
                            Destroy(hit.collider.gameObject);
                        }
                        
                    }
                }
            }
        }
    }
    private void Drop()
    {

    }
}

[Serializable]
public class WeaponHolder
{
    public string Name;
    public GameObject Weapon;
    public Image Icon;
    public bool isCurrentWeapon;
    public bool isDropable;
}
