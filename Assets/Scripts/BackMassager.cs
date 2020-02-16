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

-Pause a prompt?
-Instruction type: advice. Change pattern. Do you want to keep this pattern? Do a motion. Change the music? Is lighting good?
-Instruction type: cute quotes, wishes, general advices, food for thought. "I hope you're feeling safe with each other."
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

    //Set it up right
    public string[] regionWordings;

    public string[] askingFeedbackSentences;
    public int[] askingFeedbackIndexes; //Maybe useless?

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

        askingFeedbackSentences = new string[9] {"it feels" , "it's strong enough", "they're comfortable", "they want to continue",
                "some feedback", "you're both in a good position", "Check your breathing", "How can this moment be improved?",
                "Are you both feeling good?"};

        askingFeedbackIndexes = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

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
        RandomizeTime();
        RandomizeInstruction();
        //RandomizeTime();
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

    int RandomizeInt(ref int[] array) {
        int index = 0;

        for (int i = 0; i < array.Length; i++) {
            if (i == array.Length - 1) index = array.Length - 1;
            else if (Random.Range(0f, 1f) > 0.5f) {
                index = i;
                break;
            }
        }

        int result = array[index];

        for (int j = index; j < array.Length - 1; j++) {
            array[j] = array[j + 1];
        }

        array[array.Length - 1] = result;
        return result;
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
            int index = RandomizeInt(ref askingFeedbackIndexes);
            int rn = Random.Range(0, 100);

            /*
                askingFeedbackSentences = new string[9] {"it feels" , "it's strong enough", "they're comfortable", "they want to continue",
                "some feedback", "you're both in a good position", "Check your breathing", "How can this moment be improved?",
                "Are you both feeling good?"};
             */

            if (index < 3) {
                if (rn < 33) {
                    message += "Ask them ";
                    if (index < 1) message += "how " + askingFeedbackSentences[index];
                    else {
                        message += "if " + askingFeedbackSentences[index];
                        if (index == 2) message += " good";
                    }
                }
                else if (rn < 66) {
                    message += "Check ";
                    if (index < 1) message += "how ";
                    else message += "if ";
                    message += askingFeedbackSentences[index];
                }
                else {
                    message += "Make sure " + askingFeedbackSentences[index];
                    if (index == 0) message += " good";
                }

            }
            else if (index == 3) {
                if (rn < 50) message += "Ask them if " + askingFeedbackSentences[index];
                else message += "Check if " + askingFeedbackSentences[index];
            }
            else if (index == 4) message += "Ask " + askingFeedbackSentences[index];
            else if (index == 5) {
                if (rn < 50) message += "Check if " + askingFeedbackSentences[index];
                else message += "Make sure " + askingFeedbackSentences[index];
            }
            else message += askingFeedbackSentences[index];

            if(index < 7) message += ".";
        }

        return message;
    }
}