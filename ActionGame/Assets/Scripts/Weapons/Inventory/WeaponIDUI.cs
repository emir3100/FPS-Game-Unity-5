using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponIDUI : MonoBehaviour
{
    public int Id;
    public string Name;
    public int Type;
    private Image image;

    private void Start()
    {
        image = this.GetComponent<Image>();
        image.enabled = false;
    }
}
