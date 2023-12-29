using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class RightUpperArmArmourChanger : MonoBehaviour
    {
        public List<GameObject> rightUpperArmModels; // Reference to all the right upper arm armour models

        private void Awake()
        {
            GetAllRightUpperArmModels();
        }
        private void GetAllRightUpperArmModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                rightUpperArmModels.Add(transform.GetChild(i).gameObject);
            }
        }
        public void UnEquipAllRightUpperArmArmours()
        {
            foreach (GameObject rightUpperArmArmour in rightUpperArmModels)
            {
                rightUpperArmArmour.SetActive(false);
            }
        }
        public void EquipRightUpperArmArmourByName(string rightUpperArmName)
        {
            for (int i = 0; i < rightUpperArmModels.Count; i++)
            {
                if (rightUpperArmModels[i].name == rightUpperArmName)
                {
                    rightUpperArmModels[i].SetActive(true);
                }
            }
        }
    }
}