using UnityEditor;
using UnityEngine;

namespace Adnan.RYCHack
{
    public class WRLD_PSYCHAIN_MENU_EDITOR : MonoBehaviour
    {
        #region Player Editor
        [MenuItem("Deceilio/Psychain/Player/Player Camera")]
        static void CreatePlayerCamera()
        {
            var go = Resources.Load("Prefabs/Player Objects/Camera/Player Camera Manager");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Deceilio/Psychain/Player/Characters/P1_Nix")]
        static void CreatePlayerObject()
        {
            var go = Resources.Load("Prefabs/Player Objects/Characters/P1_Nix");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Deceilio/Psychain/Player/Player UI Manager")]
        static void CreatePlayerUI()
        {
            var go = Resources.Load("Prefabs/Player Objects/UI/Player UI Manager");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }
        #endregion

        #region A.I Character Editor

        // BOSSES
        [MenuItem("Deceilio/Psychain/A.I Character/Enemies/Boss/E3_Overlord")]
        static void CreateBoss1Advance()
        {
            var go = Resources.Load("Prefabs/AI Character Objects/Characters/Enemy/E3_Overlord_Boss1");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Deceilio/Psychain/A.I Character/Enemies/Boss/E4_Overlord")]
        static void CreateBoss2Advance()
        {
            var go = Resources.Load("Prefabs/AI Character Objects/Characters/Enemy/E4_Paladin_Boss2");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }

        // MOBS
        [MenuItem("Deceilio/Psychain/A.I Character/Enemies/Mobs/E5_Arissa_OnlySword")]
        static void CreateMobs1Advance()
        {
            var go = Resources.Load("Prefabs/AI Character Objects/Characters/Enemy/E5_Arissa_OnlySword");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Deceilio/Psychain/A.I Character/Enemies/Mobs/E6_Erika_Sword&Shield")]
        static void CreateMobs2Advance()
        {
            var go = Resources.Load("Prefabs/AI Character Objects/Characters/Enemy/E6_Erika_Sword&Shield");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Deceilio/Psychain/A.I Character/Enemies/Mobs/E7_Vi")]
        static void CreateMobs3Advance()
        {
            var go = Resources.Load("Prefabs/AI Character Objects/Characters/Enemy/E7_Vi");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }

        // COHORTS
        [MenuItem("Deceilio/Psychain/A.I Character/Cohort/C1_Vi")]
        static void CreateCohort1Advance()
        {
            var go = Resources.Load("Prefabs/AI Character Objects/Characters/Cohort/C1_Vi");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Deceilio/Psychain/A.I Character/Cohort/C2_Sunbird")]
        static void CreateCohort2Advance()
        {
            var go = Resources.Load("Prefabs/AI Character Objects/Characters/Cohort/C2_Sunbird");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }
        #endregion

        #region Menus
        [MenuItem("Deceilio/Psychain/Menus/Leveling Up")]
        static void CreateLevelingUpMenuObject()
        {
            var go = Resources.Load("Prefabs/Menu Objects/Enable Leveling");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }
        #endregion

        #region Props
        [MenuItem("Deceilio/Psychain/Props/Invisible Wall")]
        static void CreateInvisibleWallObject()
        {
            var go = Resources.Load("Prefabs/Props Objects/Invisible Wall");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }
        #endregion

        #region Testing
        [MenuItem("Deceilio/Psychain/Testing/Pickup/Weapon")]
        static void CreatePickupWeapon()
        {
            var go = Resources.Load("Prefabs/Item Objects/Pickup/Pickup Weapon");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Deceilio/Psychain/Testing/Interact/Chest")]
        static void CreateChest()
        {
            var go = Resources.Load("Prefabs/Item Objects/Interacting/Open Chest");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }

        [MenuItem("Deceilio/Psychain/Testing/Pickup/Item/Vial of Vigour")]
        static void CreatePickupVialOfVigour()
        {
            var go = Resources.Load("Prefabs/Item Objects/Pickup/Pickup Vigour");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        }
        [MenuItem("Deceilio/Psychain/Testing/Misc/Console")]
        static void CreateConsoleManager()
        {
            var go = Resources.Load("Prefabs/Misc Objects/World Console Manager");
            Instantiate(go, Vector3.zero, Quaternion.identity);
        } 
        #endregion
    }
}