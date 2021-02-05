using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    private Animator animator;
    private Weapon weaponScript;
    public PlayerMovement PlayerMovementScript;
    public bool PlayerIsMoving => PlayerMovementScript.IsMoving();
    private void Start()
    {
        animator = this.GetComponent<Animator>();
        weaponScript = this.GetComponent<Weapon>();
        if (PlayerMovementScript == null)
            PlayerMovementScript = GameObject.FindGameObjectsWithTag("Player").First().GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (PlayerIsMoving)
            MovingAnimation(weaponScript.MovingParam);
        else
            StopMovingAnimation(weaponScript.MovingParam);

        animator.SetFloat("PlayerVelocity", PlayerMovementScript.PlayerVelocity);
    }

    public void ShootAnimation(string param)
    {
        animator.SetTrigger(param);
    }

    private void MovingAnimation(string param)
    {
        animator.SetBool(param, true);
    }

    private void StopMovingAnimation(string param)
    {
        animator.SetBool(param, false);
    }
}
