using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public GameObject CurrentWeapon;
    public GameObject PreviousWeapon;
    private List<GameObject> allWeapons => GetAllWeapons();
    
    public GameObject Slot1, Slot2, Slot3;
    public GameObject Slot1UI, Slot2UI, Slot3UI;

    private bool slot1Full, slot2Full, slot3Full;

    private WeaponDatabase weaponDatabaseScript;

    public Text AmmoUiText;
    public GameObject InventoryUI;

    private void Start()
    {
        weaponDatabaseScript = this.gameObject.GetComponent<WeaponDatabase>();
        AddDefault();

        PreviousWeapon = CurrentWeapon;
        FadeAllUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SelectPreviousWeapon();


        if (Input.GetKeyDown(KeyCode.E))
            SelectNextWeapon();

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            Select(0);
            ChangeWeapon(CurrentWeapon);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            Select(1);
            ChangeWeapon(CurrentWeapon);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            ChangeWeapon(CurrentWeapon);
            Select(2);
        }

        if (Input.GetKeyDown(KeyCode.G))
            DropWeapon();

        ShowWeapon();
        SetAmmoText(AmmoUiText);
        SetSlotActive();
    }

    private void SetAmmoText(Text text)
    {
        var weaponScript = CurrentWeapon.GetComponent<Weapon>();
        if (weaponScript.Id == 0 || weaponScript.Id == 2 || weaponScript.Id == 3)
        {
            var laserGun = weaponScript.GetComponent<LaserGun>();
            text.text = laserGun.TotalAmmo.ToString("0000");
            text.fontSize = 30;
        }
        else if (weaponScript.Id == 4)
        {
            var rocketLauncher = weaponScript.GetComponent<RocketLauncher>();
            text.text = rocketLauncher.TotalAmmo.ToString("0000");
            text.fontSize = 30;
        }
        else if (weaponScript.Id == 1 || weaponScript.Id == 5)
        {
            text.text = "∞";
            text.fontSize = 60;
        }
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
            if (weapon != null)
                allWeapons.Add(weapon.gameObject);
        }

        foreach (Transform weapon in Slot2.transform)
        {
            if (weapon != null)
                allWeapons.Add(weapon.gameObject);
        }

        foreach (Transform weapon in Slot3.transform)
        {
            if (weapon != null)
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
    
    public bool Exists(int weaponId)
    {
        foreach (Transform weapon in Slot1.transform)
        {
            if (weaponId == weapon.GetComponent<Weapon>().Id)
                return true;

            if (weapon != null)
                slot1Full = true;
            else
                slot1Full = false;
        }

        foreach (Transform weapon in Slot2.transform)
        {
            if (weaponId == weapon.GetComponent<Weapon>().Id)
                return true;

            if (weapon != null)
                slot2Full = true;
            else
                slot2Full = true;
        }

        foreach (Transform weapon in Slot3.transform)
        {
            if (weaponId == weapon.GetComponent<Weapon>().Id)
                return true;

            if (weapon != null)
                slot3Full = true;
            else
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
        GetWeaponUI(weapon.Id, weapon.Type);
        
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
            GetWeaponUI(weapon.Id, weapon.Type);
            Destroy(weaponId.gameObject);
            FindObjectOfType<AudioManager>().Play("PickUpSound");
        }
        else
        {
            //weapon already exists
        }

        if (weaponId.Id == 1 && !Exists(weaponId.Id) && !slot2Full)
        {
            var weapon = weaponDatabaseScript.Weapons[weaponId.Id];
            var instantiatedWeapon = Instantiate(weapon.WeaponPrefab);
            instantiatedWeapon.transform.SetParent(GetSlotType(weapon).transform);
            instantiatedWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero); // Sets the rotation to 0
            instantiatedWeapon.transform.localScale = Vector3.one; // Sets the scale to 1
            instantiatedWeapon.transform.localPosition = weapon.Position;
            GetWeaponUI(weapon.Id, weapon.Type);
            Destroy(weaponId.gameObject);
            FindObjectOfType<AudioManager>().Play("PickUpSound");
        }
        else
        {
            //weapon already exists
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
            GetWeaponUI(weapon.Id, weapon.Type);
            Destroy(weaponId.gameObject);
            FindObjectOfType<AudioManager>().Play("PickUpSound");
        }
        else
        {
            //weapon already exists
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
            GetWeaponUI(weapon.Id, weapon.Type);
            Destroy(weaponId.gameObject);
            FindObjectOfType<AudioManager>().Play("PickUpSound");
        }
        else
        {
            //weapon already exists
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
            GetWeaponUI(weapon.Id, weapon.Type);
            Destroy(weaponId.gameObject);
            FindObjectOfType<AudioManager>().Play("PickUpSound");
        }
        else
        {
            //weapon already exists
        }
    }

    private void SetSlotActive()
    {
        var weaponID = CurrentWeapon.GetComponent<Weapon>().Id;
        var weaponType = weaponDatabaseScript.Weapons.First(x => x.Id == weaponID).Type;
        switch (weaponType)
        {
            case 0:
                
                Slot1UI.gameObject.SetActive(true);
                Slot1UI.transform.localScale = Vector3.zero;
                LeanTween.scale(Slot1UI, Vector3.one, 0.1f).setEaseLinear();
                Slot2UI.gameObject.SetActive(false);
                Slot3UI.gameObject.SetActive(false);
                break;
            case 1:
                Slot1UI.gameObject.SetActive(false);
                Slot2UI.gameObject.SetActive(true);
                Slot2UI.transform.localScale = Vector3.zero;
                LeanTween.scale(Slot2UI, Vector3.one, 0.1f).setEaseLinear();
                Slot3UI.gameObject.SetActive(false);
                break;
            case 2:
                Slot1UI.gameObject.SetActive(false);
                Slot2UI.gameObject.SetActive(false);
                Slot3UI.gameObject.SetActive(true);
                Slot3UI.transform.localScale = Vector3.zero;
                LeanTween.scale(Slot3UI, Vector3.one, 0.1f).setEaseLinear();
                break;
        }
    }

    private void SetWeaponValues(WeaponID weaponId, GameObject instantiatedWeapon)
    {
        var weaponScript = instantiatedWeapon.GetComponent<Weapon>();
        if(weaponScript.Id == 0 || weaponScript.Id == 2 || weaponScript.Id == 3)
        {
            var laserGun = instantiatedWeapon.GetComponent<LaserGun>();
            laserGun.TotalAmmo = weaponId.Ammo;
        }
        else if (weaponScript.Id == 4)
        {
            var rocketLauncher = instantiatedWeapon.GetComponent<RocketLauncher>();
            rocketLauncher.TotalAmmo = weaponId.Ammo;
        }
    }

    private void SendWeaponValues(Weapon weaponScript, GameObject instantiatedWeapon)
    {
        var weaponId = instantiatedWeapon.GetComponent<WeaponID>();
        if (weaponScript.Id == 0 || weaponScript.Id == 2 || weaponScript.Id == 3)
        {
            var laserGun = weaponScript.GetComponent<LaserGun>();
            weaponId.SetAmmo(laserGun.TotalAmmo);
        }
        else if (weaponScript.Id == 4)
        {
            var rocketLauncher = weaponScript.GetComponent<RocketLauncher>();
            weaponId.SetAmmo(rocketLauncher.TotalAmmo);

        }
    }

    private void RemoveWeaponFromInventory(int weaponID)
    {
        foreach (Transform weapon in Slot1.transform)
        {
            if (weaponID == weapon.GetComponent<Weapon>().Id)
            {
                Destroy(weapon.gameObject);
                slot1Full = false;
            }
        }

        foreach (Transform weapon in Slot2.transform)
        {
            if (weaponID == weapon.GetComponent<Weapon>().Id)
            {
                Destroy(weapon.gameObject);
                slot2Full = false;
            }
        }

        foreach (Transform weapon in Slot3.transform)
        {
            if (weaponID == weapon.GetComponent<Weapon>().Id)
            {
                Destroy(weapon.gameObject);
                slot3Full = false;
            }
        }
    }

    private void RemoveWeaponUI(int id, int type)
    {
        switch (type)
        {
            case 0:
                foreach (Transform item in Slot1UI.transform)
                {
                    if (item == null)
                        return;


                    var uiId = item.GetComponent<WeaponIDUI>();
                    if (uiId != null)
                    {
                        if (id == uiId.Id)
                        {
                            item.GetComponent<Image>().enabled = false;
                        }
                    }
                }
                break;

            case 1:
                foreach (Transform item in Slot2UI.transform)
                {
                    if (item == null)
                        return;


                    var uiId = item.GetComponent<WeaponIDUI>();
                    if (uiId != null)
                    {
                        if (id == uiId.Id)
                        {
                            item.GetComponent<Image>().enabled = false;
                        }
                    }
                }
                break;

            case 2:
                foreach (Transform item in Slot3UI.transform)
                {
                    if (item == null)
                        return;


                    var uiId = item.GetComponent<WeaponIDUI>();
                    if (uiId != null)
                    {
                        if (id == uiId.Id)
                        {
                            item.GetComponent<Image>().enabled = false;
                        }
                    }
                }
                break;
        }
    }

    private void AddFroceOnDrop(GameObject instantiatedWeapon, float dropForwardForce, float dropUpwardForce)
    {
        Rigidbody rigidbody;
        rigidbody = instantiatedWeapon.GetComponent<Rigidbody>();

        rigidbody.AddForce(Camera.main.transform.forward * dropForwardForce, ForceMode.Impulse);
        rigidbody.AddForce(Camera.main.transform.up * dropUpwardForce, ForceMode.Impulse);
    }

    private void DropWeapon()
    {
        if(CurrentWeapon != null)
        {
            var id = CurrentWeapon.GetComponent<Weapon>().Id;
            if (Exists(id) && id != 5)
            {
                var weaponScript = CurrentWeapon.GetComponent<Weapon>();
                var weapon = weaponDatabaseScript.Weapons[weaponScript.Id];
                var player = GameObject.FindGameObjectsWithTag("Player").First();
                var instantiatedWeapon = Instantiate(weapon.WeaponToPickUpPrefab, player.transform.position, player.transform.rotation);
                AddFroceOnDrop(instantiatedWeapon, 15f, 7f);
                FindObjectOfType<AudioManager>().Play("DropSound");
                SendWeaponValues(weaponScript, instantiatedWeapon);
                RemoveWeaponFromInventory(weaponScript.Id);
                RemoveWeaponUI(weaponScript.Id, weapon.Type);
                Select(2);
            }
        }
    }

    private void ChangeWeapon(GameObject newWeapon)
    {
        PreviousWeapon = CurrentWeapon;
        CurrentWeapon = newWeapon;
    }

    private void SelectNextWeapon()
    {

    }

    private void SelectPreviousWeapon()
    {
        //GameObject holder = CurrentWeapon;
        //CurrentWeapon

    }

    private void GetWeaponUI(int id, int type)
    {
        switch (type)
        {
            case 0:
                foreach (Transform item in Slot1UI.transform)
                {
                    if (item == null)
                        return;

                    
                    var uiId = item.GetComponent<WeaponIDUI>();
                    if(uiId != null)
                    {
                        if (id == uiId.Id)
                        {
                            item.GetComponent<Image>().enabled = true;
                        }
                        else if(!Exists(id))
                        {
                            item.GetComponent<Image>().enabled = false;
                        }
                        else
                        {
                            item.GetComponent<Image>().enabled = false;
                        }
                    }
                }
                break;

            case 1:
                foreach (Transform item in Slot2UI.transform)
                {
                    if (item == null)
                        return;

                    var uiId = item.GetComponent<WeaponIDUI>();
                    if (uiId != null)
                    {
                        if (id == uiId.Id)
                        {
                            item.GetComponent<Image>().enabled = true;
                        }
                        else if (!Exists(id))
                        {
                            item.GetComponent<Image>().enabled = false;
                        }
                        else
                        {
                            item.GetComponent<Image>().enabled = false;
                        }
                    }
                }
                break;

            case 2:
                foreach (Transform item in Slot3UI.transform)
                {
                    if (item == null)
                        return;

                    var uiId = item.GetComponent<WeaponIDUI>();
                    if (uiId != null)
                    {
                        if (id == uiId.Id)
                        {
                            item.GetComponent<Image>().enabled = true;
                        }
                        else if (!Exists(id))
                        {
                            item.GetComponent<Image>().enabled = false;
                        }
                        else
                        {
                            item.GetComponent<Image>().enabled = false;
                        }
                    }
                }
                break;
        }
    }

    private void FetchAllImageText(out List<GameObject> allImages, out List<GameObject> allText)
    {
        allImages = new List<GameObject>();
        allText = new List<GameObject>();

        foreach (Transform item in InventoryUI.transform)
        {
            if (item?.GetComponent<Image>() is null)
                continue;

            allImages.Add(item.gameObject);
            foreach (Transform itemChild in item.transform)
            {
                if (itemChild.GetComponent<Text>() != null)
                    allText.Add(itemChild.gameObject);

                if (itemChild.GetComponent<Image>() is null) 
                    continue;
                        
                allImages.Add(itemChild.gameObject);
                foreach (Transform itemChildsChild in itemChild.transform)
                {
                    if (itemChildsChild.GetComponents<Image>() != null)
                        allImages.Add(itemChildsChild.gameObject);
                }
            }
        }
    }

    private IEnumerator FadeUIImage(Image image, float time)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1); // alpha is 1
        yield return new WaitForSeconds(time);
        
        for (float i = 1; i >= 0; i -= Time.deltaTime) // fade out
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, i);
            yield return null;
        }
    }

    private IEnumerator FadeUIText(Text text, float time)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        yield return new WaitForSeconds(time);
       
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, i);
            yield return null;
        }
        
    }

    private IEnumerator FadeAll()
    {
        FetchAllImageText(out List<GameObject> allImages, out List<GameObject> allText);
        foreach (var image in allImages)
        {
            StartCoroutine(FadeUIImage(image.GetComponent<Image>(), 4f));
        }
        foreach (var text in allText)
        {
            StartCoroutine(FadeUIText(text.GetComponent<Text>(), 4f));
        }
        yield break;
    }

    void FadeAllUI()
    {
        StopAllCoroutines();
        StartCoroutine("FadeAll");
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
                        var id = CurrentWeapon.GetComponent<Weapon>().Id;
                        GetWeaponUI(id, type);
                        Slot1UI.transform.parent.gameObject.SetActive(true);
                        FadeAllUI();
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
                        var id = CurrentWeapon.GetComponent<Weapon>().Id;
                        GetWeaponUI(id, type);
                        FadeAllUI();
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
                        var id = CurrentWeapon.GetComponent<Weapon>().Id;
                        GetWeaponUI(id, type);
                        FadeAllUI();
                        return;
                    }
                    weapon.gameObject.SetActive(false);
                }
                break;
        }
    }
}