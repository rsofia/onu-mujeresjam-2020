using System;
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

    //Movement
    [Header("Movement")]
    private bool canMove = false;
    private Transform target;
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
        Inactive();
    }

    private void Update()
    {
        if (GameManager.instance.isGameOver)
        {
            if(canMove)
                StopMoving();
            
            return;
        }
        
        Move();
    }

    private void Inactive()
    {
        isCommunity = false;
        darkness.SetActive(true);
        ikControl.DeactivateIK();
        agent.enabled = false;
        GetComponent<BoxCollider>().enabled = true;
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
    public void WalkTo(Character other, bool useNavmesh)
    {
        //todo walk to
        canMove = true;
        if (useNavmesh)
        {
            agent.enabled = true;
            agent.stoppingDistance = stoppingDistance;
        }
        target = other.spotToHoldHands;
        animator.SetFloat("speed", 1.0f);
        transform.LookAt(other.transform);
    }

    private void Move()
    {
        if (canMove && target)
        {
            if (useNavMesh)
            {
                agent.SetDestination(target.position);
                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            // Done
                            StopMoving();
                        }
                    }
                }
            }
            else
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                
                transform.LookAt(target);
                if (Vector3.Distance(transform.position, target.position) < stoppingDistance)
                {
                    StopMoving();
                }
            }
           
        }
    }
    
    public void StopMoving()
    {
        canMove = false;
        target = null;
        animator.SetFloat("speed", 0.0f);
        transform.localEulerAngles = new Vector3(0, 180f, 0);
        
        agent.enabled = false;
    }
    


#endregion
   
    public void Select()
    {
        if(isCommunity)
            return;
        GetComponent<BoxCollider>().enabled = false;
        JoinCommunity();
    }
}
