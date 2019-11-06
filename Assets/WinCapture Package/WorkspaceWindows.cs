using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Valve.VR.InteractionSystem;
using WinCapture;

public class WorkspaceWindows : MonoBehaviour {

    #region INIT
    Shader windowShader;
    Shader desktopShader;
    Shader chromiumShader;
    WindowCaptureManager captureManager;
    
    public static Dictionary<IntPtr, WindowCapture> windowsRendering;
    public static Dictionary<IntPtr, GameObject> windowObjects;
    public static Dictionary<string, IntPtr> windowhwnds;
    public static string windowToRefresh;
    public static bool pollNow;

    //Windows being displayed and render count
    Int32 RenderCount = 0;
    // Windows parent object
    public GameObject windowsParent;

    //ChromiumCapture chromiumCapture;
    GameObject chromiumObject;

    //Placement Areas
    GameObject placementAreaDesk;
    GameObject placementArea1;
    GameObject placementArea2;
    GameObject placementArea3;

    // Lowercase names of windows to render
    //INIT window names to render
    public string[] WindowNames = { "desktop" }; // , "studio", "unity", "task" "desktop","calc","edge","vr","file", "task" , "desktop", "unity"

    // event trigger for when adding a window
    bool LockWindowHwnds;

    // Use this for initialization
    void Start()
    {
        //Sets window scale factor
        windowScale *= windowScaleFactor;

        windowShader = Shader.Find("WinCapture/WindowShader");
        //chromiumShader = Shader.Find("WinCapture/ChromiumShader");

        windowsRendering = new Dictionary<IntPtr, WindowCapture>();
        windowObjects = new Dictionary<IntPtr, GameObject>();
        windowhwnds = new Dictionary<string, IntPtr>();
        captureManager = new WindowCaptureManager();
        captureManager.OnAddWindow += OnAddWindow;
        captureManager.OnRemoveWindow += OnRemoveWindow;
        lastPollWindowsTime = Time.time;

        // INIT placement areas onto scene Canvases
        placementAreaDesk = GameObject.Find("Window_Area_Desk");
        placementArea1 = GameObject.Find("Window_Area_1");
        placementArea2 = GameObject.Find("Window_Area_2");
        placementArea3 = GameObject.Find("Window_Area_dump");

        // Save last update Time
        lastUpdateTime = Time.time;

        // Designates the names of what windows and desktops will be rendered
        // All names will be checked as lowercase, so keep lowercase
        WindowsToRender.AddRange(WindowNames);
        pollNow = false;
    }

    #endregion

    public static List<String> WindowsToRender = new List<string>();
    // Use this fuction to determine what windows will be rendered and displayed
    bool IsGoodWindow(WindowCapture window)
    {
        Debug.Log("Saw window: " + window.windowInfo.title);
        
        //return true; // For Debuigging -- Will display all found windows  -- Worst Case Testing

        //determines windows to render based on title of window
        string windowLowerTitle = window.windowInfo.title.ToLower();
        foreach (string name in WindowsToRender)
        {
            if (windowLowerTitle.Contains(name))
                return true;
        }
        return false;
    }

