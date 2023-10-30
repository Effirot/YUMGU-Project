using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTarget : MonoBehaviour
{
    public void SetDestinationToAgent(NavMeshAgent agent)
    {
        agent.destination = transform.position;
    }
}
