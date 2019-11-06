using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressed : MonoBehaviour
{
    private bool isPressed = false;
    private float buttonSpeed = 0.05f;
    private bool moveBack = false;
    private GameObject start;
    private GameObject end;

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            if (start == null || end == null)
            {
                start = new GameObject();
                end = new GameObject();
            }
            
            start.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            end.transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
            isPressed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed) // button has been pressed and needs to move
        {
            float step = buttonSpeed * Time.deltaTime;

            if (!moveBack) // button is moving toward the down position
            {
                float distance = Vector3.Distance(transform.position, end.transform.position);

                if (distance > 0) // move towards destination
                {
                    transform.position = Vector3.MoveTowards(transform.position, end.transform.position, step);
                }
                else if (distance <= 0) // button has reached its destination and now needs to turn around and go back
                {
                    moveBack = true;
                }
            }
            else // button is moving back to the up position
            {
                float distance = Vector3.Distance(transform.position, start.transform.position);

                if (distance > 0) // move towards destination
                {
                    transform.position = Vector3.MoveTowards(transform.position, start.transform.position, step);
                }
                else if (distance <= 0) // button has reached its destination, now call appropriate action
                {
                    moveBack = false;
                    isPressed = false;

                    ///////////////////////////
                    //
                    // call appropriate script
                    //
                    ///////////////////////////
                }
            }
        }
    }
}
