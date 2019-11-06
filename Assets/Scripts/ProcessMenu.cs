/*!
 * \class ProcessMenu
 *
 * \brief This class is responsible for handling the data in the process selector menu
 *
 * \author Michael Teixeira
 *
 * \date $Date: 2019/5/5 $
 *
 * Contact: michael.teixeira@mavs.uta.edu
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WinCapture;

public class ProcessMenu : MonoBehaviour
{
    /// <summary>
    /// The prefab for the toggle bar that populates the process selector
    /// </summary>
    public GameObject ToggleBar;
    /// <summary>
    /// The GameObject that holds the scripts for managing the window in the game
    /// </summary>
    private GameObject Windows;
    /// <summary>
    /// GameObject that holds the process selector for setting it active and inactive
    /// </summary>
    public GameObject parent;
    /// <summary>
    /// GameObject that holds the scrollbar for the process selector menu 
    /// </summary>
    public Scrollbar scrollbar;

    /// <summary>
    /// Gets and sets the Windows Gameobject at start up
    /// </summary>
    private void Start()
    {
        Windows = GameObject.Find("Windows");
        if (!Windows)
            Debug.LogError("Could not find Windows Parent Object");
    }

    /// <summary>
    /// Cycle through the toggles that are turned on, and open the coresponding program in a seperate window
    /// </summary>
    public void OpenProcess()
    {
        // search through list of toggles
        foreach (Transform child in transform)
        {
            // check if each toggle is selected
            if (child.gameObject.GetComponent<Toggle>().isOn)
            {
                // if yes open that app in a seperate window
                string name = child.gameObject.name.ToLower();
                WorkspaceWindows.WindowsToRender.Add(name);

                foreach (WindowCapture window in WindowCaptureManager.windowCapturers.Values)
                {
                    if (window.windowInfo.title.ToLower().Contains(name) )
                    {
                        Windows.GetComponent<WorkspaceWindows>().OnAddWindow(window);
                    }
                }

                child.gameObject.GetComponent<Toggle>().isOn = false;
            }
        }
        parent.gameObject.SetActive(false);
        Generate_menu.processesSelectable = false;
    }

    /// <summary>
    /// When a new program that needs to get displayed in the process selector menu this function gets called and add that process to the list
    /// </summary>
    /// <param name="name">The name of the program that needs to be opened</param>
    public void AddToList(string name)
    {
        if (name.Length < 2)
            return;

        foreach (IntPtr key in WorkspaceWindows.windowObjects.Keys)
        {
            if (WorkspaceWindows.windowObjects[key].name == name)
            {
                return; // Already rendered so no need to duplicate
            }
        }

        GameObject tempToggle = Instantiate(ToggleBar, transform, false) as GameObject;
        tempToggle.name = name;
        tempToggle.transform.GetChild(1).GetComponent<Text>().text = name;
    }

    /// <summary>
    /// When a new program that needs to get removed from the process selector menu this function gets called and removes that process from the list
    /// </summary>
    /// <param name="name">The name of the program that needs to be removed</param>
    public void RemoveFromList(string name)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == name)
            {
                Destroy(child);
            }
        }
    }

    /// <summary>
    /// Gets called when the button above the scrollbar is clicked and add 0.2f to the scrollbar value
    /// </summary>
    public void SliderUp()
    {
        scrollbar.value += 0.2f;
    }

    /// <summary>
    /// Gets called when the button below the scrollbar is clicked and subracts 0.2f from the scrollbar value
    /// </summary>
    public void SliderDown()
    {
        scrollbar.value -= 0.2f;
    }
}
