using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

/*
Message of getting too slow or too fast
Max intensity = automatically vibrates
After 2 or 3 intensity decrease, it changes buttons

Be more gentle at first?
Tweak the errors / good pushes numbers

Add joystick interactions:
-Circular clockwise
-Circular anticlockwise
-Left and right
-Up and down


DONE:
Adding a gauge so you can press slightly before end of vibration
Intensity increases after several good hits (in a row)
Intensity decreases after several (not in a row, total of the current stage)

    At some points, buttons change: the other player selects it and have to share it vocally, otherwise there's a fault

*/
public class Player : MonoBehaviour
{
    public int playerInd;
    PlayerIndex otherPlayer;

    GamePadState state;
    string[] buttons;
    string currentButton;

    public bool printing;

    //Minimum base vibration: 0.2f
    //Maximum base vibration: 2f
    //First range: 0.2 - 0.85
    //Second range: 0.86 - 1.35
    //Third range: 1.36 - 2.00
    float buttonVibratingLength;
    float vibratingTime;
    bool isVibrating;

    float vibrationIntensity = 0.3f;

    //After multiple tests, I arrived to the conclusion that pressButtonTimeWindow should be of 0.2f no matter the buttonVibratingLength;
    const float pressButtonTimeWindow = 0.2f;
    float lastTime;
    float timePassedSinceLastPush;

    public int errors;
    public int decreases;
    //After 3 good pushes, we start counting errors
    public int goodPushes;
    public bool countingErrors;
    const int goodPushesTreshold = 3;

    bool streak = false;

    void Awake() {
        if (playerInd == 1) {
            otherPlayer = (PlayerIndex)1;
        }
        else {
            otherPlayer = (PlayerIndex)0;
        }

        buttons = new string[4];
        buttons[0] = "BottomButtonP" + playerInd;
        buttons[1] = "UpButtonP" + playerInd;
        buttons[2] = "LeftButtonP" + playerInd;
        buttons[3] = "RightButtonP" + playerInd;

        currentButton = buttons[Random.Range(0, buttons.Length)];

        Debug.Log(currentButton);

        //buttonVibratingLength = 3f;

        SetupVibratingLength();
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log("H: " + Input.GetAxis("HorizontalP" + 1) + " V: " + Input.GetAxis("VerticalP1"));

        //Basically all for testing purposes
        /*if (printing) {
            if (streak) {
                float timePassedSinceLastPush = Time.time - lastTime;
                if (timePassedSinceLastPush < buttonVibratingLength - pressButtonTimeWindow) {
                    print("Too early");
                }
                else if (timePassedSinceLastPush >= buttonVibratingLength - pressButtonTimeWindow && timePassedSinceLastPush < buttonVibratingLength) {
                    print("Early but good");
                    //goodPushes++;
                }
                else if (timePassedSinceLastPush == buttonVibratingLength) {
                    print("Right on time");
                    //goodPushes++;
                }
                else if (timePassedSinceLastPush > buttonVibratingLength && timePassedSinceLastPush <= buttonVibratingLength + pressButtonTimeWindow) {
                    print("Late but good");
                    //goodPushes++;
                }
                else {
                    //print("Too late");
                    errors++;
                    streak = false;
                    //goodPushes = 0;
                }
                if (goodPushes == goodPushesTreshold) {
                    print("Starting game");
                    Time.timeScale = 0f;
                }
            }
            else {
                print("Waiting for input");
            }
        }*/

        

        //Turn off the vibration after the predetermined length
        if (isVibrating) {
            vibratingTime += Time.deltaTime;
            if (vibratingTime >= buttonVibratingLength) {
                vibratingTime = 0;
                isVibrating = false;
                GamePad.SetVibration(otherPlayer, 0f, 0f);
            }
        }

        //(Time.time - last Time ) > (buttonVibrationLength - gauge) && (Time.time - lastTime) < (buttonVibrationLength - gauge)

        //Turn on the vibration if the predetermined button is pushed
        /*if (Input.GetButtonDown(currentButton)) { // && !isVibrating
            GamePad.SetVibration(otherPlayer, 0.3f, 0.3f);
            isVibrating = true;
            lastTime = Time.time;
            streak = true;
        }*/

        if (streak) {
            float timePassedSinceLastPush = Time.time - lastTime;
            if (Input.GetButtonDown(currentButton)) {
                if (timePassedSinceLastPush >= buttonVibratingLength - pressButtonTimeWindow && timePassedSinceLastPush <= buttonVibratingLength + pressButtonTimeWindow) {
                    Vibrate();
                    lastTime = Time.time;
                    print("Yes");
                }
                else {
                    Mistake();
                    print("No");
                }
            }
            else if (timePassedSinceLastPush > buttonVibratingLength + pressButtonTimeWindow) {
                Mistake();
                print("No2");
            }
        }
        else if (Input.GetButtonDown(currentButton)) {
            Vibrate();
            lastTime = Time.time;
            print("Yes2");
        }
    }

    void SetupVibratingLength() {
        int speedLevel = Random.Range(0, 100);

        if (speedLevel < 20) {
            buttonVibratingLength = Random.Range(0.2f, 0.85f);
        }
        else if (speedLevel < 80) {
            buttonVibratingLength = Random.Range(0.86f, 1.35f);
        }
        else {
            buttonVibratingLength = Random.Range(1.36f, 2f);
        }
    }

    void Mistake() {
        print("Mistake");
        streak = false;
        goodPushes = 0;

        if (countingErrors) {
            errors++;
            countingErrors = false;
        }

        //TODO: Another number than 8
        if (errors == 3     ) {
            decreases++;
            errors = 0;
            countingErrors = false;
            if (vibrationIntensity > 0.1f) {
                vibrationIntensity -= 0.1f;
            }
        }
    }

    //This will be in another class to make it multi-platform
    void Vibrate() {
        print("Good");
        GamePad.SetVibration(otherPlayer, vibrationIntensity, vibrationIntensity);
        isVibrating = true;
        //lastTime = Time.time;
        streak = true;

        goodPushes++;
        if (goodPushes == goodPushesTreshold) {
            countingErrors = true;
        }
        //TODO: Another number than 8
        else if (goodPushes == 5) {
            //TODO: Call a function that increases
            errors = 0;
            goodPushes = 0;
            countingErrors = false;
            if (vibrationIntensity < 1f) {
                vibrationIntensity += 0.1f;
                print("Vibration intensity: " + vibrationIntensity);
            }
        }

        vibratingTime = 0f;
    }
}