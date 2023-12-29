using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class ChestInteractableManager : MonoBehaviour
    {
        public string[] targetItemsName; // Name for all the targeted items
        public Transform[] chestItemTransform; // Transform for the chest itmes
        public WRLD_WEAPON_ITEM itemInChest; // Reference to the items in chest
        void Update()
        {
            chestItemTransform = new Transform[targetItemsName.Length];

            for(int i = 0; i < targetItemsName.Length; i++)
            {
                Transform targetItemObject = transform.Find(targetItemsName[i]);

                if(targetItemObject != null)
                {
                    chestItemTransform[i] = targetItemObject.transform;
                }
            }

            foreach(Transform targetItemTransform in chestItemTransform)
            {
                if(targetItemTransform != null)
                {
                    if (targetItemTransform.childCount == 0)
                    {
                        StartCoroutine(WaitToDestoryTheChest());
                    }
                    else if (targetItemTransform.childCount != 0)
                    {
                        PickupInteractableWeapon weaponPickup = targetItemTransform.Find("Weapon").GetComponent<PickupInteractableWeapon>();
                        if (weaponPickup != null)
                        {
                            weaponPickup.weapon = itemInChest;
                        }
                    }
                }
            }
        }
        IEnumerator WaitToDestoryTheChest()
        {
            yield return new WaitForSeconds(8f);
            Destroy(gameObject);
        }
    }
}