using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	//0 Catch, 1 Fail, 2 click
	public AudioClip [] soundFX;
	public AudioSource fxPlayer;
	
    // Start is called before the first frame update
    void Start()
    {
	    fxPlayer = GetComponent<AudioSource>();
    }
}
