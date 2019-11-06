//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem.Sample
{
    public class RotateView : MonoBehaviour
    {
        public SteamVR_Action_Boolean Action_RotateViewRight;
        public SteamVR_Action_Boolean Action_RotateViewLeft;
        public SteamVR_Action_Boolean Action_RotateView180;

        public Hand hand;

        public GameObject Player;
        private Player player = null;

        [Header("Audio Sources")]
        public AudioSource headAudioSource;

        [Header("Sounds")]
        public AudioClip teleportSound;


        private void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

            if (Action_RotateViewRight == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No RotateViewRight action assigned");
                return;
            }else if(Action_RotateViewLeft == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No RotateViewLeft action assigned");
                return;
            }else if(Action_RotateView180 == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No RotateView180 action assigned");
                return;
            }

            player = InteractionSystem.Player.instance;

            Action_RotateViewRight.AddOnChangeListener(OnActionChangeRight, hand.handType);
            Action_RotateViewLeft.AddOnChangeListener(OnActionChangeLeft, hand.handType);
            Action_RotateView180.AddOnChangeListener(OnActionChange180, hand.handType);
        }

        private void OnDisable()
        {
            if (Action_RotateViewRight != null)
                Action_RotateViewRight.RemoveOnChangeListener(OnActionChangeRight, hand.handType);
            if (Action_RotateViewLeft != null)
                Action_RotateViewLeft.RemoveOnChangeListener(OnActionChangeLeft, hand.handType);
            if (Action_RotateView180 != null)
                Action_RotateView180.RemoveOnChangeListener(OnActionChange180, hand.handType);
        }

        private void OnActionChangeRight(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
        {
            if (newValue)
            {
                StartCoroutine(Rotate(90));
            }
        }

        private void OnActionChangeLeft(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
        {
            if (newValue)
            {
                StartCoroutine(Rotate(-90));
            }
        }

        private void OnActionChange180(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
        {
            if (newValue)
            {
                StartCoroutine(Rotate(180));
            }
        }

        private IEnumerator Rotate(float degree)
        {
            Player.transform.Rotate(0,degree,0);
            headAudioSource.transform.SetParent(player.hmdTransform);
            headAudioSource.transform.localPosition = Vector3.zero;
            headAudioSource.clip = teleportSound;
            headAudioSource.Play();
            yield return null;
        }
        
    }
}