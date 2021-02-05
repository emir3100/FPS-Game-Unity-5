using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public GameObject CurrentWeapon;
    private List<GameObject> allWeapons => GetAllWeapons();
    private int totalWeapons => allWeapons.Count;
    
    public GameObject Slot1; // type:0 
    public GameObject Slot2; // type:1
    public GameObject Slot3; // type:2

    private bool slot1Full, slot2Full, slot3Full;

    private WeaponDatabase weaponDatabaseScript;

    private void Start()
    {
        weaponDatabaseScript = this.gameObject.GetComponent<WeaponDatabase>();
        AddDefault();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            PreviousWeapon();

        if (Input.GetKeyDown(KeyCode.E))
            NextWeapon();

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            Select(0);

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            Select(1);

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            Select(2);

        if (Input.GetKeyDown(KeyCode.G))
            DropWeapon();

        ShowWeapon();
    }

    private GameObject GetSlotType(WeaponData weapon)
    {
        if (weapon.Type == 0)
            return Slot1;
        else if (weapon.Type == 1)
            return Slot2;
        else if (weapon.Type == 2)
            return Slot3;
        else
            return null;
    }

    private List<GameObject> GetAllWeapons()
    {
        List<GameObject> allWeapons = new List<GameObject>();
        foreach (Transform weapon in Slot1.transform)
        {
            allWeapons.Add(weapon.gameObject);
        }

        foreach (Transform weapon in Slot2.transform)
        {
            allWeapons.Add(weapon.gameObject);
        }

        foreach (Transform weapon in Slot3.transform)
        {
            allWeapons.Add(weapon.gameObject);
        }
        return allWeapons;
    }

    private void ShowWeapon()
    {
        foreach (var weapon in allWeapons)
        {
            if (weapon == CurrentWeapon)
                weapon.SetActive(true);
            else
                weapon.SetActive(false);
        }
    }
    
    private bool Exists(int weaponId)
    {
        foreach (Transform weapon in Slot1.transform)
        {
            if (weaponId == weapon.GetComponent<Weapon>().Id)
                return true;

            if (weapon != null)
                slot1Full = true;
        }

        foreach (Transform weapon in Slot2.transform)
        {
            if (weaponId == weapon.GetComponent<Weapon>().Id)
                return true;

            if (weapon != null)
                slot2Full = true;
        }

        foreach (Transform weapon in Slot3.transform)
        {
            if (weaponId == weapon.GetComponent<Weapon>().Id)
                return true;

            if (weapon != null)
                slot3Full = true;
        }
        return false;
    }

    public void AddDefault()
    {
        var weapon = weaponDatabaseScript.Weapons[5];
        var instantiatedWeapon = Instantiate(weapon.WeaponPrefab);
        instantiatedWeapon.transform.SetParent(Slot3.transform);
        instantiatedWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero); // Sets the rotation to 0
        instantiatedWeapon.transform.localScale = Vector3.one; // Sets the scale to 1
        instantiatedWeapon.transform.localPosition = weapon.Position;
        CurrentWeapon = instantiatedWeapon;
    }

    public void AddWeapon(WeaponID weaponId)
    {
        if(weaponId.Id == 0 && !Exists(weaponId.Id) && !slot2Full)
        {
            var weapon = weaponDatabaseScript.Weapons[weaponId.Id];
            var instantiatedWeapon = Instantiate(weapon.WeaponPrefab);
            instantiatedWeapon.transform.SetParent(GetSlotType(weapon).transform);
            instantiatedWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero); // Sets the rotation to 0
            instantiatedWeapon.transform.localScale = Vector3.one; // Sets the scale to 1
            instantiatedWeapon.transform.localPosition = weapon.Position;
            SetWeaponValues(weaponId, instantiatedWeapon);
            Destroy(weaponId.gameObject);
            FindObjectOfType<AudioManager>().Play("PickUpSound");
        }
        else
        {
            Debug.Log("Weapon is already in your inventory");
        }

        if (weaponId.Id == 1 && !Exists(weaponId.Id) && !slot2Full)
        {
            var weapon = weaponDatabaseScript.Weapons[weaponId.Id];
            var instantiatedWeapon = Instantiate(weapon.WeaponPrefab);
            instantiatedWeapon.transform.SetParent(GetSlotType(weapon).transform);
            instantiatedWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero); // Sets the rotation to 0
            instantiatedWeapon.transform.localScale = Vector3.one; // Sets the scale to 1
            instantiatedWeapon.transform.localPosition = weapon.Position;
            SetWeaponValues(weaponId, instantiatedWeapon);
            Destroy(weaponId.gameObject);
            FindObjectOfType<AudioManager>().Play("PickUpSound");
        }
        else
        {
            Debug.Log("Weapon is already in your inventory");
        }

        if (weaponId.Id == 2 && !Exists(weaponId.Id) && !slot1Full)
        {
            var weapon = weaponDatabaseScript.Weapons[weaponId.Id];
            var instantiatedWeapon = Instantiate(weapon.WeaponPrefab);
            instantiatedWeapon.transform.SetParent(GetSlotType(weapon).transform);
            instantiatedWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero); // Sets the rotation to 0
            instantiatedWeapon.transform.localScale = Vector3.one; // Sets the scale to 1
            instantiatedWeapon.transform.localPosition = weapon.Position;
            SetWeaponValues(weaponId, instantiatedWeapon);
            Destroy(weaponId.gameObject);
            FindObjectOfType<AudioManager>().Play("PickUpSound");
        }
        else
        {
            Debug.Log("Weapon is already in your inventory");
        }

        if (weaponId.Id == 3 && !Exists(weaponId.Id) && !slot1Full)
        {
            var weapon = weaponDatabaseScript.Weapons[weaponId.Id];
            var instantiatedWeapon = Instantiate(weapon.WeaponPrefab);
            instantiatedWeapon.transform.SetParent(GetSlotType(weapon).transform);
            instantiatedWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero); // Sets the rotation to 0
            instantiatedWeapon.transform.localScale = Vector3.one; // Sets the scale to 1
            instantiatedWeapon.transform.localPosition = weapon.Position;
            SetWeaponValues(weaponId, instantiatedWeapon);
            Destroy(weaponId.gameObject);
            FindObjectOfType<AudioManager>().Play("PickUpSound");
        }
        else
        {
            Debug.Log("Weapon is already in your inventory");
        }

        if (weaponId.Id == 4 && !Exists(weaponId.Id) && !slot1Full)
        {
            var weapon = weaponDatabaseScript.Weapons[weaponId.Id];
            var instantiatedWeapon = Instantiate(weapon.WeaponPrefab);
            instantiatedWeapon.transform.SetParent(GetSlotType(weapon).transform);
            instantiatedWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero); // Sets the rotation to 0
            instantiatedWeapon.transform.localScale = Vector3.one; // Sets the scale to 1
            instantiatedWeapon.transform.localPosition = weapon.Position;
            SetWeaponValues(weaponId, instantiatedWeapon);
            Destroy(weaponId.gameObject);
            FindObjectOfType<AudioManager>().Play("PickUpSound");
        }
        else
        {
            Debug.Log("Weapon is already in your inventory");
        }

        if (slot1Full)
            Debug.Log("Slot 1 is Full");

        if (slot2Full)
            Debug.Log("Slot 2 is Full");

        if (slot3Full)
            Debug.Log("Slot 3 is Full");
    }

    private void SetWeaponValues(WeaponID weaponId, GameObject instantiatedWeapon)
    {
        var weaponScript = instantiatedWeapon.GetComponent<Weapon>();
        if(weaponScript.Id == 0 || weaponScript.Id == 2 || weaponScript.Id == 3)
        {
            var laserGun = instantiatedWeapon.GetComponent<LaserGun>();
            laserGun.TotalAmmo = weaponId.Ammo;
        }
        else if (weaponScript.Id == 1) // jumpgun
        {
            return;
        }
        else if (weaponScript.Id == 4)
        {
            var rocketLauncher = instantiatedWeapon.GetComponent<RocketLauncher>();
            rocketLauncher.TotalAmmo = weaponId.Ammo;
        }
        else if (weaponScript.Id == 5) // hands
        {
            return;
        }
    }

    private void RemoveWeaponFromInventory(int weaponID)
    {
        foreach (Transform weapon in Slot1.transform)
        {
            if (weaponID == weapon.GetComponent<Weapon>().Id)
                Destroy(weapon.gameObject);
        }

        foreach (Transform weapon in Slot2.transform)
        {
            if (weaponID == weapon.GetComponent<Weapon>().Id)
                Destroy(weapon.gameObject);
        }

        foreach (Transform weapon in Slot3.transform)
        {
            if (weaponID == weapon.GetComponent<Weapon>().Id)
                Destroy(weapon.gameObject);
        }
    }

    private void DropWeapon()
    {
        var id = CurrentWeapon.GetComponent<Weapon>().Id;
        if (Exists(id) && id != 5)
        {
            var weaponScript = CurrentWeapon.GetComponent<Weapon>();
            var weapon = weaponDatabaseScript.Weapons[weaponScript.Id];
            var player = GameObject.FindGameObjectsWithTag("Player").First();
            var instantiatedWeapon = Instantiate(weapon.WeaponToPickUpPrefab, player.transform.position, player.transform.rotation);
            var weaponId = instantiatedWeapon.GetComponent<WeaponID>();

            if (weaponScript.Id == 0 || weaponScript.Id == 2 || weaponScript.Id == 3)
            {
                var laserGun = instantiatedWeapon.GetComponent<LaserGun>();
                //weaponId.Ammo = laserGun.TotalAmmo;
            }
            else if (weaponScript.Id == 1) // jumpgun
            {
                return;
            }
            else if (weaponScript.Id == 4)
            {
                var rocketLauncher = instantiatedWeapon.GetComponent<RocketLauncher>();
                //weaponId.Ammo = rocketLauncher.TotalAmmo;
                
            }
            RemoveWeaponFromInventory(weaponScript.Id);
        }
    }

    private void NextWeapon()
    {

    }

    private void PreviousWeapon()
    {

    }

    private void Select(int type)
    {
        switch (type)
        {
            case 0:
                foreach (Transform weapon in Slot1.transform)
                {
                    if (weapon == null)
                        return;
                    if(type == 0)
                    {
                        CurrentWeapon = weapon.gameObject;
                        return;
                    }
                    weapon.gameObject.SetActive(false);

                }
                break;
            case 1:
                foreach (Transform weapon in Slot2.transform)
                {
                    if (weapon == null)
                        return;

                    if (type == 1)
                    {
                        CurrentWeapon = weapon.gameObject;
                        return;
                    }
                    weapon.gameObject.SetActive(false);
                }
                break;
            case 2:
                foreach (Transform weapon in Slot3.transform)
                {
                    if (weapon == null)
                        return;

                    if (type == 2)
                    {
                        CurrentWeapon = weapon.gameObject;
                        return;
                    }
                    weapon.gameObject.SetActive(false);
                }
                break;
        }
    }

    




    //public int SelectedWeapon = 0;
    //public Transform GunHolder;
    //public Transform UIWeapons;

    //void Start()
    //{
    //    SelectWeapon();
    //}

    //void Update()
    //{
    //    int previousSelectedWeapon = SelectedWeapon;
    //    if (Input.GetAxis("Mouse ScrollWheel") > 0f)
    //    {
    //        if (SelectedWeapon >= GunHolder.transform.childCount - 1)
    //            SelectedWeapon = 0;
    //        else
    //            SelectedWeapon++;
    //    }
    //    if (Input.GetAxis("Mouse ScrollWheel") < 0f)
    //    {
    //        if (SelectedWeapon <= 0)
    //            SelectedWeapon = GunHolder.transform.childCount - 1;
    //        else
    //            SelectedWeapon--;
    //    }
    //    if (previousSelectedWeapon != SelectedWeapon)
    //    {
    //        SelectWeapon();
    //    }
    //}
    //void SelectWeapon()
    //{
    //    //Weapon
    //    int i = 0;
    //    foreach (Transform weapon in GunHolder.transform)
    //    {
    //        if (i == SelectedWeapon)
    //            weapon.gameObject.SetActive(true);
    //        else
    //            weapon.gameObject.SetActive(false);
    //        i++;
    //    }
    //    //UI Weapon
    //    int j = 0;
    //    foreach (Transform weaponUI in UIWeapons.transform)
    //    {
    //        if (j == SelectedWeapon)
    //            weaponUI.gameObject.SetActive(true);
    //        else
    //            weaponUI.gameObject.SetActive(false);
    //        j++;
    //    }
    //}
}
