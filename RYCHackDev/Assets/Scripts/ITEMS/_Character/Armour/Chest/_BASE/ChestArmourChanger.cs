using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class ChestArmourChanger : MonoBehaviour
    {
        public List<GameObject> chestModels; // Reference to all the chest armours models

        private void Awake()
        {
            GetAllChestModels();
        }
        private void GetAllChestModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                chestModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllChestArmours()
        {
            foreach (GameObject chestArmour in chestModels)
            {
                chestArmour.SetActive(false);
            }
        }
        public void EquipChestArmourByName(string chestName)
        {
            for (int i = 0; i < chestModels.Count; i++)
            {
                if (chestModels[i].name == chestName)
                {
                    chestModels[i].SetActive(true);
                }
            }
        }
    }
}