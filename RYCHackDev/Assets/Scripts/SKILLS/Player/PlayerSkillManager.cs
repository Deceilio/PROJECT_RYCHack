using UnityEngine;

namespace Deceilio.Psychain
{
    public class PlayerSkillManager : CharacterSkillManager
    {
        [HideInInspector] public PlayerManager player; // Reference to thw Player Manager script
        [HideInInspector] public GameObject rightPanel; // Reference to the right panel for the parent object
        [HideInInspector] public Transform skillScreenWindow; // Reference to the skill window game object  
        
        private void Start()
        {
            currentSkillBeingUsed = skillsEquippedSlots[currentSkillIndex];
            player.quickSlotsUI.UpdateSkillQuickSlotUI(currentSkillBeingUsed);
        }
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();

            rightPanel = GameObject.Find("Right Panel");
            skillScreenWindow = rightPanel.transform.Find("Skills Screen Window");
            Transform skillWindowTransform = skillScreenWindow.Find("Skill Window");
            skillWindowUI = skillWindowTransform.gameObject.GetComponent<UI_SkillWindow>();
        }
        protected override void Update()
        {
            base.Update();
        }
        public void ChangeSkill()
        {
            if(player.isGrounded)
            {
                currentSkillIndex = currentSkillIndex + 1;

                if (currentSkillIndex > skillsEquippedSlots.Length - 1)
                {
                    currentSkillIndex = -1;
                    LoadSkillOnSlot(nullSkill);
                }
                else if (skillsEquippedSlots[currentSkillIndex] != null)
                {
                    currentSkillBeingUsed = skillsEquippedSlots[currentSkillIndex];
                    LoadSkillOnSlot(skillsEquippedSlots[currentSkillIndex]);
                }
                else
                {
                    currentSkillIndex = currentSkillIndex + 1;
                }
            }
        }
        public void LoadSkillOnSlot(WRLD_SKILL_ITEM skillItem)
        {
            currentSkillBeingUsed = skillItem;
            player.quickSlotsUI.UpdateSkillQuickSlotUI(skillItem);
        }
    }
}