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
    
    public List<Character> charactersInCommmunity = new List<Character>();

    [HideInInspector]
    public bool isGameOver = false;

    public UIManager ui;
    [SerializeField] private ParticleSystem selectedParticle;
    private void Awake()
    {
        if(instance != null)
            Destroy(this);
        instance = this;
        
        ui.SetGoalText(charactersInCommmunity.Count, communitySizeGoal);

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
        selectedParticle.transform.position = new Vector3(position.x, 0.1f, position.z);
        if(selectedParticle.isStopped)
            selectedParticle.Play();
    }

    public IEnumerator GoToPoint(Vector3 worldPoint)
    {
        int communityCount = charactersInCommmunity.Count;

        if (communityCount > 0)
        {
            charactersInCommmunity[communityCount-1].WalkToPoint(worldPoint, true);
            for (int i = communityCount-2; i >= 0 ; i--)
            {
                try
                {
                    charactersInCommmunity[i].WalkTo(charactersInCommmunity[i+1], true);
                }
                catch (Exception e)
                {
                    Console.WriteLine("unfortunate");
                    throw;
                }
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
        if (!fromCharacter.IsOnCommunity())
        {
            fromCharacter.Panicked();
            return;;
        }
        int index = -1;
        for (int i = 0; i < charactersInCommmunity.Count; i++)
        {
            if (fromCharacter == charactersInCommmunity[i])
            {
                //get index to remove
                index = i;
                break; //exit loop
            }
        }

        if (index >= 0)
        {
            for (int c = 0; c <= index; c++)
            {
                
                charactersInCommmunity[c].Panicked();
            }
            charactersInCommmunity.RemoveRange(0, index+1);
        }
        
        ui.SetGoalText(charactersInCommmunity.Count, communitySizeGoal);
    }
    
    
}
