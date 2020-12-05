using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public TextAsset level;
    public GameObject characterPrefab;
    private int limitX, limitY;
    public Transform characterParent;
    
    private void Awake()
    {
        ReadLevel();
    }

    public void ReadLevel()
    {
        int x = 0;
        int y = 0;
        foreach (char character in level.text)
        {
            if (character == '\n')
            {
                limitX = x;
                x = 0;
                y++;
            }
            
            x++;
            if (character == '1')
            {
                GameObject obj = Instantiate(characterPrefab, characterParent);
                obj.transform.position = new Vector3(x, 0, -y);
            }
        }

        limitY = y;
    }
}
