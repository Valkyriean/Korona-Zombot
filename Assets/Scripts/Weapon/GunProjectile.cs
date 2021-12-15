using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : MonoBehaviour
{
    [SerializeField] public float delay;
    [SerializeField] private GameObject self;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(self, delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
