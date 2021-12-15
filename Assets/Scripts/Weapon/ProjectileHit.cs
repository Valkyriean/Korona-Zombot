using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    [SerializeField] private GameObject self;
    [SerializeField] private GameObject hole;
    public float gundamage = 20f;
        
    // private void OnTriggerEnter(Collider other) {
    //     if (other.gameObject.tag == "Enemy") {
    //         other.GetComponent<EnemyAI>().TakeDamage(gundamage);
    //         Vector3 hitPos = (other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
    //         Debug.Log(transform.position);
    //         GameObject gunHole = Instantiate(hole, hitPos, Quaternion.LookRotation(transform.position - hitPos));
    //     }
    //     //if (self) Destroy(self);
    // }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            return;
        }
        
        if (other.gameObject.tag == "Enemy") {
            if (other.gameObject) {
                other.gameObject.GetComponent<EnemyAI>().TakeDamage(gundamage);
                other.gameObject.GetComponent<EnemyAI>().putStagger((other.transform.position - this.transform.position).normalized * 0.5f);
            }
        } 
        else {
            GameObject gunHole = Instantiate(hole, other.contacts[0].point + 0.001f * other.contacts[0].normal, Quaternion.FromToRotation(Vector3.up, other.contacts[0].normal));
            // GameObject gunHole = Instantiate(hole, other.contacts[0].point, Quaternion.FromToRotation(Vector3.up, other.contacts[0].normal));
            gunHole.transform.parent = other.gameObject.transform;
        }
        if (self) Destroy(self);
    }
}
