//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem.Sample
{
    public class TrackpadMovement : MonoBehaviour
    {
        //public ISteamVR_Action_Vector2 MovementAxis;

        private Vector2 trackpad;
        private float Direction;
        private Vector3 moveDirection;

        public GameObject Player;
        public SteamVR_Input_Sources Hand;//Set Hand To Get Input From
        public float speed;
        //public CapsuleCollider Collider;
        public GameObject AxisHand;//Hand Controller GameObject
        public float Deadzone;//the Deadzone of the trackpad. used to prevent unwanted walking.


        void Update()
        {
            //Set size and position of the capsule collider so it maches our head.
            //Collider.height = Player.transform.localPosition.y;
            //Collider.center = new Vector3(Player.transform.localPosition.x, Player.transform.localPosition.y / 2, Player.transform.localPosition.z);

            moveDirection = Quaternion.AngleAxis(Angle(trackpad) + AxisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward;//get the angle of the touch and correct it for the rotation of the controller
            updateInput();
            if (GetComponent<Rigidbody>().velocity.magnitude < speed && trackpad.magnitude > Deadzone)
            {//make sure the touch isn't in the deadzone and we aren't going to fast.
                GetComponent <Rigidbody>().AddForce(moveDirection * 30);
            }
        }

        public static float Angle(Vector2 p_vector2)
        {
            if (p_vector2.x < 0)
            {
                return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
            }
            else
            {
                return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
            }
        }

        private void updateInput()
        {
            trackpad = SteamVR_Actions._default.MovementAxis.GetAxis(Hand);
        }

    }
}