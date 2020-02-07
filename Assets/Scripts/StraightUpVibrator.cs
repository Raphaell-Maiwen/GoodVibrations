using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class StraightUpVibrator : MonoBehaviour
{
    /*
    -Patterns where the two halves have different strenghts
    -Refactor (vibrationStrengthLeft and vibrationStrengthRight + GamePad.SetVibration called at everyupdate instead of
        being called in functions and such)
    -Countdown on screen for recording
    -For BackMassager mode: activate / deactivate controller
    -Rewrite with the new input system?
    */

    public vibeMode currentMode;
    public float vibrationStength = 0f;

    //Auto-pilot variables
    public float timePassedSinceLastChange = 0f;
    public float timeUntilNextChange = 0f;
    public const int increaseOdds = 80;
    public bool willIncrease;

    public bool _________________________________________;
    //Recoring variables
    public const float recordingTimeLimit = 10f;
    public float timeRecorded = 0f;
    public List<float> vibratingValues;
    public int playbackIndex = 0;
    public bool hasStartedRecording;


    public bool ________________________________________;
    //Pattern variables
    public List<vibration[]> vibrations;
    public float timeVibrating = 0f;
    bool pausing = false;
    public int patternIndex1 = 0;
    public int patternIndex2 = 0;

    public enum vibeMode { 
        manual,
        autoPilot,
        recording,
        playback,
        pattern
    }

    public struct vibration {
        public float duration;
        public float pause;
        public float intensity;

        public vibration(float dur, float pau, float inten) {
            this.duration = dur;
            this.pause = pau;
            this.intensity = inten;
        }
    }

    private void Awake() {
        currentMode = vibeMode.manual;
        SetupVibrationPatterns();
    }

    // Update is called once per frame
    void Update() {
        if (currentMode == vibeMode.autoPilot) {
            AutoPilot();
        }
        else if (currentMode == vibeMode.recording) {
            Record();
        }
        else if (currentMode == vibeMode.playback) {
            Playback();
        }
        else if (currentMode == vibeMode.pattern) {
            Pattern();
        }

        if (Input.GetButtonDown("BottomButtonP1")) {
            //Use ? operator
            if (currentMode == vibeMode.autoPilot) currentMode = vibeMode.manual;
            else currentMode = vibeMode.autoPilot;

            if (currentMode == vibeMode.autoPilot) {
                StartAutopilot();
            }
        }
        else if (Input.GetButtonDown("LeftButtonP1")) {
            if (currentMode == vibeMode.recording) currentMode = vibeMode.playback;
            else if (currentMode == vibeMode.playback) currentMode = vibeMode.manual;
            //Add a condition that you have to be in manual?
            else {
                currentMode = vibeMode.recording;
                playbackIndex = 0;
                hasStartedRecording = false;
                vibratingValues.Clear();
            }
        }
        else if (Input.GetButtonDown("UpButtonP1")) {
            if (currentMode != vibeMode.pattern) {
                currentMode = vibeMode.pattern;
                timeVibrating = 0f;
                patternIndex1 = 0;
                patternIndex2 = 0;
                GamePad.SetVibration((PlayerIndex)0, vibrations[0][0].intensity, vibrations[0][0].intensity);
            }
            else {
                timeVibrating = 0f;
                patternIndex1++;
                patternIndex2 = 0;
                if (patternIndex1 == vibrations.Count) patternIndex1 = 0;
                vibrationStength = vibrations[patternIndex1][patternIndex2].intensity;
                GamePad.SetVibration((PlayerIndex)0, vibrationStength, vibrationStength);
            }
            //Otherwise cycle
        }
        else if (Input.GetButtonDown("RightButtonP1")) {
            currentMode = vibeMode.manual;
            vibrationStength = 0f;
            GamePad.SetVibration((PlayerIndex)0, vibrationStength, vibrationStength);
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

    void SetupVibrationPatterns() {
        //Dur, pau, inten
        vibrations = new List<vibration[]>();
        vibrations.Add(new vibration[4]);
        vibrations[0][0] = new vibration(0.45f, 0.3f, 0.3f);
        vibrations[0][1] = new vibration(0.45f, 0.3f, 0.3f);
        vibrations[0][2] = new vibration(0.45f, 0.3f, 0.3f);
        vibrations[0][3] = new vibration(1.5f, 0.5f, 0.8f);


        vibrations.Add(new vibration[4]);
        vibrations[1][0] = new vibration(1.5f, 0.1f, 0.8f);
        vibrations[1][1] = new vibration(1.3f, 0.2f, 1f);
        vibrations[1][2] = new vibration(0.6f, 0.4f, 0.3f);
        vibrations[1][3] = new vibration(0.5f, 0.5f, 0.2f);

        vibrations.Add(new vibration[8]);
        vibrations[2][0] = new vibration(0.7f, 0.1f, 0.4f);
        vibrations[2][1] = new vibration(0.7f, 0.1f, 0.4f);
        vibrations[2][2] = new vibration(0.7f, 0.1f, 0.8f);
        vibrations[2][3] = new vibration(0.7f, 0.1f, 0.4f);
        vibrations[2][4] = new vibration(0.7f, 0.1f, 0.6f);
        vibrations[2][5] = new vibration(0.7f, 0.1f, 0.6f);
        vibrations[2][6] = new vibration(0.7f, 0.1f, 1f);
        vibrations[2][7] = new vibration(0.7f, 0.1f, 0.6f);

        //next paradiddle
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

    void Record() {
        if (!hasStartedRecording) {
            float newVibratingValue = Input.GetAxis("RTPlayer1");
            if (newVibratingValue != 0.0f) {
                vibratingValues.Add(newVibratingValue);
                GamePad.SetVibration((PlayerIndex)0, newVibratingValue, newVibratingValue);
                hasStartedRecording = true;
            }
        }

        else if (hasStartedRecording) {
            float newVibratingValue = Input.GetAxis("RTPlayer1");

            print("Recording " + newVibratingValue);

            vibratingValues.Add(newVibratingValue);
            GamePad.SetVibration((PlayerIndex)0, newVibratingValue, newVibratingValue);

            timeRecorded += Time.deltaTime;
            if (timeRecorded >= recordingTimeLimit) {
                timeRecorded = 0f;
                currentMode = vibeMode.playback;
            }
        }
    }

    void Playback() {
        GamePad.SetVibration((PlayerIndex)0, vibratingValues[playbackIndex], vibratingValues[playbackIndex]);
        playbackIndex++;

        if (playbackIndex == vibratingValues.Count) {
            playbackIndex = 0;
        }
    }

    void Pattern() {
        timeVibrating += Time.deltaTime;

        if (!pausing) {
            if (timeVibrating >= vibrations[patternIndex1][patternIndex2].duration) {
                timeVibrating = 0f;
                pausing = true;
                vibrationStength = 0f;
            }
            else {
                vibrationStength = vibrations[patternIndex1][patternIndex2].intensity;
            }
        }
        else if(timeVibrating >= vibrations[patternIndex1][patternIndex2].pause){
            timeVibrating = 0f;
            pausing = false;
            patternIndex2++;
            vibrationStength = 0f;
            if (patternIndex2 == vibrations[patternIndex1].Length) {
                patternIndex2 = 0;
                vibrationStength = vibrations[patternIndex1][patternIndex2].intensity;
            }
        }

        GamePad.SetVibration((PlayerIndex)0, vibrationStength, vibrationStength);
    }
}