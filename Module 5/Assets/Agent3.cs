using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent3 : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public WASDMovement playerMovement;
    Vector3 wanderTarget;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        playerMovement = target.GetComponent<WASDMovement>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeDirection = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeDirection);
    }

    void Evade()
    {
        Vector3 targetDirection = target.transform.position - this.transform.position;

        float lookAhead = targetDirection.magnitude/(agent.speed + playerMovement.currentSpeed);

        Flee(target.transform.position + target.transform.forward * lookAhead);
    }
    
    void Wander()
    {
        float wanderRadius = 20;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    bool canSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - this.transform.position;
        if (Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        {
            return raycastInfo.transform.gameObject.tag == "Player";
        }
        return false;
    }

    void Update()
    {
        if (canSeeTarget())
        {
            Evade();
        }
        else 
            Wander();
    }
}
