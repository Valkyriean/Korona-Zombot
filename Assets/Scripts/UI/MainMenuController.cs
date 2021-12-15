using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    private AudioSource click;

    

    private void Start() {
        Cursor.lockState = CursorLockMode.None;
        click = GetComponent<AudioSource>();
    }

    public void PlayGame(){
        click.Play();

        SceneManager.LoadSceneAsync("MainScene");

    }

    public void Tutorial(){
        click.Play();
        SceneManager.LoadSceneAsync("Prologue");
    }

    public void TitlePage(){
        click.Play();

        SceneManager.LoadSceneAsync("StartScene");
    }

    public void QuitGame(){
        click.Play();

        Application.Quit();
    }
}
