using Blasphemous.Framework.Items;
using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using Gameplay.GameControllers.AnimationBehaviours.Player.Attack;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using Gameplay.GameControllers.Penitent.Animator;
using Gameplay.GameControllers.Penitent.Attack;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Blasphemous.ScreamingSinners.Beads;

class RB601_Effect : ModItemEffectOnEquip
{
    private int _currentStacks = 0;
    private int _regularStackReq;
    private int _extremeStackReq;

    private RawBonus _attackSpeedBonus;
    private RawBonus _fervourStrengthReduction = null;
    private float _healAmount;

    private float _currentTimer = 0f;
    private float _updateInterval = 0.5f;

    private enum EffectState
    {
        Inactive,
        Active_Basic,
        Active_Medium,
        Active_Extreme
    }
    private EffectState _currentEffectState = EffectState.Inactive;

    public RB601_Effect(RB601_Config config)
    {
        _regularStackReq = config.regularStackReq;
        _extremeStackReq = config.extremeStackReq;
        _attackSpeedBonus = new(config.attackSpeedBuff);
    }

    private void IncreaseStack(ref Hit hit)
    {
        if (_currentEffectState == EffectState.Active_Extreme) 
        {
            Core.Logic.Penitent.Stats.Life.Current += _healAmount;
            return;
        }
        if (hit.DamageElement != DamageArea.DamageElement.Normal) { return; }
        _currentStacks++;
        if (_currentStacks == _regularStackReq) 
        {
            _currentEffectState = EffectState.Active_Medium;
            // gain unstoppable stance, code pending
        }
        if (_currentStacks == _extremeStackReq)
        {
            _currentEffectState = EffectState.Active_Extreme;
        }
    }

    // reset condition still pending, code WIP
    private void ResetStack(PenitentAttackAnimations attack)
    {
        _currentStacks = 0;
        ResetFervourRegen();
        _currentEffectState = EffectState.Inactive;
    }

    private void IncreaseAttackSpeed(PenitentAttackAnimations attack)
    {
        if (_currentEffectState == EffectState.Inactive)
        {
            Core.Logic.Penitent.Stats.AttackSpeed.AddRawBonus(_attackSpeedBonus);
            _currentEffectState = EffectState.Active_Basic;
        }
    }

    private void ReduceFervourRegen()
    {
        if (_currentEffectState != EffectState.Active_Extreme) { return; }

        if (_fervourStrengthReduction != null)
        {
            Core.Logic.Penitent.Stats.FervourStrength.RemoveRawBonus(_fervourStrengthReduction);
        }
        _healAmount = Core.Logic.Penitent.Stats.FervourStrength.Final;
        _fervourStrengthReduction = new(Core.Logic.Penitent.Stats.FervourStrength.Final);
        Core.Logic.Penitent.Stats.FervourStrength.AddRawBonus(_fervourStrengthReduction);
    }

    private void ResetFervourRegen()
    {
        if (_fervourStrengthReduction != null)
        {
            Core.Logic.Penitent.Stats.FervourStrength.RemoveRawBonus(_fervourStrengthReduction);
        }
        _fervourStrengthReduction = null;
    }

    protected override void Update()
    {
        _currentTimer += Time.deltaTime;
        if (_currentTimer >= _updateInterval)
        {
            _currentTimer = 0;
            ResetFervourRegen();
            ReduceFervourRegen();
        }
    }

    protected override void ApplyEffect()
    {
        Main.ScreamingSinners.EventHandler.OnPlayerUpwardAttack += IncreaseAttackSpeed;
        Main.ScreamingSinners.EventHandler.OnEnemyDamaged += IncreaseStack;
    }

    protected override void RemoveEffect()
    {
        Main.ScreamingSinners.EventHandler.OnPlayerUpwardAttack -= IncreaseAttackSpeed;
        Main.ScreamingSinners.EventHandler.OnEnemyDamaged -= IncreaseStack;
    }
}

public class RB601_Config
{
    public int regularStackReq = 30;
    public int extremeStackReq = 60;
    public float attackSpeedBuff = 0.5f;
}

