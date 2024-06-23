using Blasphemous.Framework.Items;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using Gameplay.GameControllers.Penitent.Abilities;
using Framework.FrameworkCore.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using JetBrains.Annotations;

namespace Blasphemous.ScreamingSinners.Beads;

class RB602_Effect : ModItemEffectOnEquip
{
    private int _currentStacks = 0;
    private int _regularStackReq;
    private int _extremeStackReq;
    private RawBonus _attackBuff;

    private enum EffectState
    {
        Inactive,
        Active_Regular,
        Active_Extreme
    }
    private EffectState _currentEffectState = EffectState.Inactive;

    public RB602_Effect(RB602_Config config)
    {
        _regularStackReq = config.regularStackReq;
        _extremeStackReq = config.extremeStackReq;
        _attackBuff = new(config.attackBuff);
    }

    private void IncreaseStack_Parry(Parry parry)
    {
        if (_currentEffectState == EffectState.Active_Extreme) { return; }
        
        _currentStacks += 1;
        Main.ScreamingSinners.Log($"RB602's current stack: {_currentStacks}");

        if (_currentStacks == _regularStackReq)
        {
            _currentEffectState = EffectState.Active_Regular;
            IncreaseAttack();
        }
        if (_currentStacks == _extremeStackReq)
        {
            _currentEffectState = EffectState.Active_Extreme;
            Main.ScreamingSinners.Log($"every attack executes enemy!");
        }    
    }

    private void IncreaseStack_ParryGuardSlide(GuardSlide guardSlide)
    {
        if (_currentEffectState == EffectState.Active_Extreme) { return; }

        _currentStacks += 1;
        Main.ScreamingSinners.Log($"RB602's current stack: {_currentStacks}");

        if (_currentStacks == _regularStackReq)
        {
            _currentEffectState = EffectState.Active_Regular;
            IncreaseAttack();
        }
        if (_currentStacks == _extremeStackReq)
        {
            _currentEffectState = EffectState.Active_Extreme;
            Main.ScreamingSinners.Log($"every attack executes enemy!");
        }
    }

    private void ResetStack(ref Hit hit)
    {
        //if (hit.DamageAmount <= Mathf.Epsilon) { return; }

        _currentStacks = 0;
        _currentEffectState = EffectState.Inactive;
        ResetAttack();
        Main.ScreamingSinners.Log($"RB602's attack buff disappears!");
    }

    private void IncreaseAttack()
    {
        Core.Logic.Penitent.Stats.Strength.AddRawBonus(_attackBuff);
        Main.ScreamingSinners.Log($"RB602 increases attack!");
    }

    private void ResetAttack()
    {
        Core.Logic.Penitent.Stats.Strength.RemoveRawBonus(_attackBuff);
    }

    /// <summary>
    /// Enemies hit by Mea Culpa attacks enter executable state.
    /// </summary>
    private void AttackExecute(ref Hit hit)
    {
        if (_currentEffectState != EffectState.Active_Extreme) { return; }

        if (hit.DamageElement == DamageArea.DamageElement.Normal)
        {
            hit.DamageType = DamageArea.DamageType.OptionalStunt;
            Main.ScreamingSinners.Log($"Mea Culpa hit with DamageType {hit.DamageType}!");
        }
    }

    protected override void ApplyEffect()
    {
        Main.ScreamingSinners.EventHandler.OnParryRiposte += IncreaseStack_Parry;
        Main.ScreamingSinners.EventHandler.OnParryGuardSlide += IncreaseStack_ParryGuardSlide;
        Main.ScreamingSinners.EventHandler.OnPlayerDamaged += ResetStack;
        Main.ScreamingSinners.EventHandler.OnEnemyDamaged += AttackExecute;
    }

    protected override void RemoveEffect()
    {
        Main.ScreamingSinners.EventHandler.OnParryRiposte -= IncreaseStack_Parry;
        Main.ScreamingSinners.EventHandler.OnParryGuardSlide -= IncreaseStack_ParryGuardSlide;
        Main.ScreamingSinners.EventHandler.OnPlayerDamaged -= ResetStack;
        Main.ScreamingSinners.EventHandler.OnEnemyDamaged -= AttackExecute;
    }
}

public class RB602_Config
{
    public int regularStackReq = 6;
    public int extremeStackReq = 12;
    public float attackBuff = 8f;
}