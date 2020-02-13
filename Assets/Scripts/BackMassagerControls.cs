using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class BackMassagerControls : MonoBehaviour
{
    private float vibrationStrength = 0.2f;
    private BackMassager massagerScript;

    // Start is called before the first frame update
    void Start()
    {
        GamePad.SetVibration((PlayerIndex)0, vibrationStrength, vibrationStrength);
        massagerScript = GetComponent<BackMassager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("RBPlayer1")) {
            vibrationStrength += 0.1f;
            GamePad.SetVibration((PlayerIndex)0, vibrationStrength, vibrationStrength);
        }
        else if (Input.GetButtonDown("LBPlayer1")) {
            vibrationStrength -= 0.1f;
            GamePad.SetVibration((PlayerIndex)0, vibrationStrength, vibrationStrength);
        }
        else if (Input.GetButtonDown("RightButtonP1")) {
            massagerScript.GenerateNextInstruction();
        }
        vibrationStrength = Mathf.Clamp(vibrationStrength, 0f, 1f);
    }
}
