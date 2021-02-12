using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            var rigidbody = collision.collider.GetComponent<Rigidbody>();
            AddForce(rigidbody);
        }
    }

    private void AddForce(Rigidbody rigidbody)
    {

        if (rigidbody != null)
        {
            rigidbody.AddForce(Vector3.up * 4000f);
            FindObjectOfType<AudioManager>().Play("JumpGun");
        }
    }
}
