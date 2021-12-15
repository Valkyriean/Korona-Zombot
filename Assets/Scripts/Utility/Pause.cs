using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject uiLayer;
    [SerializeField] private GameObject menuLayer;
    private bool paused = false;
    // Update is called once per frame
    void Start()
    {
        resume();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (paused){
                paused = false;
                resume();
            } else {
                paused = true;
                pause();
            }
        }
    }

    public void resume() {
        uiLayer.SetActive(true);
        menuLayer.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void pause() {
        uiLayer.SetActive(false);
        menuLayer.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
}