    int desktopCount = 0;
    int dumpCount = 0;
    /**
    * Determines behavior of what happens when API finds a new system window, and creates objects for it as neccesary
    */
    public void OnAddWindow(WindowCapture window)
    {
        if (!windowsRendering.ContainsKey(window.hwnd) && IsGoodWindow(window))
        {
            // pick which parent object the new window will have
            bool dump = false;
            if (RenderCount < 2)
            {
                windowsParent = placementArea1;
            }
            else if (RenderCount < 4)
            {
                windowsParent = placementArea2;
            }
            else
            {
                windowsParent = placementArea3;
                dump = true;
            }

            // determine where to place the window based on edge of previous window rendered
            // will place 4 windows in each window area, up to 3 window areas  TODO update this for new features
            float nextXposition = 0, nextZposition = 0;
            bool desktop = false;
            if (RenderCount % 4 != 0 && windowScale == 1)
            {
                if (RenderCount % 4 == 1)
                    nextXposition = windowsParent.transform.position.x + 5f;
                else if(RenderCount % 4 == 2)
                    nextXposition = windowsParent.transform.position.x - 5f;
                else if(RenderCount % 4 == 3)
                {
                    nextXposition = windowsParent.transform.position.x - 15f;
                }
            }else if(dump)
            {
                //all windows after 9 will be placed on the dump area - 7.5 in the x position
                nextXposition = windowsParent.transform.position.x - (10f * (RenderCount - 9)); 
            }
            else
            {
                nextXposition = windowsParent.transform.position.x + 15f;
            }


            if (RenderCount < 4  && windowScaleFactor == 2)
            {
                if (RenderCount == 0 || RenderCount == 2)
                    nextXposition = windowsParent.transform.position.x + 10f;
                else if (RenderCount == 1 || RenderCount == 3)
                    nextXposition = windowsParent.transform.position.x - 10f;
            }
            else if (dump)
            {
                //all windows after 9 will be placed on the dump area - 7.5 in the x position
                if(dumpCount < 5)
                    nextXposition = windowsParent.transform.position.x - (10f * (RenderCount - 9));
                else if(dumpCount < 10)
                {
                    nextXposition = windowsParent.transform.position.x - (10f * (RenderCount - 9-5));
                    nextZposition = windowsParent.transform.position.z - 5;
                }
                else if (dumpCount < 15)
                {
                    nextXposition = windowsParent.transform.position.x - (10f * (RenderCount - 9 - 10));
                    nextZposition = windowsParent.transform.position.z - 10;
                }
                else if (dumpCount < 20)
                {
                    nextXposition = windowsParent.transform.position.x - (10f * (RenderCount - 9 - 15));
                    nextZposition = windowsParent.transform.position.z - 15;
                }
                dumpCount++;
                
            }
            else
            {
                //nextXposition = windowsParent.transform.position.x + 10f;
            }
            if (window.isDesktop && windowScaleFactor == 2)
            {
                //windowScaleFactor = 1.25f;
                windowsParent = placementAreaDesk;
                if(desktopCount < 1)
                    nextXposition = windowsParent.transform.position.x;
                else if(desktopCount < 2)                                                          
                    nextXposition = windowsParent.transform.position.x -15f;
                else if (desktopCount < 3)
                    nextXposition = windowsParent.transform.position.x + 15f;
                else if (desktopCount < 4)
                    nextXposition = windowsParent.transform.position.x - 30f;
                else
                    nextXposition = windowsParent.transform.position.x - 30f;
                desktopCount++;
                RenderCount--;
                desktop = true;
            }

            // Create Window Object
            GameObject windowObject = GameObject.CreatePrimitive(PrimitiveType.Plane);

            windowObject.transform.parent = windowsParent.transform;//attachs the window object to the parent trasform
            windowObject.name = window.windowInfo.title;
            windowObject.tag = "WSWindow";
            //
            windowObject.transform.GetComponent<Renderer>().material = new Material(windowShader);
            windowObject.transform.localPosition = new Vector3(nextXposition, 0, nextZposition);//local position of the window rendering
            windowObject.transform.localEulerAngles = new Vector3(90, 0, 0);//local Rotation of the window rendering
            //windowObject.transform.localScale = new Vector3(1.5f, 0, 1.5f);//local scale of the window rendering
            windowsRendering[window.hwnd] = window;
            windowObjects[window.hwnd] = windowObject;
            windowhwnds[window.windowInfo.title] = window.hwnd;

            // Generate the Initial Window Texture & Scale
            byte[] textureBytes = window.GetWindowTexture(out bool didChange, out int width, out int height);
            Texture2D windowTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
            window.windowTexture = windowTexture;
            if(desktop)
            {
                windowObject.transform.localScale = new Vector3(width * 0.0005f, 0.1f, height * 0.0005f);
            }
            else
            {
                windowObject.transform.localScale = new Vector3(width * windowScale, 0.1f, height * windowScale);
            }
            renderWindowUpdates(textureBytes, window, width, height, windowObject);
            windowObject.GetComponent<Renderer>().material.mainTexture = window.windowTexture;
            // Adds SteamVR Interactible Script to the Window Object for picking up
            windowObject.AddComponent<Interactable>();
            windowObject.AddComponent<Throwable>();
            windowObject.layer = LayerMask.NameToLayer("windows");
            var rb = windowObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            //windowObject.AddComponent<Valve.VR.InteractionSystem.Interactable>();

            Debug.Log("Rendered window: " + window.windowInfo.title);
            RenderCount ++;
        }
    }
    /**
    * removes the windows from the scene when passed that windows object
    */
    void OnRemoveWindow(WindowCapture window)
    {
        Debug.Log("removed " + window.windowInfo.title);
        if (windowsRendering.ContainsKey(window.hwnd))
        {
            GameObject windowObjectRemoving = windowObjects[window.hwnd];
            Destroy(windowObjectRemoving);
            windowObjects.Remove(window.hwnd);
            windowsRendering.Remove(window.hwnd);
        }
    }

    float lastPollWindowsTime;

