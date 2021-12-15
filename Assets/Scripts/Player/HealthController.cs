using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

    public int startingHealth = 100;
    private int currentHealth;
    [SerializeField] private AnimationSM ASM;
    [SerializeField] private Slider healthBar;

    private float sheld = 60;
    private float sheldRegin = 0;
    [SerializeField] private Image sheld1, sheld2, sheld3;
    [SerializeField] private Image sheld1Half, sheld2Half, sheld3Half;
    // Start is called before the first frame update
    public Vector3 cameraOffset;
    private Vector3 weaponOffset;
    [SerializeField] private GameObject weapons;
    private AudioSource blockSound, damageSound;


    void Start()
    {
        this.ResetHealthToStarting();
        cameraOffset = new Vector3(0f, 0f, 0f);
        weaponOffset = new Vector3(0f, 0f, 0f);
    }


    void Update() {
        healthBar.value = currentHealth / (float)startingHealth;

        addSheld();
        showSheld();
        offsetReturn();

        weapons.transform.localEulerAngles = weaponOffset;
    }
    
    //show sheld in UI
    private void showSheld() {
        sheld1.enabled = (sheld >= 20);
        sheld2.enabled = (sheld >= 40);
        sheld3.enabled = (sheld == 60);
        sheld1Half.enabled = (sheld >= 10 && sheld < 20);
        sheld2Half.enabled = (sheld >= 30 && sheld < 40);
        sheld3Half.enabled = (sheld >= 50 && sheld < 60);
    }
    
    // Add sheld value
    private void addSheld() {
        if (sheldRegin < 0){
            sheldRegin += Time.deltaTime;
        } else {
            if(sheld < 60) {
                sheld += 15f * Time.deltaTime;
                if(sheld > 60) sheld = 60;
            }
        }
    }

    // Reset health to original starting health
    public void ResetHealthToStarting()
    {
        currentHealth = startingHealth;
    }


    public void ApplyDamage(int damage){
        
        if (ASM.state == "blocking") {
            blockSound = GetComponents<AudioSource>()[1];
            blockSound.Play();
            if (sheld > 0) {
                sheld -= damage;
                sheldRegin = -0.5f;
                if (sheld <= 0) {
                    sheldRegin = -1.5f;
                    sheld = 0f;
                }
                weaponOffset += new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * (float)damage;
                return;
            }
            weaponOffset += new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * (float)damage;
            cameraOffset += new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * (float)damage;
            currentHealth -= damage / 2;
        } else {
            damageSound = GetComponents<AudioSource>()[2];
            damageSound.Play();
            currentHealth -= damage;
            cameraOffset += new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * (float)damage * 2f;
        }
        
        if (currentHealth <= 0)
        {
            // Cursor.lockState = CursorLockMode.None;

            SceneManager.LoadScene("GameOver");
        }
    }

    private void offsetReturn() {
        if (cameraOffset.magnitude > 0.1f) {
            cameraOffset -= cameraOffset.normalized * Time.deltaTime * 30;
        } else {
            cameraOffset = new Vector3(0f, 0f, 0f);
        }
        if (weaponOffset.magnitude > 0.1f) {
            weaponOffset -= weaponOffset.normalized * Time.deltaTime * 30;
        } else {
            weaponOffset = new Vector3(0f, 0f, 0f);
        }
    }

    public void setHealth(int value){
        currentHealth = value;
    }

    public int getHealth(){
        return currentHealth;
    }

    public float getSheld(){
        return sheld;
    }

    public void addHealth(int value){
        currentHealth += value;
        if (currentHealth > startingHealth) currentHealth = startingHealth;
    }
}
