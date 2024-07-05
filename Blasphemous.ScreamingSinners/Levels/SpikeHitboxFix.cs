using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Framework.Managers;
using Tools.Playmaker2.Action;
using Gameplay.GameControllers.Penitent.Damage;

namespace Blasphemous.ScreamingSinners.Levels;

// Replace default trap checker (that's located on the feet)
// with one that's over the whole body
[HarmonyPatch(typeof(Penitent), "OnAwake")]
class Penitent_SpikeDetection_Patch
{
    public static void Prefix(Penitent __instance)
    {
        CheckTrap trapChecker = __instance.GetComponentInChildren<CheckTrap>();
        GameObject holder = trapChecker.gameObject;
        UnityEngine.Object.Destroy(trapChecker);

        BoxCollider2D collider = holder.GetComponent<BoxCollider2D>();

        PenitentDamageArea penitentDamageArea = __instance.GetComponentInChildren<PenitentDamageArea>();
        BoxCollider2D damageCollider = penitentDamageArea.DamageAreaCollider as BoxCollider2D;

        collider.size = new Vector2(
            damageCollider.size.x,
            damageCollider.size.y);
        collider.offset = new Vector2(
            -0.2f,
            -0.1f);

        holder.AddComponent<CheckTrapDerived>();
    }
}
/// <summary>
/// Component added to the player instead of the original that calls special method
/// </summary>
public class CheckTrapDerived : CheckTrap
{
    private void OnTriggerExit2D(Collider2D other)
    {
    }
}

// WIP, still have bugs

/*
// re-size the spike hitbox to the same size of damage hitbox when crouching
[HarmonyPatch(typeof(PenitentDamageArea), "SetBottomSmallDamageArea")]
class PenitentDamageArea_CrouchSpikeDetection_Patch
{
    public static void Postfix()
    {
        Penitent penitent = Core.Logic.Penitent;

        CheckTrap trapChecker = penitent.GetComponentInChildren<CheckTrap>();
        GameObject holder = trapChecker.gameObject;
        UnityEngine.Object.Destroy(trapChecker);

        BoxCollider2D collider = holder.GetComponent<BoxCollider2D>();

        PenitentDamageArea penitentDamageArea = penitent.GetComponentInChildren<PenitentDamageArea>();
        BoxCollider2D damageCollider = penitentDamageArea.DamageAreaCollider as BoxCollider2D;

        collider.size = new Vector2(
            damageCollider.size.x,
            damageCollider.size.y);
        collider.offset = new Vector2(
            -0.2f,
            -0.1f);
        Main.ScreamingSinners.Log($"collider size: {collider.size}\n" +
            $"collider offset: {collider.offset}");
    }
}

// re-size the spike hitbox to the same size of damage hitbox when the damage hitbox resets
[HarmonyPatch(typeof(PenitentDamageArea), "SetDefaultDamageArea")]
class PenitentDamageArea_ResetSpikeDetection_Patch
{
    public static void Postfix()
    {
        Penitent penitent = Core.Logic.Penitent;

        CheckTrap trapChecker = penitent.GetComponentInChildren<CheckTrap>();
        GameObject holder = trapChecker.gameObject;
        UnityEngine.Object.Destroy(trapChecker);

        BoxCollider2D collider = holder.GetComponent<BoxCollider2D>();

        PenitentDamageArea penitentDamageArea = penitent.GetComponentInChildren<PenitentDamageArea>();
        BoxCollider2D damageCollider = penitentDamageArea.DamageAreaCollider as BoxCollider2D;

        collider.size = new Vector2(
            damageCollider.size.x,
            damageCollider.size.y);
        collider.offset = new Vector2(
            -0.2f,
            -0.1f);

        Main.ScreamingSinners.Log($"collider size: {collider.size}\n" +
            $"collider offset: {collider.offset}");
    }
}


*/