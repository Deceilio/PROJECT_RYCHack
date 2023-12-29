using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class LeftLegArmourChanger : MonoBehaviour
    {
        public List<GameObject> leftLegModels; // Reference to all the left leg armour models

        private void Awake()
        {
            GetAllLeftLegModels();
        }
        private void GetAllLeftLegModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                leftLegModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllLeftLegArmours()
        {
            foreach (GameObject leftLegArmour in leftLegModels)
            {
                leftLegArmour.SetActive(false);
            }
        }

        public void EquipLeftLegArmourByName(string leftLegName)
        {
            for (int i = 0; i < leftLegModels.Count; i++)
            {
                if (leftLegModels[i].name == leftLegName)
                {
                    leftLegModels[i].SetActive(true);
                }
            }
        }
    }
}
