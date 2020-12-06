using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private RaycastHit hit;
    public InputAction inputAction;

    private void Update()
    {
        //  if(playerInput.actions["Fire"].WasPressedThisFrame())
      if(Mouse.current.leftButton.wasPressedThisFrame)
            OnLeftClick();
    }

    public void OnLeftClick()
    {
        Debug.Log("Left click");
        if(GameManager.instance.isGameOver)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider.CompareTag(GameConstants.characterTag))
            {
                hit.collider.GetComponent<Character>().Select();
            }
            else if (hit.collider.CompareTag(GameConstants.floorTag))
            {
                StartCoroutine(GameManager.instance.GoToPoint(hit.point));
            }
        }
    }
    
}
