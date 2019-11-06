using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using UnityEngine;

/**
 * This Script uses Unity's raycast behavior to detect the window object
 * that a object is pointing at, retrieve it's handle and window dimensions,
 * then bring the window to the forground and set the cursor location to the
 * center of that window. This script is meant to be applied to an object, in
 * this case the player headset with the ray in the direction the player is facing.
 * 
 */
public class ActiveWindowRaycast : MonoBehaviour
{
    public float distance = 2;
    RaycastHit whatIHit;
    IntPtr hwnd;
    string current_active_window_name;//unused
    public GameObject windows;
    Rect bounds;

    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hWnd);
    [DllImport("user32.dll")]
    static extern bool SetActiveWindow(IntPtr hWnd);
    [DllImport("user32.dll")]
    static extern bool SetFocus(IntPtr hWnd);
    [DllImport("user32.dll")]
    static extern bool AllowSetForegroundWindow(int dwProcessId);
    [DllImport("user32.dll")]
    static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
    [DllImport("kernel32.dll")]
    static extern uint GetCurrentThreadId();
    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

    public struct Rect
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }

    void Start()
    {

    }

    IntPtr curHwnd;
    bool lookingAtSameWindow = false;
    // Update is called once per frame
    void Update()
    {
        
        //InteractRaycastAlternative();
        UnityEngine.Debug.DrawRay(this.transform.position, transform.forward * distance, UnityEngine.Color.cyan);
        //! The raycast of this script draws a line from the object it is attached to and holds data on what intersects this line
        if (Physics.Raycast(this.transform.position, this.transform.forward, out whatIHit, distance))
        {   
            //When the ray collides with an object
            if (whatIHit.collider.gameObject.tag == "WSWindow")
            {   //The tags are read for that object, windows are tagged "WSWindow", so when we raycast an object with that tag
                WorkspaceWindows.windowToRefresh = whatIHit.collider.gameObject.name;
                if (WorkspaceWindows.LookUpHWND(whatIHit.collider.gameObject.name) != hwnd && !lookingAtSameWindow)
                {   /** We lookup it's handle in the window object dictionary in the WorkspaceWindows script
                    /*  If it does not match the last active window recorded in this script as the current active window
                    /*  This script will execute, changing the active window to the one we raycast and moving the cursor 
                    /*  to the center of that window */

                    //! UnityEngine.Debug.Log("LOOKING AT A WINDOW!!!!");
                    //! UnityEngine.Debug.Log(name);
                    hwnd = WorkspaceWindows.LookUpHWND(whatIHit.collider.gameObject.name);//Store window of raycast window as the new active window
                    curHwnd = hwnd;
                    AllowSetForegroundWindow(-1);//Allow all processes to set forground window
                    AttachThreadInput(
                        GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero),
                        GetCurrentThreadId(), false);//Detaches thread input between this script and the forground window

                    //current_active_window_name = whatIHit.collider.gameObject.name;
                    //hwnd = WorkspaceWindows.LookUpHWND(current_active_window_name); //Returns the window's handle from the game object accoiated with it

                    SetForegroundWindow(hwnd);//! Brings window to the forground
                    SetActiveWindow(hwnd);//! Sets window to active, directing input to said window
                    SetFocus(hwnd);//! Sets window to focus, compleating the list of things windows does when you click on an application in the taskbar
                    if (!lookingAtSameWindow)
                    {
                        GetWindowRect(hwnd, ref bounds);//! Gets window's dimensions and passes it to bounds
                                                        //System.Windows.Forms.Cursor cs = new System.Windows.Forms.Cursor(System.Windows.Forms.Cursor.Current.Handle);
                        int centerH = bounds.Left + (bounds.Right - bounds.Left) / 2;//! Finds the horizontal center of our active window
                        int centerV = bounds.Top + (bounds.Bottom - bounds.Top) / 2;//! Finds the vertical center of our active window
                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(centerH, centerV);//! Sets cursor position to the center of our active window
                        System.Windows.Forms.Cursor.Clip = new Rectangle(bounds.Left, bounds.Top, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);//Set's the boundries of where our cursor can go so it does not move off window while it is active
                        lookingAtSameWindow = true;
                    }
                        
                    //Position = new System.Drawing.Point(bounds.Left, bounds.Top);
                    //Clip = new Rectangle(bounds.Left, bounds.Top, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);//doesn't work, don't know why

                    //UnityEngine.Debug.Log("Bounds UPDATE:" + bounds.Top + ':' + bounds.Right + ':' + bounds.Bottom + ':' + bounds.Left);
                    AttachThreadInput(
                        GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero),
                        GetCurrentThreadId(), true);//! Attaches thread input between this script and the forground window, preventing us from losing the ability to continue to set active window
                    //hwnd = IntPtr.Zero;
                }

            }else
            {
                lookingAtSameWindow = false;
            }
        }
        else
        {
            lookingAtSameWindow = false;
            //UnityEngine.Debug.Log("SEEING: Nothing");
        }
    }
}
