using Framework.Managers;
using Gameplay.GameControllers.Enemies.Framework.Damage;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent.Abilities;
using Gameplay.GameControllers.Penitent.Animator;
using Gameplay.GameControllers.Penitent.Attack;
using Gameplay.GameControllers.Penitent.Damage;
using HarmonyLib;
using Steamworks;
using Tools.Level.Interactables;

namespace Blasphemous.ScreamingSinners.Events;

[HarmonyPatch(typeof(Entity), "KillInstanteneously")]
class Penitent_Death_Patch
{
    public static void Postfix(Entity __instance) => Main.ScreamingSinners.EventHandler.KillEntity(__instance);
}

[HarmonyPatch(typeof(PrieDieu), "OnUse")]
class PrieDieu_Use_Patch
{
    public static void Prefix() => Main.ScreamingSinners.EventHandler.UsePrieDieu();
}

[HarmonyPatch(typeof(PenitentDamageArea), "RaiseDamageEvent")]
class Penitent_Damage_Patch
{
    public static void Prefix(ref Hit hit) => Main.ScreamingSinners.EventHandler.DamagePlayer(ref hit);
}

[HarmonyPatch(typeof(EnemyDamageArea), "TakeDamageAmount")]
class Enemy_Damage_Patch
{
    public static void Prefix(ref Hit hit) => Main.ScreamingSinners.EventHandler.DamageEnemy(ref hit);
}

[HarmonyPatch(typeof(Parry), "RaiseParryEvent")]
class Penitent_Parry_Start_Patch
{
    public static void Prefix(Parry __instance) => Main.ScreamingSinners.EventHandler.ParryStart(__instance);
}

[HarmonyPatch(typeof(Parry), "CheckParry")]
class Penitent_Parry_Riposte_Patch
{
    public static void Postfix(Parry __instance)
    {
        Main.ScreamingSinners.EventHandler.ParryRiposte(__instance);
    }
}

[HarmonyPatch(typeof(GuardSlide), "CastSlide")]
class Penitent_Parry_GuardSlide_Patch
{
    public static void Postfix(Hit hit, GuardSlide __instance)
    {
        Main.ScreamingSinners.EventHandler.ParryGuardSlide(__instance);
    }
}

[HarmonyPatch(typeof(Parry), "StopParry")]
class Penitent_Parry_Fail_Patch
{
    public static void Prefix(Parry __instance)
    {
        Main.ScreamingSinners.EventHandler.ParryFail(__instance);
    }
}

[HarmonyPatch(typeof(PenitentAttackAnimations), "SetSwordSlash")]
class Penitent_Attack_Patch
{
    public static void Postfix(PenitentAttackAnimations __instance, PenitentSword.AttackType type)
    {
        Main.ScreamingSinners.EventHandler.PlayerAttack(__instance);
        if (type == PenitentSword.AttackType.AirUpward 
            || type == PenitentSword.AttackType.GroundUpward)
        {
            Main.ScreamingSinners.EventHandler.PlayerUpwardAttack(__instance);
        }
    }
}
