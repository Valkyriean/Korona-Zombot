using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    [SerializeField] private AnimationSM ASM;

    List<string> attackLightState = new List<string>();
    List<string> attackHeavyState = new List<string>();

    //damage variable
    [SerializeField] float damageLight = 35f;
    [SerializeField] float damageHeavy = 55f;


    void Start()
    {
        attackLightState.Add("light1");
        attackLightState.Add("light2");
        attackLightState.Add("light3");
        attackHeavyState.Add("heavy1");
        attackHeavyState.Add("heavy2");
    }

    private void OnTriggerEnter(Collider other) {        
        if (other.CompareTag("Enemy")) {
            if (attackLightState.Contains(ASM.state)) {
                other.GetComponent<EnemyAI>().TakeDamage(damageLight);
                other.GetComponent<EnemyAI>().putStagger((other.transform.position - this.transform.position).normalized * 1f);
            }
            
            if (attackHeavyState.Contains(ASM.state)) {
                other.GetComponent<EnemyAI>().TakeDamage(damageHeavy);
                other.GetComponent<EnemyAI>().putStagger((other.transform.position - this.transform.position).normalized * 1f);
            }
        }
    }

    
}
