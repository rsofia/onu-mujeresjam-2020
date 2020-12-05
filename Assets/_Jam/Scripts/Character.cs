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
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        ikControl = GetComponentInChildren<CharacterIKControl>();
        Inactive();
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
        GameManager.instance.AddToCommunity(this);
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
    
    public void Select()
    {
        if(isCommunity)
            return;
        
        JoinCommunity();
    }
}
