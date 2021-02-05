using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID : MonoBehaviour
{
    public int Id;
    public string Name;
    public int Ammo;
    public float Damage;
    public float Distance;
    public float FireRate;
    public float Force;

    public int Type; // 0 = Slot1, 1 = Slot2, 2 = Slot3
}
