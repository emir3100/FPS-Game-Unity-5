using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableWeapon : MonoBehaviour
{
    [Header("Showcase Variables")]
    public float RotationSpeed;
    public float Height;
    public GameObject PickUpImage;
    public GameObject NoPickUpImage;

    [Header("Weapon Information Variables")]
    public string Name;
    public bool IsPickedUp;
    public Inventory InventoryScript;

    private RaycastHit raycast;
    private Vector3 oldPosition;

    void Start()
    {
        oldPosition = this.transform.position;
    }

    void Update()
    {
        this.transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
        Physics.Raycast(oldPosition, this.transform.up, out raycast, Height);
        Vector3 endPoint = oldPosition + (this.transform.up.normalized * Height);
        this.transform.position = new Vector3(endPoint.x, endPoint.y, endPoint.z);

        if(IsPickedUp)
        {
            NoPickUpImage.SetActive(true);
            PickUpImage.SetActive(false);
        }
        else
        {
            NoPickUpImage.SetActive(false);
            PickUpImage.SetActive(true);
        }
        //CheckIfIsEquipped();
    }

    private void CheckIfIsEquipped()
    {
        List<Transform> list = InventoryScript.GetAllEquippedWeapons();
        foreach (Transform weapon in list)
        {
            Weapon weaponScript = weapon.GetComponent<Weapon>();
            if (weaponScript != null && Name == weaponScript.Name)
            {
                IsPickedUp = true;
                return;
            }
        }
        IsPickedUp = false;
    }
}
