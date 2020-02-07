using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvicesPool : MonoBehaviour
{
    List<string> advices;

    void Awake()
    {
        //14
        advices = new List<string>{"Ask how their day went.", "Ask them to tell a good memory.", 
            "Ask them to say something good that happened recently.", "Ask what they're planning to eat next.",
            "Ask about a worry they have.", "Talk about the last cute dog you met", "Talk about something fun you've done last weekend",
            "Ask what's on their mind", "Stay silent for a minute (except if you wanna stop playing, then please speak up!)",
            "Talk about your favorite topic.", "Discuss what you'll do afterwards.","Say something nice about each other", 
            "Is this awkward?", "Mute the music and hum a melody you like."};
    }

    public string GetAdvice() {
        if (advices.Count > 0) {
            int rn = Random.Range(0, advices.Count - 1);
            string message = advices[rn];
            advices.RemoveAt(rn);
            return message;
        }

        return null;
    }
}