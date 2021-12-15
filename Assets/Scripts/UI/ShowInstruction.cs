using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInstruction : MonoBehaviour
{
    [SerializeField] private Image look, move, jump, interact, dash, lightA, heavy, block, gun, switchW, rifle, reload;
    public bool showLook, showMove, showJump, showInteract, showDash, showLight, showHeavy, showBlock, showGun, showSwitch, showRifle, showReload;
    public int allowedDoorID = 0;
    private float insAmount = 0;
    public bool canDash;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (showLook){
            insAmount += Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"));
            if (insAmount >= 60) {
                showLook = false;
                showMove = true;
                allowedDoorID = 2;
                insAmount = 0;
            }
        }
        

        updateInstruction();
    }

    
    
    
    
    void updateInstruction() {
        if (showLook) {
            look.enabled = true;
        } else {
            look.enabled = false;
        }
        if (showMove) {
            move.enabled = true;
        } else {
            move.enabled = false;
        }
        if (showJump) {
            jump.enabled = true;
        } else {
            jump.enabled = false;
        }
        if (showInteract) {
            interact.enabled = true;
        } else {
            interact.enabled = false;
        }
        if (showDash) {
            dash.enabled = true;
        } else {
            dash.enabled = false;
        }
        if (showLight) {
            lightA.enabled = true;
        } else {
            lightA.enabled = false;
        }
        if (showHeavy) {
            heavy.enabled = true;
        } else {
            heavy.enabled = false;
        }
        if (showBlock) {
            block.enabled = true;
        } else {
            block.enabled = false;
        }
        if (showGun) {
            gun.enabled = true;
        } else {
            gun.enabled = false;
        }
        if (showSwitch) {
            switchW.enabled = true;
        } else {
            switchW.enabled = false;
        }
        if (showRifle) {
            rifle.enabled = true;
        } else {
            rifle.enabled = false;
        }
        if (showReload) {
            reload.enabled = true;
        } else {
            reload.enabled = false;
        }
        
    }
}
