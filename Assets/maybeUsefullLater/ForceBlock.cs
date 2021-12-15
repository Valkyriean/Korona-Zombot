using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBlock : MonoBehaviour
{
    private AnimationSM ASM;
    private float timer = 0;
    public float blockInTime, blockOutTime;
    private Vector3 posBlock, rotBlock, posReady, rotReady, posNow, rotNow, posStep, rotStep;
    private Vector3 posBlockGun, rotBlockGun, posReadyGun, rotReadyGun, posNowGun, rotNowGun, posStepGun, rotStepGun;
    // Start is called before the first frame update
    void Start()
    {
        ASM = this.GetComponent<AnimationSM>();
        posReady = new Vector3(0.374f, 0.849f, 0.445f);
        rotReady = new Vector3(5.3f, -97.6f, -43.6f);
        posBlock = new Vector3(0.481f, 1.435f, 0.462f);
        rotBlock = new Vector3(145.4f, -145.8f, -76.7f);

        posReadyGun = new Vector3(-0.305f, 0.392f, 0.323f);
        rotReadyGun = new Vector3(59.74f, 0.0f, 0.0f);
        posBlockGun = new Vector3(-0.305f, 0.9576f, 0.457f);
        rotBlockGun = new Vector3(-3.04f, 5.21f, 1.6f);
    }


    // Update is called once per frame
    void Update()
    {
        //  Debug.Log(ASM.state);
        
        if(ASM.state == "rifleReady") return;
        if(Input.GetKeyDown(KeyCode.Alpha2) && ASM.blockState.Contains(ASM.state)) 
        {
            timer = 0;
            posNowGun = ASM.gun.transform.localPosition;
            rotNowGun = QuaternionToVector3(ASM.gun.transform.localRotation);
            posStepGun = (posReadyGun - posNowGun) / blockInTime;
            rotStepGun = (rotReadyGun - rotNowGun) / blockInTime;
            ASM.gun.GetComponent<Animator>().enabled = true;
            
            ASM.sword.GetComponent<Animator>().enabled = true;
        }
        // start blockIn
        if (Input.GetButtonDown("Fire2") && !ASM.rifleState.Contains(ASM.state))
        {
            ASM.state = "blockIn";
            StopAllCoroutines();
            posNow = ASM.sword.transform.localPosition;
            rotNow = QuaternionToVector3(ASM.sword.transform.localRotation);
            posNowGun = ASM.gun.transform.localPosition;
            rotNowGun = QuaternionToVector3(ASM.gun.transform.localRotation);
            
            posStep = (posBlock - posNow) / blockInTime;
            rotStep = (rotBlock - rotNow) / blockInTime;
            posStepGun = (posBlockGun - posNowGun) / blockInTime;
            rotStepGun = (rotBlockGun - rotNowGun) / blockInTime;
 
            ASM.sword.GetComponent<Animator>().enabled = false;
            ASM.gun.GetComponent<Animator>().enabled = false;
        }
        // blockIn or blocking
        if (Input.GetButton("Fire2") && !ASM.rifleState.Contains(ASM.state))
        {
            if (timer < blockInTime){
                // charging for blocking
                timer += Time.deltaTime;
                
                posNow += posStep * Time.deltaTime;
                rotNow += rotStep * Time.deltaTime;
                ASM.sword.transform.localPosition = posNow;
                ASM.sword.transform.localRotation = Vector3ToQuaternion(rotNow);
                
                posNowGun += posStepGun * Time.deltaTime;
                rotNowGun += rotStepGun * Time.deltaTime;
                ASM.gun.transform.localPosition = posNowGun;
                ASM.gun.transform.localRotation = Vector3ToQuaternion(rotNowGun);
            } else {
                // blocking
                ASM.state = "blocking";
                ASM.sword.GetComponent<Animator>().enabled = true;
                ASM.sword.GetComponent<Animator>().Play("Blocking");
                ASM.gun.GetComponent<Animator>().enabled = true;
                ASM.gun.GetComponent<Animator>().Play("GunReady");
            }
                
        }
        // blockOut
        if (Input.GetButtonUp("Fire2") && !ASM.rifleState.Contains(ASM.state))
        {
            timer = 0;
            ASM.state = "blockOut";
            posNow = ASM.sword.transform.localPosition;
            rotNow = QuaternionToVector3(ASM.sword.transform.localRotation);
            posStep = (posReady - posNow) / blockInTime;
            rotStep = (rotReady - rotNow) / blockInTime;
            ASM.sword.GetComponent<Animator>().enabled = false;
            

            posNowGun = ASM.gun.transform.localPosition;
            rotNowGun = QuaternionToVector3(ASM.gun.transform.localRotation);
            posStepGun = (posReadyGun - posNowGun) / blockInTime;
            rotStepGun = (rotReadyGun - rotNowGun) / blockInTime;
            ASM.gun.GetComponent<Animator>().enabled = false;
        }
        
        if (ASM.state == "blockOut")
        {
            if (timer < blockOutTime){
                // leaving from blocking
                timer += Time.deltaTime;
                posNow += posStep * Time.deltaTime;
                rotNow += rotStep * Time.deltaTime;
                ASM.sword.transform.localPosition = posNow;
                ASM.sword.transform.localRotation = Vector3ToQuaternion(rotNow);
                
                posNowGun += posStepGun * Time.deltaTime;
                rotNowGun += rotStepGun * Time.deltaTime;
                ASM.gun.transform.localPosition = posNowGun;
                ASM.gun.transform.localRotation = Vector3ToQuaternion(rotNowGun);
            } else {
                // ready
                ASM.state = "ready";
                ASM.sword.GetComponent<Animator>().enabled = true;
                ASM.sword.GetComponent<Animator>().Play("Ready");
                ASM.gun.GetComponent<Animator>().enabled = true;
                ASM.gun.GetComponent<Animator>().Play("GunWait");
            }
        }
        if (ASM.state == "rifleOut") 
        {
            if (timer < blockOutTime){
                timer += Time.deltaTime;
                posNowGun += posStepGun * Time.deltaTime;
                rotNowGun += rotStepGun * Time.deltaTime;
                ASM.gun.transform.localPosition = posNowGun;
                ASM.gun.transform.localRotation = Vector3ToQuaternion(rotNowGun);
            } else {
                ASM.gun.GetComponent<Animator>().enabled = true;
                ASM.gun.GetComponent<Animator>().Play("GunWait");
            }
            return;
        }
    }

    Quaternion Vector3ToQuaternion(Vector3 input){
        return Quaternion.Euler(input.x, input.y, input.z);
    }

    Vector3 QuaternionToVector3(Quaternion input){
        return input.eulerAngles;
    }
}
