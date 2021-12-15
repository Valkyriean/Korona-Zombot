using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartJumpIns : MonoBehaviour
{
    [SerializeField] private ShowInstruction ins;
    private bool insEnabled = true;
    private void OnTriggerStay(Collider other) {
        ins.showMove = false;
        if (insEnabled) ins.showJump = true;
    }
    public void InsDisable() {
        insEnabled = false;
    }
}
