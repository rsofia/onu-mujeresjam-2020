using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int communitySizeGoal = 10;
    
    private List<Character> charactersInCommmunity = new List<Character>();

    public bool isGameOver = false;
    
    private void Awake()
    {
        if(instance != null)
            Destroy(this);
        instance = this;

    }

    private void WinCondition()
    {
        if (charactersInCommmunity.Count >= communitySizeGoal)
        {
            Debug.Log("Game Won");
            isGameOver = true;
        }
    }

    public IEnumerator GoToPoint(Vector3 worldPoint)
    {
        int communityCount = charactersInCommmunity.Count;

        if (communityCount > 0)
        {
            charactersInCommmunity[communityCount-1].WalkToPoint(worldPoint, true);
            for (int i = communityCount-2; i >= 0 ; i--)
            {
                charactersInCommmunity[i].WalkTo(charactersInCommmunity[i+1], false);
                yield return  new WaitForSeconds(0.1f);
            }
        }
        
        
    }

    public IEnumerator AddToCommunity(Character characterToAdd)
    {
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
        WinCondition();

    }

    public void RemoveCommunity(Character fromCharacter)
    {
        for (int i = 0; i < charactersInCommmunity.Count; i++)
        {
            if (fromCharacter == charactersInCommmunity[i])
            {
                //Remove from here
                charactersInCommmunity[i].Panicked();
                charactersInCommmunity.RemoveAt(i);
                //todo REMOVE CHARACTERS
                break; //exit loop
            }
        }
    }
    
}
