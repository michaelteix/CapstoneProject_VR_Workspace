using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem.Sample
{
    /**
    * Activates all the game objects in the scene at the press of button
    */
    public class GamesButton : MonoBehaviour
    {
        public HoverButton gameButton;

        GameObject prefab;

        //Get Game objects in scene
        public GameObject games;
        public GameObject longbow;
        public GameObject remotes;
        public GameObject Buggy;
        public GameObject JoeJeff;
        public GameObject GamePlaneRemotes;
        public GameObject GamePlaneBow;
        public UnityEngine.UI.Text titleText;

        // determines if in play mode (pushed button to play)
        // or not (pushed button again to exit
        bool playing;

        Vector3 initialBuggyPosition;
        Vector3 initialJoePosition;
        Quaternion initialBuggyRotation;
        Quaternion initialJoeRotation;

        private void Start()
        {
            gameButton.onButtonDown.AddListener(OnButtonDown);

            // saves intial position of buggy and JoeJeff
            initialBuggyPosition = Buggy.transform.position;
            initialJoePosition = JoeJeff.transform.position;
            initialBuggyRotation = Buggy.transform.rotation;
            initialJoeRotation = JoeJeff.transform.rotation;

            // disables games on start up
            longbow.SetActive(false);
            remotes.SetActive(false);

            playing = false;
        }

        private void OnButtonDown(Hand hand)
        {
            if(playing)  // get rid of games
            {
                StartCoroutine(EndGames());
                titleText.text = "Need another Break?";
                playing = false;
            }
            else // Start Games
            {
                StartCoroutine(PlayGames());
                titleText.text = "Get back to Work?";
                playing = true;
            }
        }

        private IEnumerator PlayGames()
        {
            longbow.SetActive(true);
            remotes.SetActive(true);
            GamePlaneRemotes.SetActive(true);
            GamePlaneBow.SetActive(true);
            yield return null;
        }

        private IEnumerator EndGames()
        {
            longbow.SetActive(false);
            remotes.SetActive(false);
            GamePlaneRemotes.SetActive(false);
            GamePlaneBow.SetActive(true);
            if (Buggy == null)
            {
                // re- instantiate buggy
            }
            Buggy.transform.position = initialBuggyPosition;
            Buggy.transform.rotation = initialBuggyRotation;
            JoeJeff.transform.position = initialJoePosition;
            JoeJeff.transform.rotation = initialJoeRotation;

            yield return null;
        }
    }
}