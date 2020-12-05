using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private RaycastHit hit;
    public InputActionReference mouseInputReference;
    
    
    public void OnLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (Physics.Raycast(ray, out hit, 1000))//, GameConstants.characterMask))
        {
            if (hit.collider.CompareTag(GameConstants.characterTag))
            {
                hit.collider.GetComponent<Character>().Select();
            }
        }
    }
    
}
