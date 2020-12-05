using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PanAndZoom : MonoBehaviour
{
    [SerializeField] private float panSpeed = 2f;
    [SerializeField] private float zoomSpeed = 3f;
    [SerializeField] private float zoomInMax = 40f;
    [SerializeField] private float zoomOutMax = 90f;
    [SerializeField] Vector2 panlimit;
    [SerializeField] [Range(0, 1)] private float screenPercentage = 0.05f;
    
    private CinemachineInputProvider inputProvider;
    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;

    private void Awake()
    {
        inputProvider = GetComponent<CinemachineInputProvider>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
    }

    private void Update()
    {
        float x = inputProvider.GetAxisValue(0);
        float y = inputProvider.GetAxisValue(1);
        float z = inputProvider.GetAxisValue(2);
        
        if(x != 0 || y != 0)
            PanScreen(x, y);
        
        if(z != 0)
            ZoomScreen(z);
    }

    public Vector2 PanDirection(float x, float y)
    {
      Vector2 direction = Vector2.zero;
      if (y >= Screen.height * (1 - screenPercentage)) //top 5% of the screen
      {
          direction.y += 1;
      }
      else if (y <= Screen.height * screenPercentage)
      {
          direction.y -= 1;
      }

      //width
      if (x >= Screen.width * (1 - screenPercentage))
      {
          direction.x += 1;
      }
      else if (x <= Screen.width * screenPercentage)
      {
          direction.x -= 1;
      }

      return direction;

    }

    void PanScreen(float x, float y)
    {
        Vector2 direction = PanDirection(x, y);
        
        if(direction == Vector2.zero)
            return;

        Vector3 target = new Vector3(cameraTransform.position.x + direction.x*panSpeed, 
                                            cameraTransform.position.y, 
                                   cameraTransform.position.z + direction.y *panSpeed);

       target.x = Mathf.Clamp(target.x, -panlimit.x, panlimit.x);
       target.z = Mathf.Clamp(target.z, -panlimit.y, panlimit.y);
       
        cameraTransform.position = Vector3.Lerp(cameraTransform.position,
            target, Time.deltaTime);
        

      
    }

    public void ZoomScreen(float increment)
    {
        float fov = virtualCamera.m_Lens.FieldOfView;
        float target = Mathf.Clamp(fov + increment, zoomInMax, zoomOutMax);
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, target, zoomSpeed * Time.deltaTime);
    }
}
