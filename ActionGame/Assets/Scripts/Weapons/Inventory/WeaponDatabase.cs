using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDatabase : MonoBehaviour
{
    public List<WeaponData> Weapons = new List<WeaponData>();
}

[System.Serializable]
public class WeaponData
{
    public int Id;
    public int Type;
    public string Name;
    public GameObject WeaponPrefab;
    public GameObject WeaponToPickUpPrefab;
    [HideInInspector]
    public Vector3 Position => WeaponPrefab.transform.position;
}
