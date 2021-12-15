using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CalculatePivot Pivot;
    [SerializeField] private string EnemyTag = "Enemy";
    [SerializeField] private string InteractTag = "Interactive";
    public float interactDistance = 1.3f;
    public GameObject aimedInteractive = null;
    public GameObject aimedEnemy = null;
    private Transform _aimedObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_aimedObject != null){
            // var aimedObject = _aimedObject.GetComponent<Renderer>();
            // aimedObject.sharedMaterial.SetInt("_Aimed", 0);
        }

        var ray = new Ray(playerCamera.transform.position + Pivot.front * 0.4f, Pivot.front);
        RaycastHit hit;
        aimedEnemy = null;
        aimedInteractive = null;
        if (Physics.Raycast(ray, out hit)){
            // Debug.Log("Aimed on");
            var aimed = hit.transform;
            // aimed an enemy
            if (aimed.CompareTag(EnemyTag)){
                aimedEnemy = hit.transform.gameObject;
                // var aimedRender  = aimed.GetComponent<Renderer>();
                // if (aimedRender != null){
                //     // Debug.Log("Aimed on");
                //     aimedRender.sharedMaterial.SetInt("_Aimed", 1);
                // }
                _aimedObject = aimed;
            }

            // aimed an interactable item
            if (aimed.CompareTag(InteractTag)){
                if (hit.distance < interactDistance) aimedInteractive = aimed.gameObject;
                if (aimedInteractive && Input.GetKeyDown(KeyCode.F)){
                    foreach (Interactable script in aimedInteractive.GetComponents<Interactable>())
                    {
                        script.Interact(player);
                    }
                }
                    
            }

        }
    }
}
