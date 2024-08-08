using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehaviour : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject player;
    public PlayerMovement playerMovement;
    Vector3 wanderTarget;
    GameObject[] goalLocations;
    GameObject[] food;
    public TextMeshProUGUI animalStatus;

    float detectionRadius = 10f;

    public bool likesPlayer = true;
    public bool followingPlayer = false;
    public bool captured = false;
    public bool grazing = false;
    public bool willEatFood = false;

    public enum BehaviourType {   Runs, Hides, NotScared  }
    public BehaviourType behaviourType;
    public GameObject likedFood;

    //vector motion from module 1 + module 4 navmesh + module 5 pathfinding
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        playerMovement = player.GetComponent<PlayerMovement>();
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
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
        Vector3 targetDirection = player.transform.position - this.transform.position;
        float lookAhead = targetDirection.magnitude/(agent.speed + playerMovement.speed);
        Flee(player.transform.position + player.transform.forward * lookAhead);
        UpdateStatus("Running");
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
        UpdateStatus("Wandering");
    }

    void CleverHide()
    {
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGameObject = World.Instance.GetHidingSpots()[0];

        int hidingSpotsCount = World.Instance.GetHidingSpots().Length;

        for (int i = 0; i < hidingSpotsCount; i++)
        {
            Vector3 hideDirection = World.Instance.GetHidingSpots()[i].transform.position - player.transform.position;
            Vector3 hidePosition = World.Instance.GetHidingSpots()[i].transform.position + hideDirection.normalized * 5;

            float spotDistance = Vector3.Distance(this.transform.position, hidePosition);
            if (spotDistance < distance)
            {
                chosenSpot = hidePosition;
                chosenDir = hideDirection;
                chosenGameObject = World.Instance.GetHidingSpots()[i];
                distance = spotDistance;
            }
        }

        Collider hideCol = chosenGameObject.GetComponent<Collider>();
        Ray back = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float rayDistance = 100.0f;
        hideCol.Raycast(back, out info, rayDistance);

        Seek(info.point + chosenDir.normalized * 5);

        UpdateStatus("Hiding");
    }

    // module 6 flocking
    void Graze()
    {
        Seek(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        grazing = true;
    }

    void DetectFood()
    {
        foreach (GameObject f in food)
        {
            if (Vector3.Distance(f.transform.position, this.transform.position) < detectionRadius 
                && likedFood.GetComponent<MeshCollider>() == f.gameObject.GetComponent<MeshCollider>() && !willEatFood)
            {
                willEatFood = true;
                Seek(f.transform.position);
                UpdateStatus("Eating Food");
            }
        }
    }

    bool CanSeePlayer()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = player.transform.position - this.transform.position;
        if (Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
            return raycastInfo.transform.gameObject.tag == "Player";
        return false;
    }

    bool IsAnimalValid()
    {
        if (followingPlayer || captured || willEatFood)
            return false;
        else
            return true;
    }

    void Update()
    {
        food = GameObject.FindGameObjectsWithTag("food");

        MainBehaviour();
        
        // follow player code
        if (followingPlayer && !captured)
        {
            Seek(player.transform.position);
            UpdateStatus("Captured");
        }     
        else if (captured && !grazing)
        {
            followingPlayer = false;
            Graze();          
        }

        if (agent.remainingDistance < 1)
                grazing = false;
    }
    
    void OnCollisionEnter(Collision col)
    {
        if (!captured && likesPlayer && col.gameObject.tag == "Player")
            followingPlayer = true;

        if (col.gameObject.tag == "food")
        {
            Destroy(col.gameObject);
            likesPlayer = true;
            willEatFood = false;
            UpdateStatus("Likes Player");
        } 
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "trigger")
        {
            followingPlayer = false;
            captured = true;
            agent.speed = 5f;
            player.GetComponent<PlayerMovement>().animalsCaught++;
            player.GetComponent<PlayerMovement>().UpdateAnimalCountDisp();
            UpdateStatus("Captured");
        }
    }

    void MainBehaviour()
    {
        if (CanSeePlayer() && IsAnimalValid())
        {
            switch (behaviourType)
            {
                case BehaviourType.Hides:
                CleverHide();
                break;

                case BehaviourType.Runs:
                Evade();
                break;
                
                case BehaviourType.NotScared:
                Wander();
                break;
            }
        }
        else if (!CanSeePlayer() && IsAnimalValid())
        {
            Wander();
            DetectFood();
        }       
    }

    void UpdateStatus(string status)
    {
        animalStatus.text = gameObject.name + " " + status;
    }
}