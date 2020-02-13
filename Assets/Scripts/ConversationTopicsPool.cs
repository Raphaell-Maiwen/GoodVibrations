using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationTopicsPool : MonoBehaviour
{
    List<string> advices;

    void Awake()
    {
        //25
        advices = new List<string>{"Ask how their day went.", "Ask them to tell a peaceful memory.", 
            "Ask them to say something good that happened recently.", "Ask what they're planning to eat next.",
            "Ask about a worry they have.", "Talk about the last cute dog you met", "Talk about something fun you've done last weekend",
            "Ask what's on their mind.", "Stay silent for a minute (except if you wanna stop playing, then please speak up!)",
            "Talk about your favorite topic.", "Discuss what you'll do afterwards.","Say something nice about each other", 
            "Is this awkward?", "Mute the music and hum a melody you like.", "Tell each other a secret.", 
            "Talk about something you're looking forward to in the next week or two.", "Why are you playing this together?",
            "Name someone you like, and something you appreciate about them.", "Describe your idea of a perfect day.",
            "Think of the most wholesome thing you can. You don't have to share it with each other.", 
            "Talk about a good movie, book or game you've finished lately.", "Say a thing you like from your relationship with each other.",
            "Discuss something you aim to change in your lives.", "Share an achievement, recent or not, that you're proud of.",
            "Say something you wish for the other person."};
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