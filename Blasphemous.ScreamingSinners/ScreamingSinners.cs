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
using Blasphemous.ScreamingSinners.Beads;
using Framework.Managers;





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

    internal LevelFlagHandler LevelFlagHandler { get; } = new();

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

    protected override void OnAllInitialized()
    {
        base.OnAllInitialized();
        Main.ScreamingSinners.EventHandler.OnPlayerKilled += LevelFlagHandler.OnFirstDeath;
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
        
        // spikes
        provider.RegisterObjectCreator("spikes-jondo-tiny", new ObjectCreator(
            new SceneLoader("D03Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/inverted-bell-spritesheet_56"),
            new SpikeModifier(new Vector2(0.9f, 0.8f))));
        provider.RegisterObjectCreator("spikes-jondo", new ObjectCreator(
            new SceneLoader("D03Z02S03_DECO", "MIDDLEGROUND/AfterPlayer/Spikes/inverted-bell-spritesheet_23"),
            new SpikeModifier()));
        provider.RegisterObjectCreator("spikes-jondo-long", new ObjectCreator(
            new SceneLoader("D03Z02S03_DECO", "MIDDLEGROUND/AfterPlayer/Spikes/inverted-bell-spritesheet_25"),
            new SpikeModifier(new Vector2(4f, 0.8f))));
        provider.RegisterObjectCreator("spikes-patio", new ObjectCreator(
            new SceneLoader("D04Z01S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier(new Vector2(2.6f, 0.8f))));
        provider.RegisterObjectCreator("spikes-canvases", new ObjectCreator(
            new SceneLoader("D05Z02S01_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier(new Vector2(3f, 0.8f))));
        provider.RegisterObjectCreator("spikes-rooftops", new ObjectCreator(
            new SceneLoader("D06Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier()));
        provider.RegisterObjectCreator("spikes-brotherhood", new ObjectCreator(
            new SceneLoader("D17BZ02S01_DECO", "MIDDLEGROUND (1)/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()));
        provider.RegisterObjectCreator("spikes-miriam", new ObjectCreator(
            new SceneLoader("D23Z01S05_DECO", "MIDDLEGROUND/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()));

        // ladder
        provider.RegisterObjectCreator("ladder-jondo", new ObjectCreator(
            new SceneLoader("D03Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0}"),
            new LadderModifier()));

        // terrain

        provider.RegisterObjectCreator("platform-droppable-library", new ObjectCreator(
            new SceneLoader("D05Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/library_spritesheet_34"),
            new DroppablePlatformModifier(new Vector2(2f, 1f), new Vector2(0f, -0.3f)))) ;
    }

}
