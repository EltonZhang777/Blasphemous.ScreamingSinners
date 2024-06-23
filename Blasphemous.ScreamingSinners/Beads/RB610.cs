using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Blasphemous.ModdingAPI;
using Framework.Inventory;
using Framework.Managers;
using Gameplay.GameControllers.Penitent.Abilities;

using HarmonyLib;

namespace Blasphemous.ScreamingSinners.Beads;

// work-in-progress
/*

[HarmonyPatch(typeof(PrayerUse), "CanUsePrayer", MethodType.Getter)]
class RB610_Effect_PrayerUseUnlimiting
{
    [HarmonyPostfix]
    public static bool UnlimitPrayerUse(ref bool ___result)
    {
        if (Main.ScreamingSinners.ItemHandler.IsEquipped("RB610"))
        {
            ___result = true;
            return false;
        }
        return true;
    }
}

*/