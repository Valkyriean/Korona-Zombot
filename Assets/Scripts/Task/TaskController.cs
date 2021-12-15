using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskController : MonoBehaviour
{
    public int taskProcess = 0;
    [SerializeField] private GameObject wayPoint;
    [SerializeField] private Text textBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (taskProcess == 0) {
            wayPoint.transform.position = new Vector3(-15f, 8.5f, -50.6f);
            textBox.text = "Search for the chip";
        }
        if (taskProcess == 1) {
            wayPoint.transform.position = new Vector3(-56.25f, -3.5f, -73.5f);
            textBox.text = "Fix the junction box";
        } 
        if (taskProcess == 2) {
            wayPoint.transform.position = new Vector3(-60.2f, 0.9f, -22.6f);
            textBox.text = "Upload chip content and yourself";
        }
        if (taskProcess == 3) {
            SceneManager.LoadScene("StartScene");
        }
    }
}
