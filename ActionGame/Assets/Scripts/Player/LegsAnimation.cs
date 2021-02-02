using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsAnimation : MonoBehaviour
{
    public bool IsMoving;
    public bool IsIdle;
    public float Velocity = 0.1f;
    private Animator LegsAnimator;
    public string MovingName;
    public string IdleName;
    public string MovingSpeedName;
    int VelocityHash;
    public PlayerMovement playerScript;
    private void Start()
    {
        LegsAnimator = GetComponent<Animator>();
        VelocityHash = Animator.StringToHash("Velocity");
    }
    void Update()
    {
        if (playerScript.IsMoving())
        {
            IsMoving = true;
            IsIdle = false;
        }
        else
        {
            IsMoving = false;
            IsIdle = true;
        }

        if (IsMoving)
        {
            SetBoolAnimator(MovingName, true);
            SetBoolAnimator(IdleName, false);
        }
        else if (IsIdle)
        {
            SetBoolAnimator(MovingName, false);
            SetBoolAnimator(IdleName, true);
        }

        LegsAnimator.SetFloat(VelocityHash, Velocity);
        Velocity = playerScript.rb.velocity.magnitude/100f;

        if (Velocity > 1f)
            Velocity = 1f;
    }

    private void SetBoolAnimator(string param, bool boolean)
    {
        LegsAnimator.SetBool(param, boolean);
    }
}
