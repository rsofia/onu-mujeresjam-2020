using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpLookAt : MonoBehaviour
{
    private Vector3 target;
    private Transform targetTransform;
    private Transform objToRotate;
    private float rotationSpeed;
 
    private Quaternion lookAtRotation;
    private Vector3 direction;
    private bool isOnSlerpVector = false;
    private bool isOnSlerpTransform = false;

    public void LookAt(Transform objToRotate, Vector3 target, float rotationSpeed = 3.0f)
    {
        
        this.target = target;
        this.rotationSpeed = rotationSpeed;
        this.objToRotate = objToRotate;
        isOnSlerpVector = true;
        isOnSlerpTransform = false;
    }

    public void StopLookintAt()
    {
        isOnSlerpTransform = false;
        isOnSlerpVector = false;
    }
    
    public void LookAt(Transform objToRotate, Transform target, float rotationSpeed = 3.0f)
    {
        this.targetTransform = target;
        this.rotationSpeed = rotationSpeed;
        this.objToRotate = objToRotate;
        isOnSlerpTransform = true;
        isOnSlerpVector = false;

    }
    
    void Update()
    {
        if (isOnSlerpVector)
        {
            //direction = (new Vector3(target.x, 0, target.z) - new Vector3(objToRotate.position.x, 0, objToRotate.position.z)).normalized;
            
            // Determine which direction to rotate towards
            direction = target - objToRotate.position;

            // The step size is equal to speed times frame time.
            float singleStep = rotationSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);

            //Debug.DrawRay(transform.position, newDirection, Color.red);

            float angle;
            Vector3 axis = Vector3.up;
            Quaternion.LookRotation(newDirection).ToAngleAxis(out angle, out axis);
            objToRotate.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        }
        else if (isOnSlerpTransform)
        {
           // Determine which direction to rotate towards
           direction = targetTransform.position - objToRotate.position;

           // The step size is equal to speed times frame time.
           float singleStep = rotationSpeed * Time.deltaTime;

           // Rotate the forward vector towards the target direction by one step
           Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);


           float angle;
           Vector3 axis = Vector3.up;
           Quaternion.LookRotation(newDirection).ToAngleAxis(out angle, out axis);
           objToRotate.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            
        }
    }
}
