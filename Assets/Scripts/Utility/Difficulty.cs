using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 0 : Easy
// 1 : Normal
// 2 : Hard

public class Difficulty : MonoBehaviour
{
    private GameObject easyOn, easyOff;
    private GameObject normalOn, normalOff;
    private GameObject hardOn, hardOff;
    public int difficulty;

    // Start is called before the first frame update
    void Start()
    {
        easyOn = GameObject.Find("EasyOn");
        easyOff = GameObject.Find("EasyOff");
        normalOn = GameObject.Find("NormalOn");
        normalOff = GameObject.Find("NormalOff");
        hardOn = GameObject.Find("HardOn");
        hardOff = GameObject.Find("HardOff");
        // if (!PlayerPrefs.HasKey("Difficulty")){
        //     PlayerPrefs.SetInt("Difficulty", 1);
        // }
        difficulty = PlayerPrefs.GetInt("Difficulty", 1);
        switch (difficulty) {
            case 0:
                setEasy();
                break;
            case 1:
                setNormal();
                break;
            case 2:
                setHard();
                break;
        }
    }

    public void setEasy() {
        PlayerPrefs.SetInt("Difficulty", 0);
        easyOn.SetActive(true);
        easyOff.SetActive(false);
        normalOn.SetActive(false);
        normalOff.SetActive(true);
        hardOn.SetActive(false);
        hardOff.SetActive(true);
    }

    public void setNormal() {
        PlayerPrefs.SetInt("Difficulty", 1);
        easyOn.SetActive(false);
        easyOff.SetActive(true);
        normalOn.SetActive(true);
        normalOff.SetActive(false);
        hardOn.SetActive(false);
        hardOff.SetActive(true);
    }

    public void setHard() {
        PlayerPrefs.SetInt("Difficulty", 2);
        easyOn.SetActive(false);
        easyOff.SetActive(true);
        normalOn.SetActive(false);
        normalOff.SetActive(true);
        hardOn.SetActive(true);
        hardOff.SetActive(false);
    }

}
