using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour, Interactable
{
    [SerializeField] private GameObject self;
    [SerializeField] private int healthGain;
    // Start is called before the first frame update
    public void Interact(GameObject other) {
        other.transform.GetChild(0).gameObject.GetComponent<HealthController>().addHealth(healthGain);
        if (self) Destroy(self);
    }
    
}
