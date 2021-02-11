using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private RectTransform crosshairTransform;

    [Range (10f,60f)]
    public float Size = 10f;
    [Range(5f, 25f)]
    public float Lenght = 5f;
    [Range(1f, 10f)]
    public float Width = 2f;

    public bool ChangeOnShoot = true;
    public bool PlayerShooting;
    public float Speed = 2f;
    private RectTransform top, bottom, left, right;
    private float originalSize;

    [System.Obsolete]
    private void Start()
    {
        originalSize = Size;
        crosshairTransform = GetComponent<RectTransform>();
        top = transform.Find("Top").GetComponent<RectTransform>();
        bottom = transform.Find("Bottom").GetComponent<RectTransform>();
        left = transform.Find("Left").GetComponent<RectTransform>();
        right = transform.Find("Right").GetComponent<RectTransform>();
    }

    private void Update()
    {
        crosshairTransform.sizeDelta = new Vector2(Size, Size);
        top.sizeDelta = new Vector2(Width, Lenght);
        bottom.sizeDelta = new Vector2(Width, Lenght);
        left.sizeDelta = new Vector2(Lenght, Width);
        right.sizeDelta = new Vector2(Lenght, Width);


        if (ChangeOnShoot)
        {
            if (PlayerShooting)
                StartCoroutine("ShootCrosshair");
            else
            {
                StopCoroutine("ShootCrosshair");
                StartCoroutine("ResetShootCrosshair");
            }
        }
    }

    private IEnumerator ShootCrosshair()
    {
        if(Size < originalSize*2.5f)
        {
            Size += 1f * Speed * Time.deltaTime;
            crosshairTransform.sizeDelta = new Vector2(Size, Size);
        }
    
        yield return null;
    }
    private IEnumerator ResetShootCrosshair()
    {
        yield return new WaitForSeconds(0.05f);
        if(Size > originalSize)
        {
            Size -= 1f * Speed*4f * Time.deltaTime;
            crosshairTransform.sizeDelta = new Vector2(Size, Size);
        }
    }

    public void IsShooting(bool isShooting)
    {
        if (isShooting)
            PlayerShooting = true;
        else
            PlayerShooting = false;
    }
}
