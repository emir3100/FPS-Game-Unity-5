using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private RectTransform crosshairTransform;

    [Range (10f,60f)]
    public float size = 10f;
    [Range(5f, 25f)]
    public float lenght = 5f;
    [Range(1f, 10f)]
    public float width = 2f;

    private RectTransform top, bottom, left, right;

    [System.Obsolete]
    private void Start()
    {
        crosshairTransform = GetComponent<RectTransform>();
        top = transform.FindChild("Top").GetComponent<RectTransform>();
        bottom = transform.FindChild("Bottom").GetComponent<RectTransform>();
        left = transform.FindChild("Left").GetComponent<RectTransform>();
        right = transform.FindChild("Right").GetComponent<RectTransform>();
    }

    void Update()
    {
        crosshairTransform.sizeDelta = new Vector2(size, size);
        top.sizeDelta = new Vector2(width, lenght);
        bottom.sizeDelta = new Vector2(width, lenght);
        left.sizeDelta = new Vector2(lenght, width);
        right.sizeDelta = new Vector2(lenght, width);
    }
}
