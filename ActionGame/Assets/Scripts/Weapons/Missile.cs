using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Missile : MonoBehaviour
{
    public GameObject[] ExplosionPrefabs;
    public bool isToucing = false;
    public AudioClip ExplosionSound;
    public float radius = 7;
    public float explosionForce = 800f;
    public float Damage = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player") return;
        isToucing = true;
        if (isToucing)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject randomExplosion = ExplosionPrefabs[Shuffled(ExplosionPrefabs.Length)];
        AudioSource.PlayClipAtPoint(ExplosionSound, this.gameObject.transform.position);
        Instantiate(randomExplosion, this.gameObject.transform.position, this.gameObject.transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, this.transform.position, radius);
            }
        }   
        Destroy(this.gameObject);
    }

    private int Shuffled(int count)
    {
        int randomNum;
        randomNum = Random.Range(0, count);
        return randomNum;
    }
}
