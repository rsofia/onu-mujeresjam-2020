using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterIKControl : MonoBehaviour
{
   private Animator animator;

   private bool ikActiveRight = false;
   private bool ikActiveLeft = false;
   private Transform rightHandObj = null;
   private Transform leftHandObj = null;
   [SerializeField][Range(0, 1)]private float positionWeight = 1.0f;
   [SerializeField][Range(0, 1)]private float rotationWeight = 0.5f;

   private void Awake()
   {
      animator = GetComponent<Animator>();
   }

   public void ActivateIKLeft(Transform hand)
   {
      leftHandObj = hand;
      ikActiveLeft = true;
   }

   public void ActivateIKRight(Transform hand)
   {
      rightHandObj = hand;
      ikActiveRight = true;
   }
   
   public void DeactivateIK()
   {
      ikActiveLeft = ikActiveRight = false;
   }
   
   private void OnAnimatorIK(int layerIndex)
   {
      if (animator)
      {
         if (ikActiveRight)
         {
            if (rightHandObj != null)
            {
               animator.SetIKPositionWeight(AvatarIKGoal.RightHand, positionWeight);
               animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rotationWeight);
               animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
               animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
            }
         }
         else
         {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
         }

         if (ikActiveLeft)
         {
            if (leftHandObj != null)
            {
               animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, positionWeight);
               animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, rotationWeight);
               animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
               animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
            } 
            else
            {
               animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
               animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
         }
      }
   }
}
