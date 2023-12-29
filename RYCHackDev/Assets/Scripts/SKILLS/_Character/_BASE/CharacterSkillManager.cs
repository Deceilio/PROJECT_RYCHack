using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Deceilio.Psychain
{
    public class CharacterSkillManager : MonoBehaviour
    {
        CharacterManager character; // Reference to the Character Manager script
        [HideInInspector] public UI_SkillWindow skillWindowUI; // Reference to the UI Skill Window script

        [Header("CURRENT SKILL EQUIPPED")]
        public WRLD_SKILL_ITEM currentSkillBeingUsed; // Tells which skill the character is currently using

        [Header("NULL SKILL")]
        public WRLD_SKILL_ITEM nullSkill;  // Empty/Null skill for empty slot

        [Header("SKILL INDEX")]
        public int currentSkillIndex = 0; // Index number for the particular skill in quick slot 

        [Header("CHARACTER SKILLS")]
        public WRLD_SKILL_ITEM[] skillsEquippedSlots = new WRLD_SKILL_ITEM[1]; // Contains all the skills which character has

        [Header("CHARACTER SKILL NAMES")]
        public string[] nameOfAllTheSkillsCharacterHas; // Contains all the name of skills which character have

        private Dictionary<WRLD_SKILL_ITEM, GameObject> instantiatedSkills = new Dictionary<WRLD_SKILL_ITEM, GameObject>(); // Dictionary to not repeat the same skills in the menu
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        protected virtual void Update()
        {
            // BELOW CODE: Access all of the skills which character have and create array of strings of those skills name
            if (skillsEquippedSlots != null)
            {
                nameOfAllTheSkillsCharacterHas = new string[skillsEquippedSlots.Length];

                for (int i = 0; i < skillsEquippedSlots.Length; i++)
                {
                    nameOfAllTheSkillsCharacterHas[i] = skillsEquippedSlots[i].itemName;
                }
            }

            InstantiateSkillsMenuObjects();
        }
        private void InstantiateSkillsMenuObjects()
        {
            int skillsCount = Mathf.Min(skillsEquippedSlots.Length, nameOfAllTheSkillsCharacterHas.Length);

            for (int i = 0; i < skillsCount; i++)
            {
                WRLD_SKILL_ITEM skillItem = skillsEquippedSlots[i];
                string skillText = nameOfAllTheSkillsCharacterHas[i];

                if (!instantiatedSkills.ContainsKey(skillItem))
                {
                    GameObject skillsSlots = Instantiate(skillWindowUI.skillInventorySlot, skillWindowUI.contentPanel.transform);
                    Button skillOptionButton = skillsSlots.GetComponentInChildren<Button>();

                    skillsSlots.GetComponentInChildren<TextMeshProUGUI>().text = skillText;

                    TextMeshProUGUI skillTextComponent = skillOptionButton.GetComponentInChildren<TextMeshProUGUI>();
                    skillTextComponent.text = skillText;

                    skillWindowUI.AddButtonListeners(skillOptionButton, skillTextComponent, skillItem);
                    instantiatedSkills.Add(skillItem, skillsSlots);
                }
            }
        }
    }
}