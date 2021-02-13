using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionWaypoint : MonoBehaviour
{
    public string Info;
    public Image Img;
    public Transform Target;
    public Text Meter;
    public Text InfoText;
    public Vector3 Offset;

    private void Update()
    {
        if(Target == null)
        {
            Img.gameObject.SetActive(false);
        }



        float minX = Img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;
        float minY = Img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(Target.position + Offset);

        if (Vector3.Dot((Target.position - transform.position), transform.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        Img.transform.position = pos;
        Meter.text = ((int)Vector3.Distance(Target.position, transform.position)).ToString() + "m";
        InfoText.text = Info;
    }
}