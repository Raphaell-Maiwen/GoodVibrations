using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class BackMassagerControls : MonoBehaviour
{
    private float vibrationStrength = 0.2f;
    private BackMassager massagerScript;
    public Text subText;

    // Start is called before the first frame update
    void Start()
    {
        GamePad.SetVibration((PlayerIndex)0, vibrationStrength, vibrationStrength);
        massagerScript = GetComponent<BackMassager>();
        subText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("RBPlayer1") || Input.GetKeyDown(KeyCode.RightArrow)) {
            vibrationStrength += 0.1f;
            GamePad.SetVibration((PlayerIndex)0, vibrationStrength, vibrationStrength);
        }
        else if (Input.GetButtonDown("LBPlayer1") || Input.GetKeyDown(KeyCode.LeftArrow)) {
            vibrationStrength -= 0.1f;
            GamePad.SetVibration((PlayerIndex)0, vibrationStrength, vibrationStrength);
        }
        else if (Input.GetButtonDown("RightButtonP1") || Input.GetKeyDown(KeyCode.Space)) {
            massagerScript.GenerateNextInstruction();
        }
        vibrationStrength = Mathf.Clamp(vibrationStrength, 0f, 1f);
    }
}
