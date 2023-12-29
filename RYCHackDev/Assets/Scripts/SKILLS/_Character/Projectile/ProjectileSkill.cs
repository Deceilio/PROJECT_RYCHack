using UnityEngine;

namespace Deceilio.Psychain
{
    [CreateAssetMenu(menuName = "Psychain/Skills/Projectile Skill")]
    public class ProjectileSkill : WRLD_SKILL_ITEM
    {
        [Header("SKILL PROPERTIES")]
        public int baseDamage; // Base damage from the projectile

        [Header("SKILL DATA")]
        public float projectileForwardVelocity; // Velocity for the projectile
        public float projectileUpwardVelocity; // Upward velocity for the projectile
        public float projectileMass; // Mass/Size of the projectile
        public bool isEffectedByGravity; // Checks if the projectile is effected by gravity or not
        public Vector3 projectileSize; // Local size for thge projectile
        Rigidbody rigidBody; //

        public override void AttemptToCastSkill(CharacterManager character)
        {
            base.AttemptToCastSkill(character);
            // BELOW CODE: Instantiate the skill in the casting hand of the player
            GameObject instantiatedWarmUpSkillFX = Instantiate(skillWarmUpFX, character.characterWeaponSlotManager.rightHandSlot.transform);
            instantiatedWarmUpSkillFX.gameObject.transform.localScale = projectileSize;
            // BELOW CODE: Play animation to cast skill
            character.characterAnimatorManager.PlayTargetActionAnimation(skillAnimation, true);
            Debug.Log("Attempting to cast skill...");
        }
        
        public override void SuccessfullyCastSkill(CharacterManager character)
        {
            base.SuccessfullyCastSkill(character);

            PlayerManager player = character as PlayerManager;

            // BELOW CODE: Handle Player's logic
            if(player != null)
            {
                GameObject instantiatedSkillFX = Instantiate(skillCastFX, player.playerWeaponSlotManager.rightHandSlot.transform.position, player.playerCameraManager.cameraPivotTransform.rotation);
                WRLD_SKILL_DAMAGE_HITBOX skillDamageCollider = instantiatedSkillFX.GetComponent<WRLD_SKILL_DAMAGE_HITBOX>();
                skillDamageCollider.entityTeamIDNumber = player.playerStatsManager.entityTeamIDNumber;
                rigidBody = instantiatedSkillFX.GetComponent<Rigidbody>();
                //skillDamageCollider = instantiatedSkillFX.GetComponent<SkillDamageCollider>();

                if (player.playerCameraManager.currentLockOnTarget != null)
                {
                    instantiatedSkillFX.transform.LookAt(player.playerCameraManager.currentLockOnTarget.transform);
                }
                else
                {
                    instantiatedSkillFX.transform.rotation = Quaternion.Euler(player.playerCameraManager.cameraPivotTransform.eulerAngles.x, player.playerStatsManager.transform.eulerAngles.y, 0);
                }

                rigidBody.AddForce(instantiatedSkillFX.transform.forward * projectileForwardVelocity);
                rigidBody.AddForce(instantiatedSkillFX.transform.up * projectileUpwardVelocity);
                rigidBody.useGravity = isEffectedByGravity;
                rigidBody.mass = projectileMass;
                instantiatedSkillFX.transform.parent = null;
                Debug.Log("Skill cast successful!");
            }
            // BELOW CODE: Handle A.I's logic
            else
            {

            }
        }
    }
}
