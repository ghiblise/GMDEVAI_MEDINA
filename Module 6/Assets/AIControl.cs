using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator animator;
    float speedMultiplier;
    float detectionRadius = 10;
    float fleeRadius = 10;

    void ResetAgent()
    {
        speedMultiplier = Random.Range(0.1f, 1.5f);
        agent.speed = 2 * speedMultiplier;
        agent.angularSpeed = 120;
        animator.SetFloat("speedMultiplier", speedMultiplier);
        animator.SetTrigger("isWalking");
        agent.ResetPath();
    }

    void Start()
    {
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();

        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        animator.SetFloat("wOffset", Random.Range(0.1f, 1f));

        ResetAgent();
    }

    void Update()
    {
        if (agent.remainingDistance < 1)
        {
            ResetAgent();
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }
    }

    public void DetectNewMonster(Vector3 location)
    {
        if (Vector3.Distance(location, this.transform.position) < detectionRadius)
        {
            Vector3 fleeDirection = (this.transform.position - location).normalized;
            Vector3 newGoal = this.transform.position + fleeDirection * fleeRadius;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newGoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                animator.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }

    public void DetectNewLandmark(Vector3 location)
    {
        if (Vector3.Distance(location, this.transform.position) < detectionRadius)
        {
            agent.SetDestination(location);
            animator.SetTrigger("isRunning");
            agent.speed = 10;
            agent.angularSpeed = 500;
        }
    }
}
