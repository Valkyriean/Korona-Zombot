using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePivot : MonoBehaviour
{
    public Vector3 front, back, up, down, right, left;
    [SerializeField] private GameObject point0, pointFront, pointUp, pointRight;

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        front = Vector3.Normalize(pointFront.transform.position - point0.transform.position);
        back = -front;
        up = Vector3.Normalize(pointUp.transform.position - point0.transform.position);
        down = -up;
        right = Vector3.Normalize(pointRight.transform.position - point0.transform.position);
        left = -right;
    }
}
