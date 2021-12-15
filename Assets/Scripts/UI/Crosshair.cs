using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Crosshair : MonoBehaviour
{
    [SerializeField] private AnimationSM ASM;
    [SerializeField] private PlayerAim Aim;
    public float crosshaiSize, crosshaiLocation;
    [SerializeField] private Image CrosshairUp, CrosshairLeft, CrosshairRight;
    [SerializeField] private Image CrosshairUpR, CrosshairLeftR, CrosshairRightR;
    [SerializeField] private PlayerMovement player;
    List<string> rangeState = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        crosshaiSize = 10;
        crosshaiLocation = 10;
        rangeState.Add("blocking");
        rangeState.Add("rifleReady"); 
        CrosshairUpR.enabled = false;
        CrosshairLeftR.enabled = false;
        CrosshairRightR.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Aim.aimedEnemy) {
            CrosshairUp.enabled = false;
            CrosshairLeft.enabled = false;
            CrosshairRight.enabled = false;
            CrosshairUpR.enabled = true;
            CrosshairLeftR.enabled = true;
            CrosshairRightR.enabled = true;
        } else {
            CrosshairUp.enabled = true;
            CrosshairLeft.enabled = true;
            CrosshairRight.enabled = true;
            CrosshairUpR.enabled = false;
            CrosshairLeftR.enabled = false;
            CrosshairRightR.enabled = false;
        }


        if (player.isGrounded) {
            if (crosshaiSize > 10) crosshaiSize -= Time.deltaTime * 100;
        } else {
            if (crosshaiSize < 25) crosshaiSize += Time.deltaTime * 50;
        }
        
        if (rangeState.Contains(ASM.state)) {
            if (crosshaiLocation > crosshaiSize * - 0.5f) crosshaiLocation -= Time.deltaTime * 100;
            if (crosshaiLocation < crosshaiSize * - 0.5f - 0.2f) crosshaiLocation += Time.deltaTime * 100;
        } else {
            if (crosshaiLocation > crosshaiSize) crosshaiLocation -= Time.deltaTime * 100;
            if (crosshaiLocation < crosshaiSize - 0.2f) crosshaiLocation += Time.deltaTime * 100;
        }
        
        CrosshairUp.GetComponent<RectTransform>().localPosition  = new Vector3(0, 2f * crosshaiLocation, 0);
        CrosshairLeft.GetComponent<RectTransform>().localPosition  = new Vector3(-1.7f * crosshaiLocation, -crosshaiLocation, 0);
        CrosshairRight.GetComponent<RectTransform>().localPosition  = new Vector3(1.7f * crosshaiLocation, -crosshaiLocation, 0);
        CrosshairUpR.GetComponent<RectTransform>().localPosition  = new Vector3(0, 2f * crosshaiLocation, 0);
        CrosshairLeftR.GetComponent<RectTransform>().localPosition  = new Vector3(-1.7f * crosshaiLocation, -crosshaiLocation, 0);
        CrosshairRightR.GetComponent<RectTransform>().localPosition  = new Vector3(1.7f * crosshaiLocation, -crosshaiLocation, 0);

    }
}
