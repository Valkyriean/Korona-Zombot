using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour, Interactable
{
    public void Interact(GameObject other) {
        SceneManager.LoadScene("StartScene");
    }
}
