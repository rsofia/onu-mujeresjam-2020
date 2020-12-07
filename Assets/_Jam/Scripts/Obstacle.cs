using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Obstacle : MonoBehaviour
{
    private Vector3 target;
    [SerializeField] private float range = 20f;
    [SerializeField] private float maxDistance = 15f;
    private NavMeshAgent agent;
    private bool hasPath;
    private float stoppingDistance = 0.1f;
    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToAnotherPoint();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("collided with " + other.gameObject.name);
        if (other.collider.tag == GameConstants.characterTag)
        {
            Debug.Log("Collided with a character");
            GameManager.instance.RemoveCommunity(other.collider.GetComponent<Character>());
            GoOppositeDirection(other.collider.transform);
        }
    }

    private void Move()
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            GoToAnotherPoint();
            return;
        }
            
        
        if (AtEndOfPath())
        {
            GoToAnotherPoint();
            return;
        }
                
        if (Vector3.Distance(transform.position, agent.destination) < stoppingDistance)
        {
            GoToAnotherPoint();
        }
    }
    
    bool AtEndOfPath()
    {
        hasPath |= agent.hasPath;
        if (hasPath && agent.remainingDistance <= agent.stoppingDistance + stoppingDistance )
        {
            // Arrived
            hasPath = false;
            return true;
        }
 
        return false;
    }

    private void GoToAnotherPoint()
    {
        RandomPointNavmesh.RandomPoint(transform.position, range, maxDistance, out target);
        agent.SetDestination(target);
        hasPath = false;
             
    }

    private void GoOppositeDirection(Transform c)
    {
      // Vector3 direction = (new Vector3(c.transform.position.x, 0, c.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
            
        // Determine which direction to rotate towards
       Vector3 direction = c.position - transform.position;

       float rotationSpeed = 4.0f;
        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);

        target = newDirection * -120f;

        // float angle;
        // Vector3 axis = Vector3.up;
        // Quaternion.LookRotation(newDirection).ToAngleAxis(out angle, out axis);
        // transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

}
