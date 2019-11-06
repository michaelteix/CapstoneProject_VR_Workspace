using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustDeskHeight : MonoBehaviour
{
    public GameObject ControllerLeft;
    public GameObject ControllerRight;
    public GameObject Camera;
    public GameObject Desktop;
    public GameObject ResetHints;
    public GameObject timer;
    public float distance = 0.5f;
    public float offset = -0.075f;
    UnityEngine.UI.Text timertext;
    int timeLeft = 3;
    bool active;

    // Start is called before the first frame update
    void Start()
    {
        ResetHints.SetActive(false);
        timertext = timer.GetComponent<UnityEngine.UI.Text>();
        ResetHints.transform.SetParent( Camera.transform );
        ResetHints.transform.position = Camera.transform.position + Camera.transform.forward * distance;
        ResetHints.transform.rotation = new Quaternion(0,0,0,0);
        active = false;
    }
    /**
    * Will initate the 3 second countdown for the desk and it's child
    * objecgs to set it's y position to that of the desks with a proper offset to match your real world desk
    */
    public void ResetDeskHeight()
    {
        ResetHints.SetActive(true);
        StartCoroutine("LoseTime");
        Time.timeScale = 1;
        active = true;
    }

    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            if(timeLeft == 0)
            {
                GameObject Controller = GameObject.Find("Accessory");
                if(!Controller)
                {
                    if (ControllerLeft)
                        Controller = ControllerLeft;
                    else if (ControllerRight)
                        Controller = ControllerRight;
                    else
                        break;
                }
                var newY = Controller.transform.position.y - Desktop.transform.position.y + offset;
                transform.position = new Vector3(transform.position.x, transform.position.y + newY, transform.position.z);
                break;
            }
        }
        ResetHints.SetActive(false);
        timeLeft = 3;
        active = false;
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        if(active)
            timertext.text = ("" + timeLeft);
    }
}
