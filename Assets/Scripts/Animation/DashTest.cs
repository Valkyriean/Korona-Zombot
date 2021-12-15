using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTest : MonoBehaviour
{
    [SerializeField] private DoorController door;
    [SerializeField] private ShowInstruction ins;
    [SerializeField] private bool enter;
    private float timer;
    private bool triggered = false;
    private void OnTriggerEnter(Collider other) {
        if (enter){
            triggered = true;   
            timer = 0;
        }
        if (!enter) ins.showDash = false;
    }

    void Update()
    {
        if (!enter) return;
        // Debug.Log(timer);
        if(timer >= 0.35f){
            door.Close();
            triggered = false;
            timer = 0;
        } else {
            if(triggered) timer += Time.deltaTime;
        }
    }
}
