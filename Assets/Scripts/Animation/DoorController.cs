using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private bool OpenedOnStart;
    public bool triggerReady = true;
    public bool isOpened;
    // Start is called before the first frame update
    void Start()
    {
        isOpened = OpenedOnStart;
        if (OpenedOnStart) {
            this.GetComponent<Animator>().Play("Opened");
        } else {
            this.GetComponent<Animator>().Play("Closed");
        }
    }

    void Update()
    {
        // // for testing reasons
        // if (Input.GetKeyDown(KeyCode.Alpha9)) {
        //     Trigger();
        // }
    }

    public void Trigger() {
        if (triggerReady) {
            if (isOpened) {
                StartCoroutine(closing());
            } else {
                StartCoroutine(opening());
            }
        }
    }

    public void Open() {
        if (triggerReady) {
            if (isOpened) {
                // StartCoroutine(closing());
            } else {
                StartCoroutine(opening());
            }
        }
    }

    public void Close() {
        if (triggerReady) {
            if (isOpened) {
                StartCoroutine(closing());
            } else {
                // StartCoroutine(opening());
            }
        }
    }

    IEnumerator closing() {
        triggerReady = false;
        this.GetComponent<Animator>().Play("Closing");
        yield return new WaitForSeconds(1.0f);
        isOpened = false;
        triggerReady = true;
        this.GetComponent<Animator>().Play("Closed");
    }

    IEnumerator opening() {
        triggerReady = false;
        this.GetComponent<Animator>().Play("Opening");
        yield return new WaitForSeconds(1.0f);
        isOpened = true;
        triggerReady = true;
        this.GetComponent<Animator>().Play("Opened");
    }
}
