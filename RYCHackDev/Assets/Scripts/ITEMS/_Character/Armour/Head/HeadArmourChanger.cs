using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class HeadArmourChanger : MonoBehaviour
    {
        public List<GameObject> headModels; // Reference to all the head armours models

        private void Awake()
        {
            GetAllHeadModels();
        }
        private void GetAllHeadModels()
        {
            int childrenGameObjects = transform.childCount;

            for(int i = 0; i < childrenGameObjects; i++)
            {
                headModels.Add(transform.GetChild(i).gameObject);
            }    
        }

        public void UnEquipAllHeadArmours()
        {
            foreach (GameObject headArmour in headModels)
            {
                headArmour.SetActive(false);
            }
        }

        public void EquipHeadArmourByName(string headName)
        {
            for (int i = 0; i < headModels.Count; i++)
            {
                if (headModels[i].name == headName)
                {
                    headModels[i].SetActive(true);
                }
            }
        }
    }
}