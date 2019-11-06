using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem.Sample
{
    /*!
     * This class is for the button that resets the entire Whiteboard texture.
     * This class file is attached to the RedButton placed on the Whiteboard.
     */ 
    public class EraseBoard : MonoBehaviour
    {
        //var wbScript : MonoScript;
        public HoverButton Button;

        /*!
         * This is the function that dynamically runs when the Unity project is first started.
         * When the button is pressed down, the second function gets called.
         */
        private void Start()
        {
            Button.onButtonDown.AddListener(OnButtonDown);
            UnityEngine.Debug.Log("IN ERASE BUTTON START");
        }

        /*!
         * This is the function is called when the RedButton is pressed down.
         * It destroys the texture and calls Start() from the WhiteboardScript to get a new texture.
         */
        private void OnButtonDown(Hand hand)
        {
            var whiteboardO = GameObject.Find("Whiteboard");
            var whiteboardScript = whiteboardO.GetComponent<WhiteboardScript>();
            Destroy(whiteboardScript.whiteboardTexture);
            whiteboardScript.Start();
        }
    }
}
