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

    [HideInInspector]
    public bool isGameOver = false;

    public UIManager ui;
    public GameObject selectedParticle;
    private void Awake()
    {
        if(instance != null)
            Destroy(this);
        instance = this;
        
        ui.SetGoalText(charactersInCommmunity.Count, communitySizeGoal);
        selectedParticle.SetActive(false);

    }

    private void WinCondition()
    {
        ui.SetGoalText(charactersInCommmunity.Count, communitySizeGoal);
        if (charactersInCommmunity.Count >= communitySizeGoal)
        {
            Debug.Log("Game Won");
            isGameOver = true;
            ui.ShowGameOver(true);
        }
    }

    public void SetAndActivateParticle(Vector3 position)
    {
        selectedParticle.transform.position = position;
        selectedParticle.SetActive(true);
    }

    public IEnumerator GoToPoint(Vector3 worldPoint)
    {
        int communityCount = charactersInCommmunity.Count;

        if (communityCount > 0)
        {
            charactersInCommmunity[communityCount-1].WalkToPoint(worldPoint, true);
            for (int i = communityCount-2; i >= 0 ; i--)
            {
                charactersInCommmunity[i].WalkTo(charactersInCommmunity[i+1], true);
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
                yield return  new WaitForSeconds(0.1f);
                charactersInCommmunity[i].WalkTo(charactersInCommmunity[i+1], true);
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
        if(!fromCharacter.IsOnCommunity())
            return;
        Debug.Log("<b>Gonna remove from community</b>");
        int index = -1;
        for (int i = 0; i < charactersInCommmunity.Count; i++)
        {
            if (fromCharacter == charactersInCommmunity[i])
            {
                index = i;
                //Remove from here
                charactersInCommmunity[i].Panicked();
                Debug.Log("<b>PANIC</b>" +  i);
                //todo REMOVE CHARACTERS
                break; //exit loop
            }
        }

        if (index >= 0)
        {
            charactersInCommmunity.RemoveRange(0, index);
        }
    }
    
    
}
