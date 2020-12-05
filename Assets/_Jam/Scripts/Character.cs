using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private bool isCommunity = false;
    [SerializeField] private GameObject darkness;
    [SerializeField] private Transform handToHold;

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
    }

    public void HoldHands(Transform target)
    {
        ikControl.ActivateIK(target);
    }

    public Transform GetHandToHold()
    {
        return handToHold;
    }
    
    public void Select()
    {
        JoinCommunity();
    }
}
