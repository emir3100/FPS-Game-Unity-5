using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    public int Id;
    public Image Img;
    public Transform Target;
    public Text Meter;
    public Text InfoText;
    public Text ObjectiveWarning;
    public Vector3 Offset;
    public float AllowedDistance;
    private PlayerHealth playerHealthScript;
    public bool IsTrigger;
    public bool IsActive;
    private Objective currentObjective => FindCurrentObjective();
    private ObjectiveManager objectiveManager;

    private void Start()
    {
        objectiveManager = GameObject.FindGameObjectsWithTag("Player").First().GetComponent<ObjectiveManager>();
        playerHealthScript = GameObject.FindGameObjectsWithTag("Player").First().GetComponent<PlayerHealth>();
        
        
    }

    private Objective FindCurrentObjective()
    {
        var ObjectiveContainer = this.gameObject;
        foreach (var objective in objectiveManager.ObjectivesActive)
        {
            if (Id == objective.Id)
                return objective;
        }
        return null;
    }

    private void Update()
    {
        if(Target == null)
        {
            currentObjective.enabled = false;
        }

        if (IsTrigger)
        {
            if (Vector3.Distance(Target.position, transform.position) < 4f)
            {
                currentObjective.enabled = false;
            }
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
        InfoText.text = "Objective";

        if (Vector3.Distance(Target.position, transform.position) > AllowedDistance)
        {
            playerHealthScript.Health -= 0.1f;
            ObjectiveWarning.enabled = true;
        }
        else
            ObjectiveWarning.enabled = false;
    }
}