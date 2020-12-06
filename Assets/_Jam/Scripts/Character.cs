﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    private bool isCommunity = false;
    [SerializeField] private GameObject darkness;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    private Animator animator;
    private CharacterIKControl ikControl;
    private SlerpLookAt slerpLookAt;

    //Movement
    [Header("Movement")]
    private bool canMove = false;
    private Transform target;
    protected Vector3 targetWorld;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float stoppingDistance = 0.5f;
    public Transform spotToHoldHands;
    private NavMeshAgent agent;
    private bool useNavMesh;
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        ikControl = GetComponentInChildren<CharacterIKControl>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        agent.speed = speed;
        slerpLookAt = GetComponent<SlerpLookAt>();
        Inactive();
    }

    private void Update()
    {
        // if (GameManager.instance.isGameOver)
        // {
        //     if(canMove)
        //         StopMoving();
        //     
        //     return;
        // }
        
        Move();
    }

    private void Inactive()
    {
        isCommunity = false;
        darkness.SetActive(true);
        ikControl.DeactivateIK();
        agent.enabled = false;
    }

    private void JoinCommunity()
    {
        isCommunity = true;
        darkness.SetActive(false);
        StartCoroutine(GameManager.instance.AddToCommunity(this));
    }

    public void Panicked()
    {
        isCommunity = false;
        ikControl.DeactivateIK();
        //TODO FINISH PACKICKED
        
    }

    public bool IsOnCommunity()
    {
        return isCommunity;
    }

    #region hold hands
    public void HoldWithRightHand(Transform left)
    {
        ikControl.ActivateIKRight(left);
    }
    
    public void HoldWithLeftHand(Transform right)
    {
        ikControl.ActivateIKLeft(right);
    }

    public Transform GetLeftHand()
    {
        return leftHand;
    }

    public Transform GetRightHand()
    {
        return rightHand;
    }
#endregion

    #region Movement
    public void WalkTo(Character other, bool _useNavMesh)
    {
        if(canMove)
            return;
        
        
        canMove = true;
        if (_useNavMesh)
        {
            agent.enabled = true;
            this.useNavMesh = _useNavMesh;
        }
        target = other.spotToHoldHands;
        animator.SetFloat("speed", 1.0f);
        
       slerpLookAt.LookAt(this.transform, other.transform);
    }

    public void WalkToPoint(Vector3 point, bool _useNavMesh)
    {
        if(canMove)
            return;
        
        canMove = true;
        targetWorld = point;
        if (_useNavMesh)
        {
            agent.enabled = true;
            useNavMesh = _useNavMesh;
        }
        animator.SetFloat("speed", 1.0f);
        slerpLookAt.LookAt(transform, point);
        
    }

    private void Move()
    {
        if (canMove)
        {
            Vector3 goalPosition;
            if (target == null)
                goalPosition = targetWorld;
            else
                goalPosition = target.position;
            
            if (useNavMesh)
            {
                if (goalPosition != agent.destination)
                {
                    agent.SetDestination(goalPosition);
                    hasPath = false;
                }
                
                if (AtEndOfPath())
                {
                    agent.isStopped = true;
                    StopMoving();
                    return;
                }
                
                if (Vector3.Distance(transform.position, agent.destination) < stoppingDistance)
                {
                    agent.isStopped = true;
                    StopMoving();
                }
                
               // if (!agent.pathPending)
               // {
                    // Debug.Log(gameObject.name + " still has path pending");
                    // if (agent.remainingDistance <= agent.stoppingDistance)
                    // {
                    //     Debug.Log(gameObject.name + " remaining distance accomplished" + agent.remainingDistance + " stopping " + agent.stoppingDistance);
                    //     if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.2f)
                    //     {
                    //         agent.isStopped = true;
                    //         Debug.Log(gameObject.name + " stop");
                    //         StopMoving();
                    //     }
                    // }
                //}
            }
            else
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, goalPosition, step);
                
                if(target != null)
                    slerpLookAt.LookAt(this.transform, target);
                
                if (Vector3.Distance(transform.position, goalPosition) < stoppingDistance)
                {
                    StopMoving();
                }
            }
           
        }
    }
    
    public float pathEndThreshold = 0.1f;
    private bool hasPath = false;
    bool AtEndOfPath()
    {
        hasPath |= agent.hasPath;
        if (hasPath && agent.remainingDistance <= agent.stoppingDistance + pathEndThreshold )
        {
            // Arrived
            hasPath = false;
            return true;
        }
 
        return false;
    }
    
    public void StopMoving()
    {
        canMove = false;
        target = null;
        animator.SetFloat("speed", 0.0f);
        slerpLookAt.StopLookintAt();
        transform.localEulerAngles = new Vector3(0, 180f, 0);
        agent.enabled = false;
        useNavMesh = false;
        animator.transform.localEulerAngles = Vector3.zero;
    }
    


#endregion
   
    public void Select()
    {
        if(isCommunity)
            return;
        //GetComponent<BoxCollider>().enabled = false;
        JoinCommunity();
    }
}
