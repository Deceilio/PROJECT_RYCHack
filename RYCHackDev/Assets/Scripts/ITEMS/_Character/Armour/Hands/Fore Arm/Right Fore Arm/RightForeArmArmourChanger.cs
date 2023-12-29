using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class RightForeArmArmourChanger : MonoBehaviour
    {
        public List<GameObject> rightForeArmModels; // Reference to all the right fore arm armour models

        private void Awake()
        {
            GetAllRightForeArmModels();
        }
        private void GetAllRightForeArmModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                rightForeArmModels.Add(transform.GetChild(i).gameObject);
            }
        }
        public void UnEquipAllRightForeArmArmours()
        {
            foreach (GameObject rightForeArmArmour in rightForeArmModels)
            {
                rightForeArmArmour.SetActive(false);
            }
        }
        public void EquipRightForeArmArmourByName(string rightForeArmName)
        {
            for (int i = 0; i < rightForeArmModels.Count; i++)
            {
                if (rightForeArmModels[i].name == rightForeArmName)
                {
                    rightForeArmModels[i].SetActive(true);
                }
            }
        }
    }
}