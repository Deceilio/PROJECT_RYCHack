using UnityEngine;

namespace Adnan.RYCHack
{
    public enum CharacterSlot
    {
        CharacterSlot_01,
        CharacterSlot_02,
        CharacterSlot_03,
        CharacterSlot_04,
        CharacterSlot_05,
        CharacterSlot_06,
        CharacterSlot_07,
        CharacterSlot_08,
        CharacterSlot_09,
        CharacterSlot_10,
        NO_SLOT
    }
    public enum WeaponType
    {
        StraightSword,
        Unarmed,
        SmallShield,
        Shield,
        Bow
    }
    public enum WeaponHoldingType
    {
        RightHanded,
        LeftHanded,
        EitherHanded
    }
    public enum WeaponSkillType
    {
        None,
        Technician,
        PyromancyCaster,
        FaithCaster,
    }
    public enum AttackType
    {
        Light,
        Heavy
        // lightAttack01
        // lightAttack02
        // lightAttack03
        // heavyAttack01
    }
    public enum AmmoType
    {
        Arrow,
        Bolt
    }

    public enum AICombatStyle
    {
        SwordAndShield,
        Claws,
        Archer
    }
    public enum AIAttackActionType
    {
        meleeAttackAction,
        rangedAttackAction
    }
    public enum DamageType
    {
        Physical,
        Fire
    }
    public enum BuffClass
    {
        Physical,
        Fire
    }

    public enum StateParticleType
    {
        poison
    }

    public enum EncumbranceLevel
    {
        Light, // Light roll 
        Medium, // Medium roll
        Heavy, // Heavy roll
        Overloaded // Walk speed only
    }

    public class WRLD_ENUMS : MonoBehaviour
    {

    }
}