using UnityEngine;
using System.Collections.Generic;

using Blasphemous.ScreamingSinners.Items;
using Blasphemous.ScreamingSinners.Levels;
using Blasphemous.ScreamingSinners.Timing;
using Blasphemous.ScreamingSinners.Events;

using Blasphemous.ModdingAPI;
using Blasphemous.Framework.Items;
using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using UnityEngine.Windows.Speech;
using System;
using Blasphemous.ScreamingSinners.Beads;





/// <summary>
/// initializes all major handling services
/// </summary>
namespace Blasphemous.ScreamingSinners;

public class ScreamingSinners : BlasMod
{
    // initialize mod class
    public ScreamingSinners() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    // initialize handlers
    internal ItemHandler ItemHandler { get; } = new();
    internal Blasphemous.ScreamingSinners.Events.EventHandler EventHandler { get; } = new();
    internal TimeHandler TimeHandler { get; } = new();

    // initialize special item effects
    internal RB602_Effect _RB602_Effect {  get; private set; }

    protected override void OnInitialize()
    {
        // initialize localization
        LocalizationHandler.RegisterDefaultLanguage("en");

        // initialize config
        SSConfig cfg = ConfigHandler.Load<SSConfig>();
        ConfigHandler.Save(cfg);
    }


    protected override void OnExitGame()
    {
    }

    protected override void OnUpdate()
    {
    }


    /// <summary>
    /// Register all custom things
    /// </summary>
    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        // Beads
        provider.RegisterItem(new StandardRosaryBead("RB601", true).AddEffect(new RB601_Effect(new RB601_Config())));
        provider.RegisterItem(new StandardRosaryBead("RB602", true).AddEffect(new RB602_Effect(new RB602_Config())));
        provider.RegisterItem(new StandardRosaryBead("RB603", true).AddEffect(new RB603_Effect(new RB603_Config())));
        provider.RegisterItem(new StandardRosaryBead("RB609", true).AddEffect(new RB609_Effect(new RB609_Config())));

        // Sword hearts
        //provider.RegisterItem(new StandardSwordHeart("HE501", false).AddEffect(new HealthRegen(_tempHE501)));

        // Quest items
        //provider.RegisterItem(new StandardQuestItem("QI502", false));



        // Level edits
        /*
        provider.RegisterObjectCreator("patio-column", new ObjectCreator(
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Arcs/garden-spritesheet_31 (3)"),
            new NoModifier("Column")));
        */
        provider.RegisterObjectCreator("spikes-patio", new ObjectCreator(
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()));
    }

}
