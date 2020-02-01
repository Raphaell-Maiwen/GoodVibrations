using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class StraightUpVibrator : MonoBehaviour
{
    public vibeMode currentMode;

    //bool isAutopilotOn;
    public float vibrationStength = 0f;
    public float timePassedSinceLastChange = 0f;
    public float timeUntilNextChange = 0f;
    public const int increaseOdds = 80;
    public bool willIncrease;

    public enum vibeMode { 
        manual,
        autoPilot,
        recording,
        playback,
        pattern
    }

    /*
    Features:
    -Record pattern
    -Patterns
        
    DONE:
    -Increase and decrease strenght
    -Auto-pilot
    */

    private void Awake() {
        currentMode = vibeMode.manual;
    }

    // Update is called once per frame
    void Update() {
        if (currentMode == vibeMode.autoPilot) {
            AutoPilot();
        }

        if (Input.GetButtonDown("BottomButtonP1")) {
            //isAutopilotOn = !isAutopilotOn;

            //Use ? operator
            if (currentMode == vibeMode.autoPilot) currentMode = vibeMode.manual;
            else currentMode = vibeMode.autoPilot;

            if (currentMode == vibeMode.autoPilot) {
                StartAutopilot();
            }
        }
        else if (currentMode == vibeMode.manual) {
            if (Input.GetButtonDown("RBPlayer1")) {
                vibrationStength += 0.1f;
            }
            else if (Input.GetButtonDown("LBPlayer1")) {
                vibrationStength -= 0.1f;
            }
            vibrationStength = Mathf.Clamp(vibrationStength, 0f, 1f);
            GamePad.SetVibration((PlayerIndex)0, vibrationStength, vibrationStength);
        }

        //Ouain
        //GamePad.SetVibration((PlayerIndex)0, vibrationStength, vibrationStength);
    }

    void StartAutopilot() {
        vibrationStength = 0.1f;

        GamePad.SetVibration((PlayerIndex)0, vibrationStength, vibrationStength);
        willIncrease = true;

        timePassedSinceLastChange = 0f;
        RandomizeTime();
    }

    void AutoPilot() {
        timePassedSinceLastChange += Time.deltaTime;

        if (timePassedSinceLastChange >= timeUntilNextChange) {
            timePassedSinceLastChange = 0f;

            if (willIncrease) {
                vibrationStength += 0.1f;
            }
            else {
                vibrationStength -= 0.1f;
            }
            //GamePad.SetVibration((PlayerIndex)0, vibrationStength, vibrationStength);

            RandomizeTime();


            //Use ? operator
            if (Mathf.Approximately(vibrationStength, 1) || 
                (!Mathf.Approximately(vibrationStength, 0.1f) && Random.Range(0, 100) > increaseOdds)) {
                willIncrease = false;
            }
            else {
                willIncrease = true;
            }
        }
        GamePad.SetVibration((PlayerIndex)0, vibrationStength, vibrationStength);
    }

    void RandomizeTime() {
        int shortOdds = Random.Range(0, 100);

        if (shortOdds < 69) {
            timeUntilNextChange = Random.Range(5f, 15f);
        }
        else {
            timeUntilNextChange = Random.Range(15f, 30f);
        }
    }
}