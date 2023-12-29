using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class RightLegArmourChanger : MonoBehaviour
    {
        public List<GameObject> rightLegModels; // Reference to all the right leg armour models

        private void Awake()
        {
            GetAllRightLegModels();
        }
        private void GetAllRightLegModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                rightLegModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllRightLegArmours()
        {
            foreach (GameObject rightLegArmour in rightLegModels)
            {
                rightLegArmour.SetActive(false);
            }
        }

        public void EquipRightLegArmourByName(string rightLegName)
        {
            for (int i = 0; i < rightLegModels.Count; i++)
            {
                if (rightLegModels[i].name == rightLegName)
                {
                    rightLegModels[i].SetActive(true);
                }
            }
        }
    }
}