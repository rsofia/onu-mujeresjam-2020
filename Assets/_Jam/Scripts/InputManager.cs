using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private RaycastHit hit;
    public InputAction inputAction;
	public AudioManager fxPlayer;
	
	private void Start()
	{
		fxPlayer = FindObjectOfType<AudioManager>();
	}
	
    private void Update()
	{			
    	if(Mouse.current.leftButton.wasPressedThisFrame)
            OnLeftClick();
    }

    public void OnLeftClick()
	{   
		
		
		
		
		if(GameManager.instance.isGameOver)
			return;
		
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (Physics.Raycast(ray, out hit, 1500))
        {
            if (hit.collider.CompareTag(GameConstants.characterTag))
            {
            	if(!fxPlayer.fxPlayer.isPlaying)
            	{
	            	fxPlayer.fxPlayer.clip = fxPlayer.soundFX[0];
	            	fxPlayer.fxPlayer.Play();
            	}
            	
            	print("Here " + GameManager.instance.isGameOver);
                hit.collider.GetComponent<Character>().Select();
                GameManager.instance.SetAndActivateParticle(hit.collider.transform.position);
            }
            else if (hit.collider.CompareTag(GameConstants.floorTag))
            {
            	if(!fxPlayer.fxPlayer.isPlaying)
            	{
	            	fxPlayer.fxPlayer.clip = fxPlayer.soundFX[2];
	            	fxPlayer.fxPlayer.Play();
            	}
            	
                GameManager.instance.SetAndActivateParticle(hit.point);
                StartCoroutine(GameManager.instance.GoToPoint(hit.point));
            }
        }
    }
    
}
