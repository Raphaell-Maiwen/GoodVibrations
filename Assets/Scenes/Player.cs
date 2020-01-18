using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

/*
Adding a gauge so you can press slightly before end of vibration
Message of getting too slow or too fast
Intensity increases after several good hits (in a row)
Intensity decreases after several (not in a row, total of the current stage)
Max intensity = automatically vibrates
After 2 or 3 intensity decrease, it changes buttons

Be more gentle at first?

Add joystick interactions:
-Circular clockwise
-Circular anticlockwise
-Left and right
-Up and down
*/
public class Player : MonoBehaviour
{
    public int playerInd;
    PlayerIndex otherPlayer;

    GamePadState state;
    string[] buttons;
    string currentButton;

    //Minimum base vibration: 0.2f
    //Maximum base vibration: 2f
    //First range: 0.2 - 0.85
    //Second range: 0.86 - 1.35
    //Third range: 1.36 - 2.00
    float buttonVibratingLength;
    float vibratingTime;
    bool isVibrating;

    float pressButtonTimeWindow;

    float lastTime;

    void Awake()
    {
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
        buttons[3] = "RightButtonP" + playerInd ;

        currentButton = buttons[Random.Range(0, buttons.Length)];

        Debug.Log(currentButton);

        buttonVibratingLength = 0.2f;
        pressButtonTimeWindow = 0.05f;

        //SetupVibratingLength();
    }

    // Update is called once per frame
    void Update(){
        //Debug.Log("H: " + Input.GetAxis("HorizontalP" + 1) + " V: " + Input.GetAxis("VerticalP1"));

        //Turn off the vibration after the predetermined length
        if (isVibrating) {
            vibratingTime += Time.deltaTime;
            if (vibratingTime >= buttonVibratingLength) {
                vibratingTime = 0;
                isVibrating = false;
                GamePad.SetVibration(otherPlayer, 0f, 0f);
            }
        }

        //Turn on the vibration if the predetermined button is pushed
        if (Input.GetButtonDown(currentButton) && !isVibrating) {
            print(currentButton + " " + playerInd);
            GamePad.SetVibration(otherPlayer, 0.5f, 0.5f);
            isVibrating = true;
            lastTime = Time.time;
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
}
