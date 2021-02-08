using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fuel : MonoBehaviour
{
    public Slider Slider;
    public float MaxFuel = 100f;
    public float CurrentFuel;
    public float RefuelSpeed = 15f;
    public float ConsumeFuelSpeed = 10f;

    private PlayerMovement playerMovementScript;

    public void Start()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Slider.maxValue = MaxFuel;
        Slider.value = CurrentFuel;

        if (playerMovementScript.isFlying)
        {
            if(CurrentFuel > 0)
                CurrentFuel -= Time.deltaTime * ConsumeFuelSpeed;
            
        }
        else
        {
            if(CurrentFuel <= 100f)
                CurrentFuel += Time.deltaTime * RefuelSpeed;
        }

        if (CurrentFuel < 0)
        {
            CurrentFuel = 0;
            playerMovementScript.isFlying = false;
            playerMovementScript.JetPackSmoke.Stop();
        }
            

        if (CurrentFuel > 100f)
            CurrentFuel = 100f;
    }
}
