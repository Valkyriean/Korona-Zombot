using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterArena : MonoBehaviour
{
    [SerializeField] private ArenaController AC;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        AC.entered = true;
    }
}
