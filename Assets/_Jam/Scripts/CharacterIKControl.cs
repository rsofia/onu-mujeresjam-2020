using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIKControl : MonoBehaviour
{
   public Animator animator;

   private bool ikActive = false;
   private Transform rightHandObj = null;

   private void Start()
   {
      animator.GetComponent<Animator>();
   }

   public void ActivateIK(Transform hand)
   {
      rightHandObj = hand;
      ikActive = true;
   }

   public void DeactivateIK()
   {
      ikActive = false;
   }
   
   private void OnAnimatorIK(int layerIndex)
   {
      if (animator)
      {
         if (ikActive)
         {
            if (rightHandObj != null)
            {
               animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
               animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
               animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
               animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
            }
         }
         else
         {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
         }
      }
   }
}
