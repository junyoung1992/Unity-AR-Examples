using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class NavCharacterController : CharacterController
{
    private NavMeshAgent _navMeshAgent;

    // Start 함수보다 먼저 실행되는 함수
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected override void MoveTo(Vector3 target)
    {
        var objs = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (var o in objs)
        {
            o.GetComponentInChildren<NavMeshSurface>().BuildNavMesh();
        }

        _navMeshAgent.destination = target;
    }
}
