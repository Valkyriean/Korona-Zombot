using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartJumpStopIns : MonoBehaviour
{
    [SerializeField] private ShowInstruction ins;
    [SerializeField] private StartJumpIns jumpTrigger;
    [SerializeField] private HealthController HC;
    private void OnTriggerStay(Collider other) {
        ins.showJump = false;
        ins.showInteract = true;
        ins.canDash = false;
        HC.setHealth(75);
        jumpTrigger.InsDisable();
        Destroy(this);
    }
    
}
