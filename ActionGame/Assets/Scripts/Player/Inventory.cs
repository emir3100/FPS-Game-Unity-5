using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public Transform AllWeapons;
    private Transform weaponHolder;
    public List<Transform> AllWeaponsList;
    public List<Transform> WeaponEquippedList;
    public Camera cam;
    public WeaponManager WeaponManagerScript;
    public float pickupDistance;
    public float dropForwardForce, dropUpwardForce;
    public int MaxWeapons = 3;
    RaycastHit hit;

    public GameObject LaserPistolPrefab;

    void Start()
    {
        weaponHolder = this.transform;
        AllWeaponsList = GetAllWeapons();
    }

    void Update()
    {
        WeaponEquippedList = GetAllEquippedWeapons();
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickupDistance) && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }
    }

    public List<Transform> GetAllEquippedWeapons()
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform weapon in weaponHolder)
        {
            list.Add(weapon);
        }
        return list;
    }

    private List<Transform> GetAllWeapons()
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform weapon in AllWeapons)
        {
            list.Add(weapon);
        }
        return list;
    }

    private Transform GetWeapon(string name)
    {
        foreach (Transform weapon in AllWeaponsList)
        {
            Weapon weaponScript = weapon.GetComponent<Weapon>();
            if(weaponScript != null)
            {
                if (weaponScript.Name == name)
                    return weapon;
            }
        }
        return null;
    }

    private void PickUp()
    {
        if(hit.collider.tag == "PickableWeapon")
        {
            PickupableWeapon pickupableWeapon = hit.transform.GetComponent<PickupableWeapon>();
            if(pickupableWeapon != null)
            {
                if (pickupableWeapon.IsPickedUp == false)
                {
                    Destroy(hit.collider.gameObject);
                    GetWeapon(pickupableWeapon.Name).parent = weaponHolder;
                    GetAllEquippedWeapons();
                }
            }
        }
    }
    private void Drop()
    {
        var currentWeapon = WeaponManagerScript.CurrentWeapon;
        currentWeapon.parent = AllWeapons;
        currentWeapon.gameObject.SetActive(false);
        Debug.Log("drop");

        if(currentWeapon.GetComponent<Weapon>().Name == "Laser Pistol")
        {
            Instantiate(LaserPistolPrefab, transform.position, transform.rotation);
        }
        WeaponManagerScript.SelectedWeapon = 0;
        currentWeapon = null;
    }
}
