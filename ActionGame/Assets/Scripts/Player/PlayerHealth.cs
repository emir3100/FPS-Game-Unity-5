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
    private Camera camera;
    private float effect;
    public Image deathImage;
    public float speed = 5f;
    public GameObject CanvasMoving;

    private void Start()
    {
        camera = Camera.main;
        deathImage.color = new Color(deathImage.color.r, deathImage.color.g, deathImage.color.b, 0f);
    }

    private void Update()
    {
        camera.GetComponent<GlitchEffect>().intensity = effect;
        camera.GetComponent<GlitchEffect>().flipIntensity = effect;
        camera.GetComponent<GlitchEffect>().colorIntensity = effect;

        effect = 1- (Health / 100f);
        healthText.text = Health.ToString("000");

        if(Health <= 0)
        {
            Health = 0;
            Dead();
        }

        if (Health < 100f && Health > 0f)
            StartCoroutine("Regenerate");

        if(Health > 100)
        {
            Health = 100;
        }
    }

    private void Dead()
    {
        Debug.Log("Player is dead");
        deathImage.color = new Color(deathImage.color.r, deathImage.color.g, deathImage.color.b, 255f);
        deathImage.GetComponentInChildren<Text>().enabled = true;
        FindObjectOfType<AudioManager>().Play("Death");
        CanvasMoving.SetActive(false);
        Destroy(this.gameObject);
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
        FindObjectOfType<AudioManager>().Play("PlayerHit");
    }

    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(7f);
        Health += 5f * Time.deltaTime;
    }

}
