using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using UnityEngine;
namespace WinCapture
{
    public class WindowCaptureManager
    {
        public static Dictionary<IntPtr, WindowCapture> windowCapturers; // Holds all the captured windows

        public delegate void AddWindow(WindowCapture windowCapture);

        public delegate void RemoveWindow(WindowCapture windowCapture);

        public event AddWindow OnAddWindow;
        public event RemoveWindow OnRemoveWindow;
        
        public static bool appsAdded = false;
        public static bool appsRemoved = false;
        public static List<WindowCapture> toAdd = new List<WindowCapture>();
        public static List<WindowCapture> toRemove = new List<WindowCapture>();

        WindowsHolder windowsHolder;
        // Apply WinCapture/WindowShader shader to any resulting textures
        public WindowCaptureManager()
        {
            windowsHolder = new WindowsHolder();
            windowsHolder.OnAddWindow += OnAddWindowFound;
            windowsHolder.OnRemoveWindow += OnRemoveWindowFound;

            windowCapturers = new Dictionary<IntPtr, WindowCapture>();

            List<Win32Types.DisplayInfo> monitorInfos = Win32Funcs.GetDisplays();

            for (int i = 0; i < monitorInfos.Count; i++)
            {
                windowCapturers[monitorInfos[i].hwnd] = new WindowCapture(monitorInfos[i].hwnd, true);
                windowCapturers[monitorInfos[i].hwnd].windowInfo.title = "desktopBitBlt" + i;
            }
        }

        void OnAddWindowFound(System.IntPtr hwnd)
        {
            if (!windowCapturers.ContainsKey(hwnd))
            {
                appsAdded = true;
                WindowCapture window = new WindowCapture(hwnd, false);
                windowCapturers[hwnd] = window;
                toAdd.Add(window);
                if (OnAddWindow != null)
                {
                    OnAddWindow(window);
                }
            }
        }

        void OnRemoveWindowFound(System.IntPtr hwnd)
        {
            if (windowCapturers.ContainsKey(hwnd))
            {
                appsRemoved = true;
                toRemove.Add(windowCapturers[hwnd]);
                if (OnRemoveWindow != null)
                {
                    OnRemoveWindow(windowCapturers[hwnd]);
                }

                windowCapturers.Remove(hwnd);
            }
        }

        bool firstPoll = true;

        public IEnumerator Poll()
        {
            // We got the desktop things in our constructor so pass them here
            if (firstPoll)
            {
                firstPoll = false;
                foreach (WindowCapture window in windowCapturers.Values)
                {
                    if (OnAddWindow != null)
                    {
                        OnAddWindow(window);
                    }
                }
            }
            windowsHolder.UpdateWindows();
            yield return new WaitForEndOfFrame();
        }
    }
}