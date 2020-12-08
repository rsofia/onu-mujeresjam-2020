using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    private bool isCommunity = false;
    [SerializeField] private ParticleSystem heartParticle;
    [SerializeField] private ParticleSystem[] confusedParticles;
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
    private bool movingWithPanic;
    private bool moveToCharacter;
    
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

    private void FixedUpdate()
    {
        Move();
    }

    private void Inactive()
    {
        isCommunity = false;
        ikControl.DeactivateIK();
        movingWithPanic = false;
        moveToCharacter = false;
        canMove = false;
        useNavMesh = false;
        if(!heartParticle.isStopped)
            heartParticle.Stop();
        foreach (var particle in confusedParticles)
        {
            particle.Stop();
        }
        target = null;
    }

    private void JoinCommunity()
    {
        isCommunity = true;
        heartParticle.Play();
        StartCoroutine(GameManager.instance.AddToCommunity(this));
    }

    public void Panicked()
    {
        //halt what its doing
        ikControl.DeactivateIK();
        StopMoving();

        //play particle
        foreach (var particle in confusedParticles)
        {
            particle.Play();
        }
        
        //Panic animation
        animator.SetBool("panic", true);

        //Move to random position
        Vector3 goal;
        RandomPointNavmesh.RandomPoint(transform.position, 5, agent.height*2,out goal);
        canMove = true;
        targetWorld = new Vector3(goal.x, 0, goal.z)*2;
        agent.isStopped = false;
        useNavMesh = true;
        movingWithPanic = true;
        moveToCharacter = false;
        hasPath = false;
        agent.SetDestination(targetWorld);
    }

    //reset variables from panic
    private void UnPanic()
    {
        isCommunity = false;
        movingWithPanic = false;
        animator.SetBool("panic", false);
        foreach (var particle in confusedParticles)
        {
            particle.Stop();
        }
        Inactive();
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
        if (canMove)
        {
            if (AtEndOfPath())
            {
                StopMoving();
            }
        }
        
        canMove = true;
        moveToCharacter = true;
        target = other.spotToHoldHands;
        if (_useNavMesh)
        {
           agent.SetDestination(target.position);
           useNavMesh = true;
           agent.isStopped = false;
           hasPath = false;
        }
        animator.SetFloat("speed", 1.0f);
        
      // slerpLookAt.LookAt(this.transform, other.transform);
    }

    public void WalkToPoint(Vector3 point, bool _useNavMesh)
    {
        moveToCharacter = false;
        if (canMove)
        {
            if (AtEndOfPath())
            {
                StopMoving();
            }
        }
        canMove = true;
        targetWorld = new Vector3(point.x, 0, point.z);
        if (_useNavMesh)
        {
            agent.SetDestination(targetWorld);
            useNavMesh = true;
            agent.isStopped = false;
            //hasPath = false;
        }
        animator.SetFloat("speed", 1.0f);
       // slerpLookAt.LookAt(transform, point);
        
    }
    

    private void Move()
    {
        if (canMove)
        {
            if (movingWithPanic)
                movingWithPanic = true;
            Vector3 goalPosition;
            if (moveToCharacter)
            {
                goalPosition = target.position;
            }
            else
                goalPosition = targetWorld;
                
            
            if (useNavMesh)
            {
                if (movingWithPanic == false)
                {
                    agent.SetDestination(goalPosition);
                    hasPath = false;
                }
                
                if (AtEndOfPath())
                {
                    StopMoving();
                    if (movingWithPanic)
                    {
                        UnPanic();
                    }
                    return;
                }
                
                if (Vector3.Distance(transform.position, agent.destination) < stoppingDistance)
                {
                    StopMoving();
                    if (movingWithPanic)
                    {
                        UnPanic();
                    }
                }
            }
            else
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, goalPosition, step);
                
                
                if (Vector3.Distance(transform.position, goalPosition) < stoppingDistance)
                {
                    StopMoving();
                    if (movingWithPanic)
                    {
                        UnPanic();
                    }
                }
            }
           
        }
        else
        {
            //patch - test
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.position) > stoppingDistance*4)
                {
                    canMove = true;
                    animator.SetFloat("speed", 1.0f);
                    moveToCharacter = true;
                    canMove = true;
                    agent.isStopped = false;
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
        animator.SetFloat("speed", 0.0f);
       // slerpLookAt.StopLookintAt();
        transform.localEulerAngles = new Vector3(0, 180f, 0);
       // agent.enabled = false;
        agent.isStopped = true;
        animator.transform.localEulerAngles = Vector3.zero;
        animator.transform.localPosition = Vector3.zero;
    }
    


#endregion
   
    public void Select()
    {
        if(isCommunity)
            return;
        JoinCommunity();
    }
}
