using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem.Sample
{
    public class GrenadeButton : MonoBehaviour
    {
        public HoverButton RedButton;

        public GameObject grenade_prefab;

        private void Start()
        {
            RedButton.onButtonDown.AddListener(OnButtonDown);
        }

        private void OnButtonDown(Hand hand)
        {
            StartCoroutine(CreateGrenades());
        }

        private IEnumerator CreateGrenades()
        {
            GameObject grenade = GameObject.Instantiate<GameObject>(grenade_prefab);
            grenade.transform.position = this.transform.position;
            //grenade.transform.rotation = Quaternion.Euler(0, Random.value * 360f, 0);
            grenade.GetComponentInChildren<MeshRenderer>().material.SetColor("_TintColor", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));

            float startTime = Time.time;
            float overTime = 0.5f;
            float endTime = startTime + overTime;

            Vector3 initialScale = Vector3.one * 0.01f;
            Vector3 targetScale = Vector3.one * (1 + (Random.value * 0.25f));

            while (Time.time < endTime)
            {
                grenade.transform.localScale = Vector3.Slerp(initialScale, targetScale, (Time.time - startTime) / overTime);
                yield return null;
            }

            yield return null;
        }
    }
}