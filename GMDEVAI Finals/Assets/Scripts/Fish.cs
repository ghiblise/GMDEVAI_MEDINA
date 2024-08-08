using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBehaviour : MonoBehaviour
{
    GameObject[] waypoints;
    int currentWaypoint;
    public float speed = 5.0f;
    public float accuracy = 3.0f;
    public float rotSpeed = 1.0f;

    // waypoints similar to module 7 
    void Awake()
    {
        waypoints = GameObject.FindGameObjectsWithTag("waypoint");
    }

    void Start()
    {
        currentWaypoint = 0;
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        if (Vector3.Distance(waypoints[currentWaypoint].transform.position, gameObject.transform.position) < accuracy)
        {
        currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }

       var direction = waypoints[currentWaypoint].transform.position - gameObject.transform.position;
       gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
       gameObject.transform.Translate(0, 0, Time.deltaTime * speed);
    }
}
