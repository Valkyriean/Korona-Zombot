using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactDoorController : MonoBehaviour, Interactable
{
    [SerializeField] private DoorController door;
    [SerializeField] private ShowInstruction ins;
    [SerializeField] private int doorID = -1;
    public enum DoorAction {Open, Close, Change, None};
    public DoorAction doorAction;

    public void Interact(GameObject other) {
        if (doorID == -1 || ins.allowedDoorID > doorID) {
            if(doorAction == DoorAction.Open) door.Open();
            if(doorAction == DoorAction.Close) door.Close();
            if(doorAction == DoorAction.Change) door.Trigger();
            ins.showDash = true;
            ins.showInteract = false;
        }
    }
}
