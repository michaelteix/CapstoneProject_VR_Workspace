using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntingText : MonoBehaviour
{
    public bool beenHit;
    int numberofHits;
    public List<string> comments;
    public UnityEngine.UI.Text textscript;
    int count;

    // Start is called before the first frame update
    void Start()
    {
        beenHit = false;
        numberofHits = 0;
        comments.Add("Your Aim is\nWeaksauce");
        comments.Add("Try\nAgain");
        comments.Add("Can't Touch\nThis");
        comments.Add("My Grandma has\nBetter Aim");
        comments.Add("I'll forget I\nSaw That");
        comments.Add("Take off the Headset, and go\nstraight to the Eye Doctor");
        comments.Add("Someone get this guy\nsome lessons");
        comments.Add("Let me put a bet\non the other guy");
        comments.Add("Wow... Get\nBack to Work");
        
        count = comments.Count;
    }

    // Update is called once per frame
    void Update()
    {
        // Simulates a hit event
       if(beenHit)
       {
            BeenHit();
            beenHit = false;
       }
    }


    int index = 1;
    public void BeenHit()
    {
        if (index >= count)
            index = 0;
        textscript.text = comments[index];
        index++;
        numberofHits++;
    }
}
