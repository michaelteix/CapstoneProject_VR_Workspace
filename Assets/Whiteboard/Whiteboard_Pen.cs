using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!
 * This class is responsible for creating RayCasts for pens/erasers and interacting with the Whiteboard
 * In order to also prevent rotating the object and incorrectly interacting with the Whiteboard this class locks the angle of the object
 * It has two modes tagged with the Throwable script: 
 *    Drawing is where we track the Whiteboard Pen RayCast and tracking the pen angle
 *    Erasing is where the Eraser RayCast is tracked the eraser angle is tracked
 */
public class Whiteboard_Pen : MonoBehaviour
{
    // Start is called before the first frame update
    public float distance = 0.095F;
    //public Color penColor;
    static public RaycastHit touch;
    public WhiteboardScript whiteboard;
    private bool lastTouch;
    private Quaternion lastAngle;
    //private Vector3 lastZ;
    private bool isDrawing;
    private bool isErasing;

    /*!
     * This is the function that dynamically runs when the Unity project is first started
     * This initialize the booleans for checking which object is interacting with the Whiteboard
     */
    void Start()
    {
        // this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard>();
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
     * For both the Pen and the Eraser:
     *      A RayCast is extended and checked when a RayCast hit occurs with an object with the 'Whiteboar' tag.
     *      When the hit occurs, the Color, Position and Touch boolean are then set and passed to the WhiteboardScript.
     *      Lastly, for the Pen the angle gets locked when the RayCast hits the Whiteboard Collider
     */ 
    void Update()
    {
        if(isDrawing)
        {
            //InteractRaycastAlternative();
            UnityEngine.Debug.DrawRay(this.transform.position, transform.forward * distance, Color.magenta);

            if (Physics.Raycast(this.transform.position, this.transform.forward, out touch, distance))
            {

                if (touch.collider.gameObject.tag == "Whiteboard")
                {
                    //UnityEngine.Debug.Log("PEN TOUCHING WHITEBOARD!!!!");
                    //UnityEngine.Debug.Log(name);
                }

                this.whiteboard.SetColor(Color.blue);
                this.whiteboard.SetTouchPos(touch.textureCoord.x, touch.textureCoord.y);
                this.whiteboard.ToggleTouch(true);

                if (!lastTouch)
                {
                    lastTouch = true;
                    lastAngle = transform.rotation;
                    //lastZ = transform.position;
                }
            }
            else
            {
                //UnityEngine.Debug.Log("SEEING: Nothing");
                lastTouch = false;
                this.whiteboard.ToggleTouch(false);
            }
            if (lastTouch)
            {
                transform.rotation = lastAngle;
                //lastZ.z = -0.0468F;
                //transform.position = lastZ;
            }
        }
        else if (isErasing)
        {
            UnityEngine.Debug.DrawRay(this.transform.position, transform.TransformDirection(Vector3.left) * distance, Color.green);

            if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.left), out touch, distance))
            {
                UnityEngine.Debug.Log("SEEING: " + touch);
                this.whiteboard.SetTouchPos(touch.textureCoord.x, touch.textureCoord.y);
                this.whiteboard.ToggleTouch(true);

                if (!lastTouch)
                {
                    lastTouch = true;
                    //lastAngle = transform.rotation;
                    //lastZ = transform.position;
                }
            }
            else
            {
                //UnityEngine.Debug.Log("SEEING: Nothing");
                lastTouch = false;
                this.whiteboard.ToggleTouch(false);
            }
        }

    }
}










/*public class Whiteboard_Pen : MonoBehaviour
{
    
    private RaycastHit touch;
    int distance = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float penHeight = transform.Find("LaserRed").transform.localScale.z;
        Vector3 point = transform.Find("LazerRed").transform.position;
        UnityEngine.Debug.DrawRay(this.transform.position, transform.up * distance, Color.magenta);
        if (Physics.Raycast(this.transform.position, this.transform.forward, out touch, distance))
        {
            //if(!(touch.collider.tag == "Whiteboard"))
            //{
                //return;
            //}

            this.whiteboard = touch.collider.GetComponent<Whiteboard>();
            Debug.Log("Touching");
        }
    }
}*/
