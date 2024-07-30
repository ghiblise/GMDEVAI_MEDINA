using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    GameObject[] agents;
    GameObject player;

    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("AI");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // if (Input.GetMouseButton(0))
        // {
        //     RaycastHit hit;

        //     if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
        //     {
        //         foreach (GameObject ai in agents)
        //         {
        //             ai.GetComponent<AIControl>().agent.SetDestination(hit.point);
        //         }
        //     }
        // }

        foreach (GameObject ai in agents)
        {
            ai.GetComponent<AIControl>().agent.SetDestination(player.transform.position);
        }
    }
}
