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

class RB603_Effect : ModItemEffectOnEquip
{
    private int _currentStacks = 0;
    private int _maxStacks;
    private RawBonus _attackBuff;
    private enum EffectState
    {
        Inactive,
        Active
    }
    private EffectState _currentEffectState = EffectState.Inactive;

    public RB603_Effect(RB603_Config config)
    {
        _maxStacks = config.maxStacks;
        _attackBuff = new(config.attackBuff);

    }

    private void IncreaseStack_AttackHit(ref Hit hit)
    {
        if (_currentEffectState == EffectState.Active 
            || hit.DamageElement != DamageArea.DamageElement.Normal) { return; }

        if (hit.DamageType == DamageArea.DamageType.Heavy) // sword arts 
        { _currentStacks += 3; }
        else { _currentStacks++; }

        if (_currentStacks >= _maxStacks) 
        {
            _currentStacks = _maxStacks;
            IncreaseAttack();
        }
        Main.ScreamingSinners.Log($"RB603's stack increases due to attack");
    }

    private void IncreaseStack_Parry(Parry parry)
    {
        if (!parry.SuccessParry || (_currentEffectState == EffectState.Active)) { return; }

        Main.ScreamingSinners.Log($"RB603's stack increases due to successful parry");
        _currentStacks += 2;

        if (_currentStacks >= _maxStacks)
        {
            _currentStacks = _maxStacks;
            IncreaseAttack();
        }
    }

    private void ResetStack(ref Hit hit)
    {
        //if (hit.DamageAmount <= Mathf.Epsilon) { return; }

        _currentStacks = 0;
        ResetAttack();
    }

    private void IncreaseAttack()
    {
        if (_currentEffectState == EffectState.Inactive)
        {
            Core.Logic.Penitent.Stats.Strength.AddRawBonus(_attackBuff);
            _currentEffectState = EffectState.Active;
        }
        Main.ScreamingSinners.Log($"RB603 reaches max stack, activating effect!");
    }

    private void ResetAttack()
    {
        if (_currentEffectState == EffectState.Active)
        {
            Core.Logic.Penitent.Stats.Strength.RemoveRawBonus(_attackBuff);
            _currentEffectState = EffectState.Inactive;
        }
        Main.ScreamingSinners.Log($"RB603's buff disappears!");
    }

    protected override void ApplyEffect()
    {
        Main.ScreamingSinners.EventHandler.OnEnemyDamaged += IncreaseStack_AttackHit;
        Main.ScreamingSinners.EventHandler.OnParryRiposte += IncreaseStack_Parry;
        Main.ScreamingSinners.EventHandler.OnPlayerDamaged += ResetStack;
    }

    protected override void RemoveEffect()
    {
        Main.ScreamingSinners.EventHandler.OnEnemyDamaged -= IncreaseStack_AttackHit;
        Main.ScreamingSinners.EventHandler.OnParryRiposte -= IncreaseStack_Parry;
        Main.ScreamingSinners.EventHandler.OnPlayerDamaged -= ResetStack;
    }
}

public class RB603_Config
{
    public int maxStacks = 12;
    public float attackBuff = 8f;
}