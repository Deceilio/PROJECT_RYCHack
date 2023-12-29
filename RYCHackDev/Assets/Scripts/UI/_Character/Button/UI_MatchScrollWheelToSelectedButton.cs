using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class UI_MatchScrollWheelToSelectedButton : MonoBehaviour
    {
        [SerializeField] GameObject currentSelected; //Current Selected Game Object
        [SerializeField] GameObject previouslySelected; //Previously Selected Game Object
        [SerializeField] RectTransform currentSelectedTransform; //Current Selected Rect Transform for the Game Object
        
        
        [SerializeField] RectTransform contentPanel; //Content Panel Rect Transform
        [SerializeField] ScrollRect scrollRect; //Scroll Rect Game Object

        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected != null)
            {
                if (currentSelected != previouslySelected)
                {
                    previouslySelected = currentSelected;
                    currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currentSelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPosition =
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

            //BELOW CODE: IT WILL ONLY LOCK THE POSITION OF THE Y AXIS (UP & DOWN)
            newPosition.x = 0;

            contentPanel.anchoredPosition = newPosition;
        }
    }
}