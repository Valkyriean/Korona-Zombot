using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour
{
    private Vector3 lastPos, thisPos, relate;
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = player.transform.position;
        relate = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        thisPos = player.transform.position;
        transform.position = (thisPos + lastPos) / 2 + relate;
        lastPos = thisPos;
    }
}
