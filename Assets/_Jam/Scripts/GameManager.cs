using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager instance;
    
    private List<Character> charactersInCommmunity = new List<Character>();

    public void Add(Character charactedtoAdd)
    {
        int communityCount = charactersInCommmunity.Count;
        if (communityCount >= 1)
        {
            charactedtoAdd.HoldHands(charactersInCommmunity[communityCount-1].GetHandToHold());
        }
        charactersInCommmunity.Add(charactedtoAdd);
    }
    
}
