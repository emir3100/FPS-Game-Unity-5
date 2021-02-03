using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public int SelectedWeapon = 0;
    public Transform CurrentWeapon;
    public Transform GunHolder;
    public Transform UIWeapons;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = SelectedWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (SelectedWeapon >= GunHolder.transform.childCount - 1)
                SelectedWeapon = 0;
            else
                SelectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (SelectedWeapon <= 0)
                SelectedWeapon = GunHolder.transform.childCount - 1;
            else
                SelectedWeapon--;
        }
        if (previousSelectedWeapon != SelectedWeapon)
        {
            SelectWeapon();
        }
    }
    void SelectWeapon()
    {
        //Weapon
        int i = 0;
        foreach (Transform weapon in GunHolder.transform)
        {
            if (i == SelectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                CurrentWeapon = weapon;
            }
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
        //UI Weapon
        int j = 0;
        foreach (Transform weaponUI in UIWeapons.transform)
        {
            if (j == SelectedWeapon)
                weaponUI.gameObject.SetActive(true);
            else
                weaponUI.gameObject.SetActive(false);
            j++;
        }
    }
}
