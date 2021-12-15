using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPivot : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform origin;

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = (new Vector3(pivot.localPosition.x, 0f, pivot.localPosition.z)).normalized * 0.001f;
        this.transform.LookAt(origin);
        this.transform.localPosition += new Vector3(0f, 0.2f, 0f);
        pivot.localPosition = new Vector3(0f, 0f, 0f);
    }
}
