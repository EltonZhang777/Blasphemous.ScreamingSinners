using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blasphemous.Framework.Items;
using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using UnityEngine;

namespace Blasphemous.ScreamingSinners.Beads;

class RB609_Effect : ModItemEffectOnEquip
{
    private float _regularAttackReduction;
    private float _extremeAttackReduction;
    private RawBonus _regularFervourRegenBoost;
    private RawBonus _extremeFervourRegenBoost;
    
    private float _currentTimer = 0f;
    private float _updateInterval = 0.25f;

    private RB609_Config _config;

    private enum EffectState
    {
        Inactive,
        Active_Regular,
        Active_Extreme
    }

    private EffectState _currentEffectState;

    public RB609_Effect(RB609_Config config)
    {
        Main.ScreamingSinners.EventHandler.OnEnemyDamaged += ReduceAttackDamage;

        _config = config;

        _regularAttackReduction = _config.regularAttackReduction;
        _extremeAttackReduction = _config.extremeAttackReduction;
        _regularFervourRegenBoost = new RawBonus(_config.regularFervourRegenBoost);
        _extremeFervourRegenBoost = new RawBonus(_config.extremeFervourRegenBoost);
    }

    /// <summary>
    /// check whether current fervour is enough for prayer cast
    /// </summary>
    private bool IsFervourEnoughForPrayer()
    {
        return Core.Logic.Penitent.Stats.Fervour.Current
            > Core.Logic.Penitent.PrayerCast.GetEquippedPrayer().fervourNeeded
            + Core.Logic.Penitent.Stats.PrayerCostAddition.Final;
    }

    private void ReduceAttackDamage(ref Hit hit)
    {
        if (_currentEffectState == EffectState.Inactive 
            || hit.DamageElement != DamageArea.DamageElement.Normal) 
        { return; }

        if (_currentEffectState == EffectState.Active_Regular)
        {
            hit.DamageAmount *= (1 + _regularAttackReduction);
        }
        else if (_currentEffectState == EffectState.Active_Extreme)
        {
            hit.DamageAmount *= (1 + _extremeAttackReduction);
        }
    }

    protected override void Update()
    {
        if (!Main.ScreamingSinners.ItemHandler.IsEquipped("RB609")) { return; }

        _currentTimer += Time.deltaTime;
        if (_currentTimer >= _updateInterval)
        {
            _currentTimer = 0f;
            RemoveEffect();
            ApplyEffect();
        }
    }

    protected override void ApplyEffect()
    {
        if (!IsFervourEnoughForPrayer()) // current fervour not enough to cast prayer
        {
            Core.Logic.Penitent.Stats.FervourStrength.AddRawBonus(_extremeFervourRegenBoost);
            _currentEffectState = EffectState.Active_Extreme;
        }
        else 
        {
            Core.Logic.Penitent.Stats.FervourStrength.AddRawBonus(_regularFervourRegenBoost);
            _currentEffectState = EffectState.Active_Regular;
        }
    }

    protected override void RemoveEffect()
    {
        switch (_currentEffectState)
        {
            case EffectState.Active_Extreme:
                Core.Logic.Penitent.Stats.FervourStrength.RemoveRawBonus(_extremeFervourRegenBoost);
                break;

            case EffectState.Active_Regular:
                Core.Logic.Penitent.Stats.FervourStrength.RemoveRawBonus(_regularFervourRegenBoost);
                break;
        }
        _currentEffectState = EffectState.Inactive;
    }

}

public class RB609_Config
{
    public float regularAttackReduction = -0.75f;
    public float extremeAttackReduction = -0.95f;
    public float regularFervourRegenBoost = 3f;
    public float extremeFervourRegenBoost = 4.5f;
}