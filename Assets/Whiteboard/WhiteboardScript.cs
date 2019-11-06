using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*!
 *This class is the Whiteboard class: 
 * Its purpose is to generate the texture over which the drawing and the erasing will be happening
 * It has two modes tagged with the Throwable script: 
 *    Drawing is where we track the Whiteboard Pen and pixels are changed according to the pen color
 *    Erasing is where the eraser is tracked and the colored the pixels are returned to their original color
 */
public class WhiteboardScript : MonoBehaviour
{
    private int textureSize = 2048;
    private int strokeSize = 8;
    public Texture2D whiteboardTexture;
    private Color[] colors;
    private Color[] originalColor;

    private bool currentlyTouching, lastTouch;
    private float posX, posY;
    private float lastX, lastY;
    private bool isDrawing;
    private bool isErasing;
    private int eraseSize;
    public GameObject eraser;
    private float eraserWidth;
    private float eraserHeight;

    /*!
     * This is the function that dynamically runs when the Unity project is first started
     * This function renders in a texture over Whiteboard prefab
     * This is also where the variables for starting/stopping both drawing and erasing
     */ 
    public void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        this.whiteboardTexture = new Texture2D(textureSize, textureSize);
        renderer.material.mainTexture = this.whiteboardTexture;
        var tempC = whiteboardTexture.GetPixel(10, 15);
        eraseSize = strokeSize + 10;
        eraserWidth = eraser.GetComponent<MeshRenderer>().bounds.size.x;
        eraserHeight = eraser.GetComponent<MeshRenderer>().bounds.size.y;
        this.originalColor = Enumerable.Repeat<Color>(tempC, eraseSize * eraseSize).ToArray<Color>();
        UnityEngine.Debug.Log(this.originalColor);
        isDrawing = false;
        isErasing = false;
    }
    /*!
     * On pick up start drawing function
     */ 
    public void SetDrawing()
    {
        isDrawing = true;
    }
    /*!
     * On detach stop drawing function
     */
    public void StopDrawing()
    {
        isDrawing = false;
    }
    /*!
     * On pick up start erasing function
     */
    public void SetErase()
    {
        isErasing = true;
    }
    /*!
     * On detach stop erasing function
     */
    public void StopErase()
    {
        isErasing = false;
    }

    /*!
     * This the update function that is continually being called throughout the running program.
     * For both the drawing and erasing, the x/y coordinates represented on the Whiteboard texture are tracked 
     * when either the pen or the eraser is interacting with Whiteboard collider.  
     * In order to create smooth lines, interpolation is used to create points between the changed pixels.
     */ 
    void Update()
    {
        if (isDrawing)
        {
            int x = (int)(posX * textureSize - (strokeSize / 2));
            int y = (int)(posY * textureSize - (strokeSize / 2));

            if (lastTouch)
            {
                whiteboardTexture.SetPixels(x, y, strokeSize, strokeSize, colors);

                for (float t = 0.01F; t < 1.00F; t += 0.01F)
                {
                    int interpolatedX = (int)Mathf.Lerp(lastX, (float)x, t);
                    int interpolatedY = (int)Mathf.Lerp(lastY, (float)y, t);
                    whiteboardTexture.SetPixels(interpolatedX, interpolatedY, strokeSize, strokeSize, colors);
                }

                whiteboardTexture.Apply();
            }

            this.lastX = (float)x;
            this.lastY = (float)y;

            this.lastTouch = this.currentlyTouching;
        }else if(isErasing)
        {
            int x = (int)(posX * textureSize - (eraseSize / 2));
            int y = (int)(posY * textureSize - (eraseSize / 2));

            if (lastTouch)
            {
                whiteboardTexture.SetPixels(x, y, eraseSize, eraseSize, this.originalColor);

                for (float t = 0.01F; t < 1.00F; t += 0.01F)
                {
                    int interpolatedX = (int)Mathf.Lerp(lastX, (float)x, t);
                    int interpolatedY = (int)Mathf.Lerp(lastY, (float)y, t);
                    whiteboardTexture.SetPixels(interpolatedX, interpolatedY, eraseSize, eraseSize, this.originalColor);
                }

                whiteboardTexture.Apply();
            }

            this.lastX = (float)x;
            this.lastY = (float)y;

            this.lastTouch = this.currentlyTouching;
        }
    }
    /*!
     * ToggleTouch is a function that verifies whether an object is touching the Whiteboard
     */ 
    public void ToggleTouch(bool touching)
    {
        this.currentlyTouching = touching;
    }

    /*!
     * This function saves the x/y coordinates when the object touches the Whiteboard
     */ 
    public void SetTouchPos(float x, float y)
    {
        this.posX = x;
        this.posY = y;
    }

    /*!
     * This function sets the color for the changing pixels, depending on which object is interacting with the Whiteboard
     * For drawing: Blue
     * For erasing: Original texture color
     */ 
    public void SetColor(Color color)
    {
        this.colors = Enumerable.Repeat<Color>(color, strokeSize * strokeSize).ToArray<Color>();
    }
}

