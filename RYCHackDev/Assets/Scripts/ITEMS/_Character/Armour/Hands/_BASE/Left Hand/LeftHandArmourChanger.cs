using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class LeftHandArmourChanger : MonoBehaviour
    {
        public List<GameObject> leftHandModels; // Reference to all the left hand armour models

        private void Awake()
        {
            GetAllLeftHandModels();
        }
        private void GetAllLeftHandModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                leftHandModels.Add(transform.GetChild(i).gameObject);
            }
        }
        public void UnEquipAllLeftHandArmours()
        {
            foreach (GameObject leftHandArmour in leftHandModels)
            {
                leftHandArmour.SetActive(false);
            }
        }
        public void EquipLeftHandArmourByName(string leftHandName)
        {
            for (int i = 0; i < leftHandModels.Count; i++)
            {
                if (leftHandModels[i].name == leftHandName)
                {
                    leftHandModels[i].SetActive(true);
                }
            }
        }
    }
}