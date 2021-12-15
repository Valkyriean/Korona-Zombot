using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSM : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood0, blood1, blood2; 
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject sword;
    [SerializeField] public GameObject gun;
    [SerializeField] public GameObject rifle;

    public string state;
    // Start is called before the first frame update
    public List<string> rifleState = new List<string>();
    public List<string> blockState = new List<string>();
    public List<string> swordState = new List<string>();
    private List<string> swordStartState = new List<string>();

    private bool doLight, doNext;
    void Start()
    {
        sword.GetComponent<Animator>().Play("Ready");
        rifle.GetComponent<Animator>().Play("RifleWait");
        gun.GetComponent<Animator>().Play("GunWait");
        state = "ready";

        rifleState.Add("rifleOut");
        rifleState.Add("rifleReady"); 
        rifleState.Add("rifleReload");

        blockState.Add("blockIn");
        blockState.Add("blocking");
        blockState.Add("blockOut");
    
        swordStartState.Add("ready");
        // swordStartState.Add("retrieveLight1");
        // swordStartState.Add("retrieveLight2");
        // swordStartState.Add("retrieveLight3");
        // swordStartState.Add("retrieveHeavy1");
        // swordStartState.Add("retrieveHeavy2");

        doLight = false;
        doNext = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
        // Debug.Log(state);
        // switch weapon
        if (Input.GetKeyDown(KeyCode.Alpha1) && state == "rifleReady") {
            StopAllCoroutines();
            StartCoroutine(swordOut());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && state == "ready") {
            StopAllCoroutines();
            StartCoroutine(rifleOut());
        }
        

        if (Input.GetButtonUp("Fire1")) {
            
        }

        if (!blockState.Contains(state) && !rifleState.Contains(state)) {
            if (!swordStartState.Contains(state)) {
                // control attack sequence
                if (Input.GetButtonUp("Fire1")) {
                    doLight = true;
                }
                if (Input.GetButtonDown("Fire1")) {
                    doLight = false;
                    doNext = true;
                }
            } else {
                // start attack sequence
                if (Input.GetButton("Fire1")){
                    StopAllCoroutines();
                    doLight = false;
                    doNext = false;
                    StartCoroutine(chargeLight1());
                }
            }
        }

        // block
        if (Input.GetButtonDown("Fire2") && !rifleState.Contains(state)) {
            StopAllCoroutines();
            rifle.GetComponent<Animator>().Play("RifleWait");
            StartCoroutine(blockIn());
        }

        if (Input.GetButtonUp("Fire2") && !rifleState.Contains(state)) {
            StopAllCoroutines();
            StartCoroutine(blockOut());
        }


    }

    IEnumerator swordOut() {
        state = "swordOut";
        rifle.GetComponent<Animator>().Play("RifleIn");
        yield return new WaitForSeconds(0.3f);
        sword.GetComponent<Animator>().Play("SwordOut");
        yield return new WaitForSeconds(0.70f);
        if (state == "swordOut") {
            state = "ready";
            StopAllCoroutines();
            sword.GetComponent<Animator>().Play("Ready");
            rifle.GetComponent<Animator>().Play("RifleWait");
        }
    }

    IEnumerator rifleOut() {
        state = "rifleOut";
        sword.GetComponent<Animator>().Play("SwordIn");
        yield return new WaitForSeconds(0.8f);
        rifle.GetComponent<Animator>().Play("RifleOut");
        yield return new WaitForSeconds(0.5f);
        if (state == "rifleOut") {
            state = "rifleReady";
            StopAllCoroutines();
            gun.GetComponent<Animator>().Play("GunWait");
            rifle.GetComponent<Animator>().Play("RifleReady");
            sword.GetComponent<Animator>().Play("SwordWait");
        }
    }

    IEnumerator blockIn(){
        state = "blockIn";
        sword.GetComponent<Animator>().Play("BlockIn");
        gun.GetComponent<Animator>().Play("GunOut"); 
        yield return new WaitForSeconds(0.2f);
        state = "blocking";
        StopAllCoroutines();
        sword.GetComponent<Animator>().Play("Blocking");
        gun.GetComponent<Animator>().Play("GunReady");
    }

    IEnumerator blockOut(){
        state = "blockOut";
        sword.GetComponent<Animator>().Play("BlockOut");
        gun.GetComponent<Animator>().Play("GunIn");
        yield return new WaitForSeconds(0.3f);
        state = "ready";
        StopAllCoroutines();
        sword.GetComponent<Animator>().Play("Ready");
        gun.GetComponent<Animator>().Play("GunWait");
    }

    IEnumerator chargeLight1() {
        state = "chargeLight1";
        sword.GetComponent<Animator>().Play("SwordLight1P");
        yield return new WaitForSeconds(0.5f);
        if (doLight) {
            doLight = false;
            StartCoroutine(Light1());
        } else {
            StartCoroutine(chargeHeavy1());
        }
    }

    IEnumerator Light1() {
        state = "light1";
        sword.GetComponent<Animator>().Play("SwordLight1S");
        yield return new WaitForSeconds(0.33f);
        if (doNext) {
            doNext = false;
            StartCoroutine(chargeLight2());
        } else {
            StartCoroutine(retrieveLight1());
        }
    }

    IEnumerator retrieveLight1() {
        sword.GetComponent<Animator>().Play("SwordLight1R");
        state = "retrieveLight1";
        yield return new WaitForSeconds(1.0f);
        if (state == "retrieveLight1") {
            sword.GetComponent<Animator>().Play("Ready");
            state = "ready";
        }
    }

    IEnumerator chargeHeavy1() {
        state = "chargeHeavy1";
        sword.GetComponent<Animator>().Play("SwordHeavy1P");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Heavy1());
    }

    IEnumerator Heavy1() {
        sword.GetComponent<Animator>().Play("SwordHeavy1S");
        state = "heavy1";
        yield return new WaitForSeconds(0.33f);
        if (doNext) {
            doNext = false;
            StartCoroutine(chargeLight2H1());
        } else {
            StartCoroutine(retrieveHeavy1());
        }
    }

    IEnumerator retrieveHeavy1() {
        sword.GetComponent<Animator>().Play("SwordHeavy1R");
        state = "retrieveHeavy1";
        yield return new WaitForSeconds(0.67f);
        if (state == "retrieveHeavy1") {
            sword.GetComponent<Animator>().Play("Ready");
            state = "ready";
        }
    }

    IEnumerator chargeLight2() {
        sword.GetComponent<Animator>().Play("SwordLight2P");
        state = "chargeLight2";
        yield return new WaitForSeconds(0.83f);
        if (doLight) {
            doLight = false;
            StartCoroutine(Light2());
        } else {
            StartCoroutine(chargeHeavy2());
        }
    }

    IEnumerator chargeLight2H1() {
        sword.GetComponent<Animator>().Play("SwordLight2PH1");
        state = "chargeLight2";
        yield return new WaitForSeconds(0.5f);
        if (doLight) {
            doLight = false;
            StartCoroutine(Light2());
        } else {
            StartCoroutine(chargeHeavy2());
        }
    }

    IEnumerator chargeHeavy2() {
        sword.GetComponent<Animator>().Play("SwordHeavy2P");
        state = "chargeHeavy2";
        yield return new WaitForSeconds(0.17f);
        StartCoroutine(Heavy2());
    }
    
    IEnumerator Light2() {
        sword.GetComponent<Animator>().Play("SwordLight2S");
        state = "light2";
        yield return new WaitForSeconds(0.33f);
        if (doNext) {
            doNext = false;
            StartCoroutine(chargeLight3());
        } else {
            StartCoroutine(retrieveLight2());
        }
        
    }

    IEnumerator Heavy2() {
        sword.GetComponent<Animator>().Play("SwordHeavy2S");
        state = "heavy2";
        yield return new WaitForSeconds(0.33f);
        if (doNext) {
            doNext = false;
            StartCoroutine(chargeLight1H2());
        } else {
            StartCoroutine(retrieveHeavy2());
        }
    }

    IEnumerator retrieveLight2() {
        sword.GetComponent<Animator>().Play("SwordLight2R");
        state = "retrieveLight2";
        yield return new WaitForSeconds(0.5f);
        if (state == "retrieveLight2") {
            sword.GetComponent<Animator>().Play("Ready");
            state = "ready";
        }
    }

    IEnumerator retrieveHeavy2() {
        sword.GetComponent<Animator>().Play("SwordHeavy2R");
        state = "retrieveHeavy2";
        yield return new WaitForSeconds(0.50f);
        blood0.Play();
        blood1.Play();
        blood2.Play();
        yield return new WaitForSeconds(0.67f);
        if (state == "retrieveHeavy2") {
            sword.GetComponent<Animator>().Play("Ready");
            state = "ready";
        }
    }

    IEnumerator chargeLight3() {
        sword.GetComponent<Animator>().Play("SwordLight3P");
        state = "chargeLight3";
        yield return new WaitForSeconds(0.5f);
        if (doLight) {
            doLight = false;
            StartCoroutine(Light3());
        } else {
            StartCoroutine(chargeHeavy1L3());
        }
    }

    IEnumerator chargeLight1H2() {
        sword.GetComponent<Animator>().Play("SwordLight1PH2");
        state = "chargeLight1";
        yield return new WaitForSeconds(0.33f);
        if (doLight) {
            doLight = false;
            StartCoroutine(Light1());
        } else {
            StartCoroutine(chargeHeavy1());
        }
    }

    IEnumerator Light3() {
        sword.GetComponent<Animator>().Play("SwordLight3S");
        state = "light3";
        yield return new WaitForSeconds(0.6f);
        if (doNext) {
            doNext = false;
            StartCoroutine(chargeLight1L3());
        } else {
            StartCoroutine(retrieveLight3());
        }
    }


    IEnumerator chargeHeavy1L3() {
        sword.GetComponent<Animator>().Play("SwordHeavy1PL3");
        state = "chargeHeavy1";
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(Heavy1());
    }

    IEnumerator retrieveLight3() {
        sword.GetComponent<Animator>().Play("SwordLight3R");
        state = "retrieveLight3";
        yield return new WaitForSeconds(1.5f);
        if (state == "retrieveLight3") {
            sword.GetComponent<Animator>().Play("Ready");
            state = "ready";
        }
    }

    IEnumerator chargeLight1L3() {
        state = "chargeLight1";
        sword.GetComponent<Animator>().Play("SwordLight1PL3");
        yield return new WaitForSeconds(0.4f);
        if (doLight) {
            doLight = false;
            StartCoroutine(Light1());
        } else {
            StartCoroutine(chargeHeavy1());
        }
    }
}
