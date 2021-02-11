using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Text healthText;
    [Range(0f,100f)]
    public float Health;

    private void Update()
    {
        healthText.text = Health.ToString("000");

        if(Health <= 0)
        {
            Health = 0;
            Dead();
        }
    }

    private void Dead()
    {
        Debug.Log("Player is dead");
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
    }
}
