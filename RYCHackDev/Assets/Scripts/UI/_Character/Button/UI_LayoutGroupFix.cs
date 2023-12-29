using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Psychain/Layout/Layout Group Fix")]
    public class UI_LayoutGroupFix : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool fixOnEnable = true;
        [SerializeField] private bool fixWithDelay = true;
        float fixDelay = 0.025f;

        void OnEnable()
        {
            if (fixWithDelay == false && fixOnEnable == true) { LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>()); }
            else if (fixWithDelay == true) { StartCoroutine(FixDelay()); }
        }

        public void FixLayout()
        {
            if (fixWithDelay == false) { LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>()); }
            else { StartCoroutine(FixDelay()); }
        }

        IEnumerator FixDelay()
        {
            yield return new WaitForSecondsRealtime(fixDelay);
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}