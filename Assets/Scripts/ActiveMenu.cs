/* This script is invoked when the red button on the desk is pressed. When onTriggerEnter is called the button will move and the menu will appear, then do the reverse when pressed again */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class ActiveMenu : MonoBehaviour
    {
        private bool isMenuActive = false;
        private bool isPressed = false;
        private bool moveBack = false;
        private float buttonSpeed = 0.1f;
        private GameObject menu;
        private rotateMenu script;
        public GameObject startMenu;
        public Transform startPos;
        public Transform endPos;
        //private float time = 0; //uncomment to test movement

        private void OnTriggerEnter(Collider other)
        {
            if (!isPressed) { isPressed = true; }
        }

        public void OnButtonDown(Hand fromHand)
        {
            if (!isPressed) { isPressed = true; }
        }

        void Update()
        {
            /* Uncomment to activate botton every 4 seconds for testing
             * 
             * time += Time.deltaTime;
            if (time % 4 < 0.1)
            {
                isPressed = true;
            }*/

            if (isPressed) // button has been pressed and needs to move
            {
                float step = buttonSpeed * Time.deltaTime;

                if (!moveBack) // button is moving toward the down position
                {
                    float distance = Vector3.Distance(transform.position, endPos.position);

                    if (distance > 0) // move towards destination
                    {
                        transform.position = Vector3.MoveTowards(transform.position, endPos.position, step);
                    }
                    else if (distance <= 0) // button has reached its destination and now needs to turn around and go back
                    {
                        moveBack = true;
                    }
                }
                else // button is moving back to the up position
                {
                    float distance = Vector3.Distance(transform.position, startPos.position);

                    if (distance > 0) // move towards destination
                    {
                        transform.position = Vector3.MoveTowards(transform.position, startPos.position, step);
                    }
                    else if (distance <= 0) // button has reached its destination, now call appropriate action
                    {
                        moveBack = false;
                        isPressed = false;

                        if (isMenuActive)
                        {
                            script = menu.GetComponent<rotateMenu>();
                            script.CloseMenu();
                            isMenuActive = false;
                            Destroy(menu, 2.0f);
                        }
                        else
                        {
                            menu = Instantiate(startMenu) as GameObject;
                            menu.name = "Menu";
                            isMenuActive = true;
                        }
                    }
                }
            }
        }
    }
}