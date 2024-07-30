using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : NPCBaseFSM
{
    GameObject[] waypoints;
    GameObject player;
    GameObject farthestWaypoint;
    float longestDist;

    void Awake()
    {
        waypoints = GameObject.FindGameObjectsWithTag("waypoint");
        player = GameObject.FindGameObjectWithTag("Player");
        longestDist = Vector3.Distance(waypoints[0].transform.position, player.transform.position);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (waypoints.Length == 0) return;

        foreach (GameObject w in waypoints)
        {
            if (w != farthestWaypoint)
            {
                float distance = Vector3.Distance(w.transform.position, player.transform.position);

                if (distance > longestDist)
                {
                    longestDist = distance;
                    farthestWaypoint = w;
                }
            }
        }

        var direction = farthestWaypoint.transform.position - NPC.transform.position;
        NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
        NPC.transform.Translate(0, 0, Time.deltaTime * speed);
    }
}
