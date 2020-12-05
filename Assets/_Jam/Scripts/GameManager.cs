using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private List<Character> charactersInCommmunity = new List<Character>();

    private void Awake()
    {
        if(instance != null)
            Destroy(this);
        instance = this;

    }

    public void AddToCommunity(Character characterToAdd)
    {
        int communityCount = charactersInCommmunity.Count;
        if (communityCount >= 1)
        {
            characterToAdd.HoldWithRightHand(charactersInCommmunity[communityCount-1].GetLeftHand());
            charactersInCommmunity[communityCount-1].HoldWithLeftHand(characterToAdd.GetRightHand());
        }
        charactersInCommmunity.Add(characterToAdd);
    }
    
}
