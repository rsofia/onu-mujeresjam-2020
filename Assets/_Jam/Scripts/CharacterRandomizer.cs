using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRandomizer : MonoBehaviour
{
    public GameObject[] hair;
    public GameObject[] shirt;
    public GameObject[] pants;
    public GameObject[] feet;

    private void Start()
    {
        int index = 0;
        //Hair
        index  = UnityEngine.Random.Range(0, hair.Length);
        for (int i = 0; i < hair.Length; i++)
        {
            if(i != index)
                hair[i].SetActive(false);
            else
                hair[i].SetActive(true);
        }
        //shirt
        index  = UnityEngine.Random.Range(0, shirt.Length);
        for (int i = 0; i < shirt.Length; i++)
        {
            if(i != index)
                shirt[i].SetActive(false);
            else
                shirt[i].SetActive(true);
        }
        //pants
        index  = UnityEngine.Random.Range(0, pants.Length);
        for (int i = 0; i < pants.Length; i++)
        {
            if(i != index)
                pants[i].SetActive(false);
            else
                pants[i].SetActive(true);
        }
        //feet
        index  = UnityEngine.Random.Range(0, feet.Length);
        for (int i = 0; i < feet.Length; i++)
        {
            if(i != index)
                feet[i].SetActive(false);
            else
                feet[i].SetActive(true);
        }
    }
}
