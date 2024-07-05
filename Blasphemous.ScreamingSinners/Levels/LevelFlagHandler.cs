using Framework.Managers;
using Gameplay.GameControllers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blasphemous.ScreamingSinners.Levels;

/// <summary>
/// Contains the flags that determine some conditional level objects are loaded or not
/// </summary>
internal class LevelFlagHandler
{
    /// <summary>
    /// Called by the EventHandler when player died for the first time
    /// </summary>
    public void OnFirstDeath()
    {
        Core.Events.SetFlag("FIRST_DEATH", true);
        Main.ScreamingSinners.EventHandler.OnPlayerKilled -= Main.ScreamingSinners.LevelFlagHandler.OnFirstDeath;
    }
}
