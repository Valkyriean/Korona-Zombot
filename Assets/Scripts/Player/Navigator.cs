using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform wayPoint;

    // Update is called once per frame
    void Update()
    {
        wayPoint = GameObject.Find("WayPoint").transform;
        if (wayPoint) {
            agent.SetDestination(wayPoint.position);
        }
    }
}
 