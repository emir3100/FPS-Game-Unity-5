using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform RagdollVersionPrefab;
    private Animator animator;
    private Rigidbody rigidbody;
    private Transform ragdollClone;
    

    public float Health = 100f;

    public bool IsWalking;
    public bool HasStopped;
    public bool IsShooting;

    [Header("navmesh")]
    public Transform Player;
    private NavMeshAgent agent;
    public float AIWalkSpeed = 1.5f;
    public WayPoint CurrentWayPoint;
    public int MoveDirection; // 0 = forwards, 1 = backwards
    public float CurrentDistanceAIToPlayer;

    public AiState State;
    public bool PlayerAlive;

    [Header("Effects")]
    private Transform headRagdoll;
    private Transform neckRagdoll;
    public GameObject HeadShotEffect;

    [Header("Weapon")]
    public GameObject DropWeapon;
    public GameObject DropAmmo;
    private EnemyWeapon enemyWeaponScript;

    private PlayerHealth playerHealthScript;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyWeaponScript = GetComponent<EnemyWeapon>();
        agent.SetDestination(CurrentWayPoint.transform.position);
        playerHealthScript = Player.GetComponent<PlayerHealth>();
        rigidbody = GetComponent<Rigidbody>();
        PlayerAlive = true;
    }

    private void Update()
    {
        agent.speed = AIWalkSpeed;
        var vectorToTarget = this.transform.position - Player.transform.position;
        vectorToTarget.y = 0;
        CurrentDistanceAIToPlayer = vectorToTarget.magnitude;

        AnimationControll();

        switch (State)
        {
            case AiState.Patroling:
                AIPatroling();
                break;
            case AiState.Combat:
                AiCombat();
                break;
            case AiState.Dead:
                break;
        }
        if (Health <= 0)
            State = AiState.Dead;

        if (playerHealthScript.Health <= 0)
            PlayerAlive = false;
        else
            PlayerAlive = true;

        if (CurrentDistanceAIToPlayer < 50f && State != AiState.Dead)
            State = AiState.Combat;
        else
            State = AiState.Patroling;

        if (IsShooting && State != AiState.Dead)
            enemyWeaponScript.StartCoroutine("StartShooting");
        else
            enemyWeaponScript.StopCoroutine("StartShooting");
    }

    private void AnimationControll()
    {
        if (IsWalking && !HasStopped && !IsShooting)
        {
            AnimWalk();
        }
        else if (HasStopped && !IsWalking && !IsShooting)
        {
            AnimIdle();
        }
        else if (IsShooting && !IsWalking && HasStopped)
        {
            AnimStandingShot();
        }
        else if (IsShooting && IsWalking && !HasStopped)
        {
            AnimeWalkingShot();
        }
    }

    private void AiCombat()
    {
        agent.SetDestination(Player.transform.position);
        agent.stoppingDistance = 5f;
        Vector3 tagetDirection = new Vector3(Player.position.x, transform.position.y, Player.position.z);
        if (CurrentDistanceAIToPlayer < 5f)
        {
            IsShooting = true;
            IsWalking = false;
            HasStopped = true;
            AIWalkSpeed = 0f;
            this.transform.LookAt(tagetDirection);

            agent.isStopped = true;
            rigidbody.isKinematic = true;
        }
        else if (CurrentDistanceAIToPlayer < 30f && CurrentDistanceAIToPlayer > 3.5f)
        {
            IsShooting = true;
            IsWalking = true;
            HasStopped = false;
            AIWalkSpeed = 3f;
            this.transform.LookAt(tagetDirection);
            agent.isStopped = false;
            rigidbody.isKinematic = true;
        }
        else
        {
            IsShooting = false;
            IsWalking = true;
            HasStopped = false;
            animator.SetFloat("Speed", 1f);
            AIWalkSpeed = 8f;
            this.transform.LookAt(tagetDirection);
            agent.isStopped = false;
            rigidbody.isKinematic = true;
        }
    }

    private void AIPatroling()
    {
        IsWalking = true;
        HasStopped = false;
        IsShooting = false;
        animator.SetFloat("Speed", 0f);
        AIWalkSpeed = 3f;
        agent.stoppingDistance = 1f;
        agent.SetDestination(CurrentWayPoint.GetPosition());

        if (CurrentWayPoint.Distance(transform) <= 3f)
        {
            bool shouldBranch = false;

            if (CurrentWayPoint.Branches.Count > 0)
            {
                shouldBranch = UnityEngine.Random.Range(0f, 1f) <= CurrentWayPoint.Ratio;
            }

            if (shouldBranch)
            {
                CurrentWayPoint = CurrentWayPoint.GetRandomWaypoint();
            }
            else
            {
                if (MoveDirection == 0)
                {
                    if (CurrentWayPoint.NextWayPoint == null)
                    {
                        MoveDirection = 1;
                        CurrentWayPoint = CurrentWayPoint.PreviousWayPoint;
                    }
                    else
                    {
                        CurrentWayPoint = CurrentWayPoint.NextWayPoint;
                    }
                }
                else if (MoveDirection == 1)
                {
                    if (CurrentWayPoint.PreviousWayPoint == null)
                    {
                        MoveDirection = 0;
                        CurrentWayPoint = CurrentWayPoint.NextWayPoint;
                    }
                    else
                    {
                        CurrentWayPoint = CurrentWayPoint.PreviousWayPoint;
                    }
                }
            }
        }
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
            Dead();

        FindObjectOfType<AudioManager>().Play("DamageAlien");
    }

    private void Dead()
    {
        Health = 0;
        var ragdoll = Instantiate(RagdollVersionPrefab, transform.position, transform.rotation);
        ragdollClone = ragdoll;
        var weapon = Instantiate(DropWeapon, transform.position, transform.rotation);
        var ammo = Instantiate(DropAmmo, transform.position, transform.rotation);
        AddFroceOnDrop(weapon, 7f, 5f);
        AddFroceOnDrop(ammo, 7f, 5f);
        State = AiState.Dead;
        FindObjectOfType<AudioManager>().Play("Die");
        Destroy(this.gameObject);
    }

    public void HeadShot()
    {
        headRagdoll = ragdollClone.Find("Armature").Find("mixamorig1:Hips").Find("mixamorig1:Spine").Find("mixamorig1:Spine1").Find("mixamorig1:Spine2").Find("Neck").Find("Head").transform;
        neckRagdoll = ragdollClone.Find("Armature").Find("mixamorig1:Hips").Find("mixamorig1:Spine").Find("mixamorig1:Spine1").Find("mixamorig1:Spine2").Find("Neck").transform;
        headRagdoll.transform.localScale = new Vector3(0f, 0f, 0f);
        GameObject bloodhs = Instantiate(HeadShotEffect, neckRagdoll.position, Quaternion.Euler(new Vector3(-90, 0, 0)), neckRagdoll) as GameObject;
        Destroy(bloodhs, 10f);
        FindObjectOfType<AudioManager>().Play("Headshot");
        State = AiState.Dead;
    }

    private void AddFroceOnDrop(GameObject instantiatedWeapon, float dropForwardForce, float dropUpwardForce)
    {
        Rigidbody rigidbody;
        rigidbody = instantiatedWeapon.GetComponent<Rigidbody>();

        rigidbody.AddForce(Camera.main.transform.forward * dropForwardForce, ForceMode.Impulse);
        rigidbody.AddForce(Camera.main.transform.up * dropUpwardForce, ForceMode.Impulse);
    }

    private void AnimIdle()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Walking", false);
        animator.SetBool("IsMoving", false);
        animator.SetBool("Shooting", false);
    }

    private void AnimWalk()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walking", true);
        animator.SetBool("IsMoving", true);
        animator.SetBool("Shooting", false);
    }

    private void AnimStandingShot()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walking", false);
        animator.SetBool("IsMoving", false);
        animator.SetBool("Shooting", true);
    }

    private void AnimeWalkingShot()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walking", false);
        animator.SetBool("IsMoving", true);
        animator.SetBool("Shooting", true);
    }

    public enum AiState
    {
        Patroling,
        Combat,
        Dead
    }
}
