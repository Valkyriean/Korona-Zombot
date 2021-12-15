using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInteract : MonoBehaviour
{
    [SerializeField] private PlayerAim Aim;
    [SerializeField] private Image interactUI;
    // Start is called before the first frame update
    void Start()
    {
        interactUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        interactUI.enabled = false;
        if (Aim.aimedInteractive) interactUI.enabled = true;
    }
}
