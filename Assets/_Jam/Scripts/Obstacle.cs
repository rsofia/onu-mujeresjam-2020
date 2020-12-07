using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Obstacle : MonoBehaviour
{
    private Vector3 target;
    private float range = 20f;
    private NavMeshAgent agent;
    private bool hasPath;
    private float pathEndThreshold = 0.1f;
    private float stoppingDistance = 0.1f;
    
    [SerializeField] private float speed = 1.5f;

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
            GoToAnotherPoint();
        }
    }

    private void Move()
    {
       
                
        if (AtEndOfPath())
        {
            agent.isStopped = true;
            GoToAnotherPoint();
            return;
        }
                
        if (Vector3.Distance(transform.position, agent.destination) < stoppingDistance)
        {
            agent.isStopped = true;
            GoToAnotherPoint();
        }
    }
    
    bool AtEndOfPath()
    {
        hasPath |= agent.hasPath;
        if (hasPath && agent.remainingDistance <= agent.stoppingDistance + pathEndThreshold )
        {
            // Arrived
            hasPath = false;
            return true;
        }
 
        return false;
    }

    private void GoToAnotherPoint()
    {
        RandomPointNavmesh.RandomPoint(transform.position, range, out target);
        agent.SetDestination(target);
        hasPath = false;
             
    }

}
