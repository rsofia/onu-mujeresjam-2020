using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        ikControl = GetComponentInChildren<CharacterIKControl>();
        Inactive();
    }

    private void Update()
    {
        if (canMove && target)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            if (Vector3.Distance(transform.position, target.position) < stoppingDistance)
            {
                StopMoving();
            }
        }
    }

    private void Inactive()
    {
        isCommunity = false;
        darkness.SetActive(true);
        ikControl.DeactivateIK();
    }

    private void JoinCommunity()
    {
        isCommunity = true;
        darkness.SetActive(false);
        StartCoroutine(GameManager.instance.AddToCommunity(this));
    }

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

    public void WalkTo(Character other)
    {
        //todo walk to
        canMove = true;
        target = other.spotToHoldHands;
        animator.SetFloat("speed", 1.0f);
        transform.LookAt(target);
    }

    public void StopMoving()
    {
        canMove = false;
        target = null;
        animator.SetFloat("speed", 0.0f);
        transform.localEulerAngles = new Vector3(0, 180f, 0);
    }
    
    public void Select()
    {
        if(isCommunity)
            return;
        
        JoinCommunity();
    }
}
