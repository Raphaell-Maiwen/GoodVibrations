using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

/*
 Project-wise:
 -Have a menu
 -Have particle effects
 -Have soothing music, maybe ASMR sounds? Music you can change!
 -Allow up to 4 controllers? Not for February playtesting

-In the intro, wait for the player to increase and decrease the vibration.
-Error: Make sure how it feels. Make sure if they want to continue.
-Review the prompts times. They are wrong. 
-Pause a prompt?
-Instruction type: advice. Change pattern. Do you want to keep this pattern? Do a motion. Change the music? Is lighting good?
-Instruction type: cute quotes, wishes, general advices. "I hope you're feeling safe with each other."
-Each wording should be also put in a "priority list"
-Add particle effects
-Add sounds (something ASMR-ish, a soundtrack too?)
*/

public class BackMassager : MonoBehaviour
{
    public float timeSinceLastInstruction;
    public float instructionTime;

    public instructionType[] instructions;

    public instructionType currentInstruction;

    public string currentRegion = "neck";
    public string[] bodyParts;

    public Text textObject;

    ConversationTopicsPool advicesPool;

    string instructionMessage;

    public enum instructionType { 
        askingFeedback,
        changeRegion,
        conversations
    }

    private void Start() {
        //Randomize the order the beginning with permutations
        bodyParts = new string[6]{"scapula", "middle of the back", "bottom of the back", "shoulder", "back of the head", "neck"};

        RandomizeTime();
        
        ShowInstruction("Put the controller on your friend's neck.");
        GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);

        instructions = new instructionType[3]{instructionType.changeRegion, instructionType.askingFeedback, instructionType.conversations};
        RandomizeInstruction();

        advicesPool = GetComponent<ConversationTopicsPool>();
    }

    void Update(){
        timeSinceLastInstruction += Time.deltaTime;

        if (timeSinceLastInstruction >= instructionTime) {
            GenerateNextInstruction();
        }
    }

    public void GenerateNextInstruction() {
        instructionMessage = BuildInstructionMessage(currentInstruction);
        ShowInstruction(instructionMessage);
        RandomizeInstruction();
        RandomizeTime();
    }

    void RandomizeTime() {
        int shortOdds = Random.Range(0, 100);

        if (currentInstruction == instructionType.conversations) {
            if (shortOdds < 69) {
                instructionTime = Random.Range(20f, 40f);
            }
            else {
                instructionTime = Random.Range(40f, 60f);
            }
        }
        else if (instructionMessage == "Stay silent for a minute (except if you wanna stop playing, then please speak up!)") {
            instructionTime = 60f;
        }
        else {
            if (shortOdds < 69) {
                instructionTime = Random.Range(5f, 10f);
            }
            else {
                instructionTime = Random.Range(10f, 15f);
            }
        }

        timeSinceLastInstruction = 0f;
    }

    //Make one "Randomize" function, pass the arrays as ref parameters?
    void RandomizeInstruction() {
        int index = 0;

        for (int i = 0; i < instructions.Length; i++) {
            if (i == instructions.Length - 1) {
                index = instructions.Length - 1;
            }
            else if (Random.Range(0f, 1f) > 0.5f) {
                index = i;
                break;
            }
        }

        currentInstruction = instructions[index];
        for (int j = index; j < instructions.Length - 1; j++) {
            instructions[j] = instructions[j + 1];
        }
        instructions[instructions.Length - 1] = currentInstruction;
    }

    void RandomizeBodyPart() {
        int index = 0;

        for (int i = 0; i < bodyParts.Length; i++) {
            if (i == bodyParts.Length - 1) {
                index = instructions.Length - 1;
            }
            else if (Random.Range(0f, 1f) > 0.5f) {
                index = i;
                break;
            }
        }

        currentRegion = bodyParts[index];
        for (int j = index; j < bodyParts.Length - 1; j++) {
            bodyParts[j] = bodyParts[j + 1];
        }
        bodyParts[bodyParts.Length - 1] = currentRegion;
    }

    /*void RandomizeElement<T>(ref T[,] array) {

        for (int i = 0; i < array.Length; i++) {
            if (i == array.Length - 1) { 
                //ok
            }
            else if (Random.Range(0f, 1f) > 50) { 
                
            }
        }
    }*/

    void ShowInstruction(string text) {
        print(text);
        textObject.text = text;
    }

    string BuildInstructionMessage(instructionType type) {
        int textSeed = Random.Range(0, 100);
        string message = "";

        if (type == instructionType.changeRegion) {
            RandomizeBodyPart();
            if (textSeed < 20) {
                message += "Move to the " + currentRegion + ".";
            }
            else if (textSeed < 40) {
                message += "Massage the " + currentRegion + ".";
            }
            else if (textSeed < 60) {
                message += "Bring the controller to the " + currentRegion + ".";
            }
            else if (textSeed < 80) {
                message += "The " + currentRegion + " is tense.";
            }
            else {
                message += "Give some love to the " + currentRegion + ".";
            }
        }
        else if (type == instructionType.conversations) {
            message = advicesPool.GetAdvice();
        }
        else if (type == instructionType.askingFeedback) {
            if (textSeed < 20) {
                message += "Ask them ";
            }
            else if (textSeed < 40) {
                message += "Check ";
            }
            else if (textSeed < 60) {
                message += "Make sure ";
            }
            else if (textSeed < 80) {
                message += "How can this moment be improved?";
            }
            else {
                message += "Are you both feeling good?";
            }

            if (textSeed < 60) {
                int newTextSeed = Random.Range(0, 100);
                if (newTextSeed < 20) {
                    if (textSeed < 60) message += "how ";
                    message += "it feels";
                    if (textSeed > 60) message += " good";
                    message += ".";
                }
                else if (newTextSeed < 40) {
                    if (textSeed < 40) message += "if ";
                    message += "it's strong enough.";
                }
                else if (newTextSeed < 60) {
                    if (textSeed < 20) message += "some feedback.";
                    else if (textSeed < 40) message += "your breathing.";
                    else message += "you're in a good position.";
                }
                else if (newTextSeed < 80) {
                    if (textSeed < 60) message += "if ";
                    message += "they want to continue.";
                }
                else {
                    if (textSeed < 60) message += "if ";
                    message += "they're comfortable.";
                }
            }
        }

        return message;
    }
}