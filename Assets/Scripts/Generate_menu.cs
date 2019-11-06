/*!
 * \class Generate_menu
 *
 * \brief This class is responsible for in-game menu attached to the watch object, on the players left hand. 
 *
 * \author Michael Teixeira
 *
 * \date $Date: 2019/5/5 14:16:20 $
 *
 * Contact: michael.teixeira@mavs.uta.edu
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;
using WinCapture;

public class Generate_menu : MonoBehaviour
{
    /// <summary>
    /// The current material that is used for rendering the skybox.
    /// </summary>
    private static Material currentSkybox;
    /// <summary>
    /// If true menu has been generated and is active. With initial value of false.
    /// </summary>
    private static bool menuActive = false;
    /// <summary>
    /// Displays whether the hints are currently turned on or not.
    /// </summary>
    private static bool hintsOn;
    /// <summary>
    /// Holds the value of whether the process selector has been populated with values yet or not. Initial value is false.
    /// </summary>
    private static bool menuPopulated = false;
    /// <summary>
    /// Prefab for the process selector menu
    /// </summary>
    private static Canvas processMenu;
    /// <summary>
    /// GameObject in the process selector that holds the toggle object to be added to the menu
    /// </summary>
    private static GameObject viewportContent;
    /// <summary>
    /// The distance that the buttons on the watch in-game menu populate from the center
    /// </summary>
    private int distanceFromCenter = 5;
    /// <summary>
    /// Temporarily hold the material on the button that was just instantiated.
    /// </summary>
    private Renderer tempRen;
    /// <summary>
    /// Holds the names of the in-game menu buttons.
    /// </summary>
    private string[] menuOptions = { "Close", "Change Background", "Active Applications", "Toggle Hints" };
    /// <summary>
    /// Holds the GameObject that the current game object is a child of, used for setting parent on instantiation
    /// </summary>
    private GameObject parent;
    /// <summary>
    /// Is the process menu currently set to active.
    /// </summary>
    public static bool processesSelectable = false;
    /// <summary>
    /// The prefab for the process menu
    /// </summary>
    public Canvas processMenuPrefab;
    /// <summary>
    /// Prefab of the toggle for populating the process selector menu
    /// </summary>
    public GameObject togglePrefab;
    /// <summary>
    /// The prefab for the menu buttons
    /// </summary>
    public GameObject buttonPrefab;
    /// <summary>
    /// The material for the close button
    /// </summary>
    /// 
    public Material Close;
    /// <summary>
    /// The material for the Change Background button
    /// </summary>
    /// 
    public Material ChangeBackground;
    /// <summary>
    /// The material for the Active Applications button
    /// </summary>
    /// 
    public Material ActiveApplications;
    /// <summary>
    /// The material for the toggle hints button
    /// </summary>
    public Material ToggleHints;

    /// <summary>
    /// Sky box material for the beach
    /// </summary>
    /// 
    public Material Beach;
    /// <summary>
    /// Skybox material for the mountains
    /// </summary>
    /// 
    public Material Mountains;
    /// <summary>
    /// Skybox material for the aurora lights
    /// </summary>
    /// 
    public Material Aurora;
    /// <summary>
    /// 360 Video player for the beach
    /// </summary>
    /// 
    public GameObject videoPlayer1;
    /// <summary>
    /// 360 Video player for the mountains
    /// </summary>
    /// 
    public GameObject videoPlayer2;
    /// <summary>
    /// 360 Video player for the Aurora lights
    /// </summary>
    /// 
    public GameObject videoPlayer3;
    int lastIndex;
    

    /// Start is called before the first frame update and runs on startup
    [RuntimeInitializeOnLoadMethod]
    void Start()
    {
        // Set the watches parent to the left hand
        transform.SetParent(GameObject.Find("LeftHand").transform);
        transform.localPosition = new Vector3(-0.063f, 0.04f, -0.155f);

        // Remove the teleporting hints
        GameObject.Find("Teleporting").GetComponent<Teleport>().CancelTeleportHint();

        // Create and setup the appication selector menu
        processMenu = Instantiate(processMenuPrefab) as Canvas;
        processMenu.worldCamera = Camera.main;
        processMenu.transform.SetParent(Camera.main.transform);
        processMenu.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        processMenu.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        processMenu.transform.localRotation = new Quaternion(0, 0, 0, 0);
        processMenu.gameObject.SetActive(false);
        viewportContent = processMenu.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        //videoPlayer1.SetActive(false);
        videoPlayer2.SetActive(false);
        videoPlayer3.SetActive(false);
        lastIndex = 0;
    }

    /// <summary>
    /// Called when the watch has been clicked, and generates all the menu buttons
    /// </summary>
    public void Create_menu()
    {
        if (!menuActive)
        {
            menuActive = true;
            if (GameObject.Find("WatchCanvas") == null)
            {
                Debug.LogError("Could not find Watch Game Object!");
            }
            else
            {
                parent = GameObject.Find("WatchCanvas");
            }

            for (int i = 0; i < menuOptions.Length; i++)
            {
                GameObject tempButton = Instantiate(buttonPrefab) as GameObject;
                tempButton.name = menuOptions[i];
                tempButton.transform.SetParent(parent.transform);
                tempButton.transform.localScale = buttonPrefab.transform.localScale;
                tempButton.transform.localRotation = buttonPrefab.transform.localRotation;
                tempRen = tempButton.GetComponent<Renderer>();

                switch (i)
                {
                    case 0:
                        tempRen.material = Close;
                        break;
                    case 1:
                        tempRen.material = ChangeBackground;
                        break;
                    case 2:
                        tempRen.material = ActiveApplications;
                        break;
                    case 3:
                        tempRen.material = ToggleHints;
                        break;
                }


                //code from https://www.youtube.com/watch?v=ubVefEN5k2w @ 32:40
                float theta = (2 * Mathf.PI / menuOptions.Length) * i;
                float xPos = Mathf.Sin(theta);
                float yPos = Mathf.Cos(theta);

                tempButton.transform.localPosition = new Vector3(xPos, yPos, 0.3f) * distanceFromCenter;
            }
        }
    }

    /// <summary>
    /// Closes the watch menu and removes all the buttons
    /// </summary>
    private void Destroy_menu()
    {
        if (menuActive)
        {
            GameObject watchCanvas = GameObject.Find("WatchCanvas");
            int numChildren = watchCanvas.transform.childCount;

            for (int i = 1; i < numChildren; i++)
            {
                Destroy(watchCanvas.transform.GetChild(i).gameObject);
            }

            menuActive = false;
        }
    }

    /// <summary>
    /// Activates all the controller hints
    /// </summary>
    private void GenerateHints()
    {
        foreach (Hand hand in Player.instance.hands)
        {
            ControllerButtonHints.HideAllTextHints(hand);

            for (int actionIndex = 0; actionIndex < SteamVR_Input.actionsIn.Length; actionIndex++)
            {
                ISteamVR_Action_In action = SteamVR_Input.actionsIn[actionIndex];
                if (action.GetActive(hand.handType))
                {
                    ControllerButtonHints.ShowButtonHint(hand, action);
                    ControllerButtonHints.ShowTextHint(hand, action, action.GetShortName());
                }
            }
        }
        hintsOn = true;
    }

    /// <summary>
    /// Removes the all controller hints
    /// </summary>
    private void RemoveHints()
    {
        foreach (Hand hand in Player.instance.hands)
        {
            ControllerButtonHints.HideAllButtonHints(hand);
            ControllerButtonHints.HideAllTextHints(hand);
        }
        hintsOn = false;
    }

    /// <summary>
    /// Closes the process selector menu
    /// </summary>
    public void RemoveProcessSelector()
    {
        processMenu.gameObject.SetActive(false);
        processesSelectable = false;
    }

    /// <summary>
    /// Gets called when a button on the menu is clicked, then dispatches the appropriate action.
    /// </summary>
    public void CallAction()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(name);

        if (name == "Close")
        {
            Destroy_menu();
        }
        else if (name == "Change Background")
        {
            if (currentSkybox == null || currentSkybox == Aurora)
            {
                videoPlayer1.SetActive(true);
                RenderSettings.skybox = Beach;
                currentSkybox = Beach;
            }
            else if (currentSkybox == Beach)
            {
                videoPlayer1.SetActive(false);
                videoPlayer2.SetActive(true);
                RenderSettings.skybox = Mountains;
                currentSkybox = Mountains;
            }
            else if (currentSkybox = Mountains)
            {
                videoPlayer2.SetActive(false);
                videoPlayer3.SetActive(true);
                RenderSettings.skybox = Aurora;
                currentSkybox = Aurora;
            }
        }
        else if (name == "Toggle Hints")
        {
            if (hintsOn)
            {
                RemoveHints();
            }
            else
            {
                GenerateHints();
            }
        }
        else if (name == "Active Applications")
        {
            if (!processesSelectable)
            {
                if (!menuPopulated)
                {
                    // Populate appication selector menu
                    foreach (IntPtr key in WindowCaptureManager.windowCapturers.Keys)
                    {
                        viewportContent.GetComponent<ProcessMenu>().AddToList(WindowCaptureManager.windowCapturers[key].windowInfo.title);
                    }
                    menuPopulated = true;
                }

                processMenu.gameObject.SetActive(true);
                processesSelectable = true;
                Destroy_menu();
            }
        }
        else
        {
            Debug.LogError("Invalid button");
        }
    }

    /// Update is called once per frame
    void Update()
    {
        if(processesSelectable)
        {
            if (WindowCaptureManager.appsAdded)
            {
                WindowCaptureManager.appsAdded = false;

                foreach (WindowCapture window in WindowCaptureManager.toAdd)
                {
                    viewportContent.GetComponent<ProcessMenu>().AddToList(window.windowInfo.title);
                    WindowCaptureManager.toAdd.Remove(window);
                }
            }

            if (WindowCaptureManager.appsRemoved)
            {
                WindowCaptureManager.appsRemoved = false;

                foreach (WindowCapture window in WindowCaptureManager.toRemove)
                {
                    viewportContent.GetComponent<ProcessMenu>().RemoveFromList(window.windowInfo.title);
                    WindowCaptureManager.toRemove.Remove(window);
                }
            }
        }
    }
}
