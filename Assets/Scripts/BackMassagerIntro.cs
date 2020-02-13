using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackMassagerIntro : MonoBehaviour
{
    public Text textObject;
    private bool firstPrompt = true;

    int instructionsIndex = 0;
    string[] instructions;

    void Awake()
    {
        instructions = new string[12] { "Do you want to read the instructions? \n A: Yes  B: No", 
            "In this game, you'll use the controller's vibration to massage your friend.", "Press RB to increase the vibration strenght.",
            "Press LB to decrease the vibration strength.", "When the game begins, you'll get instructions on what to do.",
            "You'll also get conversation topics propositions., and other fun stuff.",
            "There's no wrong way to play this game!", "You can ignore any prompt or reword it to your taste.",
            "Actually, you can even press the B button to skip a prompt.", "You can press start at any time to review the controls.",
            "Feel free to stop the experience whenever.", "And now, let the massage begins. Enjoy!"};
        ShowNextInstruction();
    }

    void Update()
    {
        if (Input.GetButtonDown("BottomButtonP1")) {
            ShowNextInstruction();
            firstPrompt = false;
        }
        else if (firstPrompt && Input.GetButtonDown("RightButtonP1")) {
            SkipIntro();
        }
    }

    void ShowNextInstruction() {
        if (instructionsIndex < instructions.Length) {
            textObject.text = instructions[instructionsIndex];
            instructionsIndex++;
        }
        else SkipIntro();
    }

    void SkipIntro() {
        GetComponent<BackMassager>().enabled = true;
        GetComponent<BackMassagerControls>().enabled = true;
        GetComponent<BackMassagerIntro>().enabled = false;
    }
}
