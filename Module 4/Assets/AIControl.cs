using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    void Start()
    {
        this.agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
}
