using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class BackMassagerIntro : MonoBehaviour
{
    public Text textObject;
    public Text subText;
    private bool firstPrompt = true;

    int instructionsIndex = 0;
    string[] instructions;

    float vibrationStrengh;
    bool hasVibrationIncreased;
    bool hasVibrationDecreased;

    void Awake()
    {

        //"You can press start at any time to review the controls."
        instructions = new string[11] { "Do you want to read the instructions? \n A: Yes  B: No", 
            "In this game, you'll use the controller's vibration to massage your friend.", "Press RB or right arrow to increase the vibration strenght.",
            "Press LB or left arrow to decrease the vibration strength.", "When the game begins, you'll get instructions on what to do.",
            "You'll also get conversation topics propositions., and other fun stuff.",
            "There's no wrong way to play this game!", "You can ignore any prompt or reword it to your taste.",
            "Actually, you can even press the B button or the space bar to skip a prompt.",
            "Feel free to stop the experience whenever.", "And now, let the massage begins. Enjoy!"};
        ShowNextInstruction();
    }

    void Update()
    {
        if (!firstPrompt)
        {
            if (Input.GetButtonDown("RightButtonP1")) {
                ShowNextInstruction();

                if (instructionsIndex == 3 || instructionsIndex == 4)
                {
                    subText.text = "And then press B to continue.";
                }
                /*else if (instructionsIndex == instructions.Length) {
                    subText.text = "";
                }*/
                else
                {
                    subText.text = "Press B to continue.";
                }
            }
        }
        else {
            if (Input.GetButtonDown("BottomButtonP1"))
            {
                ShowNextInstruction();
                subText.text = "Press B to continue.";
                firstPrompt = false;
            }
            else if (Input.GetButtonDown("RightButtonP1"))
            {
                SkipIntro();
            }
        }

        
        if (instructionsIndex == 3 && Input.GetButtonDown("RBPlayer1") || Input.GetKeyDown(KeyCode.RightArrow)) {
            vibrationStrengh += 0.1f;
            vibrationStrengh = Mathf.Clamp(vibrationStrengh, 0f, 1f);
            GamePad.SetVibration((PlayerIndex)0, vibrationStrengh, vibrationStrengh);
            hasVibrationIncreased = true;
        }
        else if (instructionsIndex == 4 && Input.GetButtonDown("LBPlayer1") || Input.GetKeyDown(KeyCode.LeftArrow)) {
            vibrationStrengh -= 0.1f;
            vibrationStrengh = Mathf.Clamp(vibrationStrengh, 0f, 1f);
            GamePad.SetVibration((PlayerIndex)0, vibrationStrengh, vibrationStrengh);
            hasVibrationDecreased = true;
        }
    }

    void ShowNextInstruction() {
        if (instructionsIndex < instructions.Length)
        {
            if (instructionsIndex == 3 && !hasVibrationIncreased) return;
            if (instructionsIndex == 4 && !hasVibrationDecreased) return;
            textObject.text = instructions[instructionsIndex];
            if (instructionsIndex == 4) GamePad.SetVibration((PlayerIndex)0, 0f, 0f);

            instructionsIndex++;
        }
        else {
            subText.text = "";
            SkipIntro();
        } 
    }

    void SkipIntro() {
        GetComponent<BackMassager>().enabled = true;
        GetComponent<BackMassagerControls>().enabled = true;
        GetComponent<BackMassagerIntro>().enabled = false;
    }
}
