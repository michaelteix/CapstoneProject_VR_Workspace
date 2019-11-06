/* When object is created the Menu will instantly start to rotate */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateMenu : MonoBehaviour
{
    private float menuSpeed = 0.05f;
    private float tiltSpeedUp = 1.5f;
    private float tiltSpeedDown = 3.0f;
    private bool isDone = false;
    private bool opening = true;
    public GameObject rotatedMenu;
    public GameObject flatMenu;
    
    void Update()
    {
        if ( !isDone )
        {
            if ( opening )
            {
                float distance = Vector3.Distance(transform.position, rotatedMenu.transform.position);

                // Rotate the menu
                Quaternion target = Quaternion.Euler(60, 90, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * tiltSpeedUp);

                if (distance > 0)
                {
                    // Move the menu up
                    transform.position = Vector3.MoveTowards(transform.position, rotatedMenu.transform.position, menuSpeed * Time.deltaTime);
                }
                else if (distance <= 0 && transform.rotation == target)
                {
                    isDone = true;
                }
            }
            else
            {
                float distance = Vector3.Distance(transform.position, flatMenu.transform.position);

                // Rotate the menu
                Quaternion target = Quaternion.Euler(90, 90, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * tiltSpeedDown);
                
                if (distance > 0)
                {
                    // Move the menu down
                    transform.position = Vector3.MoveTowards(transform.position, flatMenu.transform.position, menuSpeed * Time.deltaTime);
                }
                else if (distance <= 0 && transform.rotation == target)
                {
                    isDone = true;
                }
            }
        }
    }

    public void CloseMenu()
    {
        opening = false;
        isDone = false;
    }
}