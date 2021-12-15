using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private DoorController door;
    [SerializeField] private EnemyAI enemy;
    [SerializeField] private Transform player;
    [SerializeField] private ShowInstruction ins;
    private HealthController playerHealthManager;
    private GunFire gunFire;
    private AnimationSM ASM;
    public bool entered = false;
    public bool finishBlock = false;
    public bool finishLight = false;
    public bool finishHeavy = false;
    public bool finishGun = false;
    public bool finishswitch = false;
    public bool finishRifle = false;
    public bool finishReload = false;
    public bool canKill = false;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthManager = player.GetChild(0).gameObject.GetComponent<HealthController>();
        ASM = player.GetChild(0).gameObject.GetComponent<AnimationSM>();
        gunFire = player.GetChild(0).gameObject.GetComponent<GunFire>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerHealthManager.getHealth() <= 50) playerHealthManager.setHealth(50);
        if (entered && !finishBlock){
            ins.showBlock = true;
        }
        if (!finishBlock && playerHealthManager.getSheld() < 60 && !canKill) {
            finishBlock = true;
            ins.showBlock = false;
            ins.showLight = true;
        }
        if (finishBlock && enemy.health == 25f && !canKill){
            finishLight = true;
            ins.showLight = false;
            ins.showHeavy = true;
        }
        if (finishLight && enemy.health == 5f && !canKill){
            finishHeavy = true;
            ins.showHeavy = false;
            ins.showGun = true;
        }
        if (finishHeavy && enemy.health == 40f && !canKill){
            finishGun = true;
            ins.showGun = false;
            ins.showSwitch = true;
        }
        if (ins.showSwitch && ASM.state == "rifleReady" && !canKill) {
            canKill = true;
            finishswitch = true;
            ins.showSwitch = false;
            ins.showRifle = true;
        }
        if(!enemy) {
            finishRifle = true;
            ins.showRifle = false;
            ins.showReload = true;
        }
        if (finishRifle && ASM.state == "rifleReady" && Input.GetKeyDown("r")) {
            finishReload = true;
            ins.showReload = false;
        }
        if(!canKill) enemy.health = 60f;
        if (finishReload){
            ins.showReload = false;
            door.Open();
        }
    }
}
