using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactTask : MonoBehaviour, Interactable
{
    [SerializeField] private GameObject self;
    [SerializeField] private bool destoryOnInteract;
    [SerializeField] private int taskID;
    [SerializeField] private TaskController taskController;

    public void Interact(GameObject other) {
        if (taskController.taskProcess == taskID - 1){
            if (self && destoryOnInteract) Destroy(self);
            taskController.taskProcess = taskID;
        }
        
    }
}
