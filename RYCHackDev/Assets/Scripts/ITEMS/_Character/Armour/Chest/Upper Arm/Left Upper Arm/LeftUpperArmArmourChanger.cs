using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class LeftUpperArmArmourChanger : MonoBehaviour
    {
        public List<GameObject> leftUpperArmModels; // Reference to all the left upper arm armour models

        private void Awake()
        {
            GetAllLeftUpperArmModels();
        }
        private void GetAllLeftUpperArmModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                leftUpperArmModels.Add(transform.GetChild(i).gameObject);
            }
        }
        public void UnEquipAllLeftUpperArmArmours()
        {
            foreach (GameObject leftUpperArmArmour in leftUpperArmModels)
            {
                leftUpperArmArmour.SetActive(false);
            }
        }
        public void EquipLeftUpperArmArmourByName(string leftUpperArmName)
        {
            for (int i = 0; i < leftUpperArmModels.Count; i++)
            {
                if (leftUpperArmModels[i].name == leftUpperArmName)
                {
                    leftUpperArmModels[i].SetActive(true);
                }
            }
        }
    }
}