using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
	//public GameObject decalPrefab;
    
    private List<Character> charactersInCommmunity = new List<Character>();

    private void Awake()
    {
        if(instance != null)
            Destroy(this);
        instance = this;

    }

    public IEnumerator AddToCommunity(Character characterToAdd)
    {
        //Place shadow so at the end it reads something
	    // Instantiate(decalPrefab, characterToAdd.transform.position + (Vector3.up * 1.8f), Quaternion.Euler(90, 0,0 ));
        
        //Add the character
        int communityCount = charactersInCommmunity.Count;
        if (communityCount >= 1)
        {
            //Last character walks up to the new one
            charactersInCommmunity[communityCount-1].WalkTo(characterToAdd, true);
            
            for (int i = communityCount-2; i >= 0 ; i--)
            {
                yield return  new WaitForSeconds(0.2f);
                charactersInCommmunity[i].WalkTo(charactersInCommmunity[i+1], false);
            }
            
            //Make them hold hands with each other
            characterToAdd.HoldWithRightHand(charactersInCommmunity[communityCount-1].GetLeftHand());
            charactersInCommmunity[communityCount-1].HoldWithLeftHand(characterToAdd.GetRightHand());
        }
        charactersInCommmunity.Add(characterToAdd);
        
       
      
    }
    
}
