using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsAnimation : MonoBehaviour
{
    public bool IsMoving;
    public bool IsIdle;
    public float MovingSpeed = 1f;
    public Animator LegsAnimator;
    public string MovingName;
    public string IdleName;
    public string MovingSpeedName;

    public PlayerMovement playerScript;

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
    }

    private void SetBoolAnimator(string param, bool boolean)
    {
        LegsAnimator.SetBool(param, boolean);
    }

    private void MovingSpeedAnimator(string param)
    {
        LegsAnimator.SetFloat(param, MovingSpeed);
    }
}
