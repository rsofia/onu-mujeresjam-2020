using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject decalPrefab;
    
    private List<Character> charactersInCommmunity = new List<Character>();

    private void Awake()
    {
        if(instance != null)
            Destroy(this);
        instance = this;

    }

    public void AddToCommunity(Character characterToAdd)
    {
        
        Instantiate(decalPrefab, characterToAdd.transform.position + (Vector3.up * 1.8f), Quaternion.Euler(90, 0,0 ));
        
        int communityCount = charactersInCommmunity.Count;
        if (communityCount >= 1)
        {
            characterToAdd.HoldWithRightHand(charactersInCommmunity[communityCount-1].GetLeftHand());
            charactersInCommmunity[communityCount-1].HoldWithLeftHand(characterToAdd.GetRightHand());
        }
        charactersInCommmunity.Add(characterToAdd);
    }
    
}
