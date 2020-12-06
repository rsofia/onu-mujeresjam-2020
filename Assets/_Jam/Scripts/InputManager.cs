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
                Debug.Log("look at " + hit.point);
            }
        }
    }
    
}
