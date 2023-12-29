using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class RightHandArmourChanger : MonoBehaviour
    {
        public List<GameObject> rightHandModels; // Reference to all the right hand armours models

        private void Awake()
        {
            GetAllRightHandModels();
        }
        private void GetAllRightHandModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                rightHandModels.Add(transform.GetChild(i).gameObject);
            }
        }
        public void UnEquipAllRightHandArmours()
        {
            foreach (GameObject rightHandArmour in rightHandModels)
            {
                rightHandArmour.SetActive(false);
            }
        }
        public void EquipRightHandArmourByName(string rightHandName)
        {
            for (int i = 0; i < rightHandModels.Count; i++)
            {
                if (rightHandModels[i].name == rightHandName)
                {
                    rightHandModels[i].SetActive(true);
                }
            }
        }
    }
}