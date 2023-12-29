using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class LeftForeArmArmourChanger : MonoBehaviour
    {
        public List<GameObject> leftForeArmModels; // Reference to all the left fore arm armour models

        private void Awake()
        {
            GetAllLeftForeArmModels();
        }
        private void GetAllLeftForeArmModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                leftForeArmModels.Add(transform.GetChild(i).gameObject);
            }
        }
        public void UnEquipAllLeftForeArmArmours()
        {
            foreach (GameObject leftForeArmArmour in leftForeArmModels)
            {
                leftForeArmArmour.SetActive(false);
            }
        }
        public void EquipLeftForeArmArmourByName(string leftForeArmName)
        {
            for (int i = 0; i < leftForeArmModels.Count; i++)
            {
                if (leftForeArmModels[i].name == leftForeArmName)
                {
                    leftForeArmModels[i].SetActive(true);
                }
            }
        }
    }
}