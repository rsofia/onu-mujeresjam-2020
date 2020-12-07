using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("collided with " + other.gameObject.name);
        if (other.collider.tag == GameConstants.characterTag)
        {
            Debug.Log("Collided with a character");
            GameManager.instance.RemoveCommunity(other.collider.GetComponent<Character>());
        }
    }

}