    public float captureRateFps = 30;
    public float windowPollPerSecond = 1;
    float windowScale = 0.0005f;
    public float windowScaleFactor = 2;
    /// Cooroutine for rendering the window textures when updates are needed
    /// has been dispatched to the main thread by updateWindowTextures in parrallel
    public IEnumerator renderWindowUpdates(byte[] textureBytes, WindowCapture window, int width, int height, GameObject windowObject)
    {
        if (windowObject == null || textureBytes == null )
        {
            yield return null;
        }
        // if window size has changed update texture
        if (width != window.windowTexture.width || height != window.windowTexture.height)
        {
            window.windowTexture.Resize(width, height);
            windowObject.transform.localScale = new Vector3(width * windowScale, 0.1f, height * windowScale);
        }

        window.windowTexture.LoadRawTextureData(textureBytes);
        window.windowTexture.Apply();

        yield return null;
    }

    public Dictionary<IntPtr,bool> beenInitiallyRendered;
    // Will get called by the Parallel.ForEach loop in order to check each window
    // for texture update individually on multiple threads
    public void UpdateWindowTextures(object key)
    {
        WindowCapture window = windowsRendering[(IntPtr)key];
        GameObject windowObject = windowObjects[(IntPtr)key];

        if (windowObject == null)
        {
            return;
        }/*
        if (!Win32Funcs.IsWindowVisible(window.hwnd) && beenInitiallyRendered[(IntPtr)key] && (IntPtr) key != IntPtr.Zero && !window.isDesktop)
        {
            return;
        }*/
        byte[] textureBytes = window.GetWindowTexture(out bool didChange, out int width, out int height);

        // Check if texture Changed

        if (didChange)
        {
            // Sends Texture rendering job back to the main thread as a coroutine 
            UnityMainThreadDispatcher.Instance().Enqueue(renderWindowUpdates(textureBytes, window, width, height, windowObject));
            beenInitiallyRendered[(IntPtr)key] = true;
        }
    }

    /// Worker Thread that will start the ForEach loop for each active window
    /// and run the loop each on seperate threads for updating each window
    public void WorkerThread()
    {
        Parallel.ForEach(windowsRendering.Keys, key => this.UpdateWindowTextures(key));
    }

    float lastUpdateTime;
    int count = 0;
    // Update is called once per frame
    void Update()
    {
        float curT = Time.time;
        if (curT - lastUpdateTime > 1.0f / captureRateFps)
        {
            count++;
            //start a thread to execute window update Parallel.ForEach loop
            //workerThread = new Thread(WorkerThread);
            //workerThread.Start();
            Task t = Task.Factory.StartNew(() => {
                Parallel.ForEach(windowsRendering.Keys, key => this.UpdateWindowTextures(key));
            });
            lastUpdateTime = curT;
        }

        #region Poll for New Windows
        // Poll for new windows
        //float curT = Time.time;
        if (curT - lastPollWindowsTime > 1.0f / windowPollPerSecond || pollNow)
        {
            // calls OnAddWindow or OnRemoveWindow above if any windows have been added or removed
            StartCoroutine(captureManager.Poll());
            lastPollWindowsTime = curT;
        }
        #endregion

        // // Creating a Chromium capture Window
        #region Chromium
        /*
        Texture2D chromiumTexture = chromiumCapture.GetChromiumTexture(out didChange);
        if (didChange)
        {
            chromiumObject.GetComponent<Renderer>().material.mainTexture = chromiumTexture;
        }
        chromiumObject.transform.localScale = new Vector3(chromiumTexture.width * windowScale, 0.1f, chromiumTexture.height * windowScale);


        if (Time.frameCount == 400)
        {
            chromiumCapture.SetUrl("http://reddit.com");
        }

        if (Time.time - lastUpdateTime < 1.0f / captureRateFps)
        {
            return;
        }
        else
        {
            lastUpdateTime = Time.time;
        }*/
        #endregion
        
        #region Non-Threaded
        /*
        // Capture each window - Non Job System
        foreach (IntPtr key in windowsRendering.Keys)
        {
            WindowCapture window = windowsRendering[key];
            GameObject windowObject = windowObjects[key];

            if (windowObject == null)
            {
                continue;
            }
            Texture2D windowTexture = window.GetWindowTexture(out didChange);
            if (didChange)
            {
                windowObject.GetComponent<Renderer>().material.mainTexture = windowTexture;
            }
            windowObject.transform.localScale = new Vector3(window.windowWidth * windowScale, 0.1f, window.windowHeight * windowScale);
        }*/
        #endregion

    }
    public static IntPtr LookUpHWND(string name)
    {
        return windowhwnds[name];
    }

    public void LateUpdate()
    {
        // Gets information about position and icon of the cursor so you can render it onto the captured surfaces
        WindowCapture.UpdateCursorInfo();
        //m_JobHandle.Complete();
    }

    private void OnDisable()
    {

    }
}
