using UnityEngine;

namespace Deceilio.Psychain
{
    public class CharacterWeaponHolderSlot : MonoBehaviour
    {
        [HideInInspector] public WRLD_WEAPON_ITEM currentWeapon; // Check for the current item used by the player
        [HideInInspector] public CharacterManager character; // Reference to the Character Manager script
        [SerializeField] public WRLD_WEAPON_ITEM weaponInHolderSlot;
        [SerializeField] public Transform[] parentOverride; // Transform for the parent gameobject for the sword
        [SerializeField] public bool isLeftHandSlot; // Check for the left hand slot of the player
        [SerializeField] public bool isRightHandSlot; // Check for the right hand slot of the player
        [SerializeField] public bool isBackSlot; //  Check for the back slot of the player

        [SerializeField] public GameObject currentWeaponModel; // Reference to the current/default weapon model gameobject

        private void Start()
        {
            character = GetComponentInParent<CharacterManager>();
        }

        private void Update()
        {
            weaponInHolderSlot = character.characterInventoryManager.leftHandWeapon;
        }

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null) 
            {
                currentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if (currentWeaponModel != null) 
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeaponModel(WRLD_WEAPON_ITEM weaponItem)
        {
            // Unload the weapon and destroy it
            UnloadWeaponAndDestroy();

            if (weaponItem == null)
            {
                // Unload the Weapon
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if (model != null)
            {
                if(isBackSlot)
                {
                    if(parentOverride != null && weaponInHolderSlot != null)
                    {
                        if(weaponInHolderSlot != null && weaponInHolderSlot.weaponType == WeaponType.SmallShield)
                        {
                            model.transform.parent = parentOverride[0];
                        }
                        else if(weaponInHolderSlot != null && weaponInHolderSlot.weaponType == WeaponType.StraightSword)
                        {
                            model.transform.parent = parentOverride[1];
                        }
                    }
                    else
                    {
                        model.transform.parent = transform;
                    }
                }
                else 
                {
                    if(parentOverride != null)
                    {
                        model.transform.parent = parentOverride[0];
                    }
                    else
                    {
                        model.transform.parent = transform;
                    } 
                }

                model.transform.localPosition= Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }
    }
}
