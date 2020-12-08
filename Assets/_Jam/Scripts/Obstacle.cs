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

    [SerializeField] private AnimationCurve curve;
    
    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToAnotherPoint();
    }

    private void OnEnable()
    {
        GameManager.onCommunityChange += ChangeSize;
    }

    private void OnDisable()
    {
        GameManager.onCommunityChange -= ChangeSize;
    }

    private void FixedUpdate()
    { 
        if(GameManager.instance.isGameOver)
            return;
        Move();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(GameManager.instance.isGameOver)
            return;
        
        if (other.collider.tag == GameConstants.characterTag)
        {
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
       Vector3 direction = c.position - transform.position;

       float rotationSpeed = 4.0f;
        float singleStep = rotationSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);

        target = newDirection * -120f;

    }

    private void ChangeSize()
    {
        float communityCount = GameManager.instance.charactersInCommmunity.Count;
        
        float size = curve.Evaluate(communityCount / GameManager.instance.GetGoal());
        transform.localScale = new Vector3(size, size, size);
    }


}
