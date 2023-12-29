using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class HipArmourChanger : MonoBehaviour
    {
        public List<GameObject> hipModels; // Reference to all the hip armour models

        private void Awake()
        {
            GetAllHipModels();
        }
        private void GetAllHipModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                hipModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllHipArmours()
        {
            foreach (GameObject hipArmour in hipModels)
            {
                hipArmour.SetActive(false);
            }
        }

        public void EquipHipArmourByName(string hipName)
        {
            for (int i = 0; i < hipModels.Count; i++)
            {
                if (hipModels[i].name == hipName)
                {
                    hipModels[i].SetActive(true);
                }
            }
        }
    }
}