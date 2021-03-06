﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourCheck : MonoBehaviour
{
    public ArmourCondition LegArmourCondition;
    public ArmourCondition ChestArmourCondition;
    public ArmourCondition HeadArmourCondiditon;
    public enum ArmourPlacement { Head, Chest, Legs}
    public ArmourPlacement armourPlacement;
    public enum ArmourCondition { none, armour};

    public float knockBackResistance, armourWeight, armourReduceSpeed, reduceJumpForce;
    float chestKnockBackResistance, legsKnockBackResistance;
    float chestWeight, legsWeight;
    float chestArmourReduceSpeed, legsArmourReduceSpeed;
    float chestReduceJump, legsReduceJump;

    [SerializeField] private ArmourStats armourStats;

    public GameObject[] ChestArmourMesh;
    public GameObject[] LegArmourMesh;
    private void Start()
    {
        SetAllArmourOn();
    }
    private void Update()
    {
        ArmourStatsCheck();
        ChangeArmourInputs();
    }
    public bool HasArmour()
    {
        if (ChestArmourCondition == ArmourCondition.armour || LegArmourCondition == ArmourCondition.armour)
        {
            return true;
        }
        else if (ChestArmourCondition == ArmourCondition.none && LegArmourCondition == ArmourCondition.none)
        {
            return false;
        }
        return false;
    }
    void ArmourStatsCheck()
    {
        switch (ChestArmourCondition)
        {
            case ArmourCondition.armour:
                chestWeight = armourStats.weightChest;
                chestArmourReduceSpeed = armourStats.reduceSpeedChest;
                chestReduceJump = armourStats.reduceJumpChest;
                chestKnockBackResistance = armourStats.knockBackResistanceChest;
                break;

            case ArmourCondition.none:
                chestWeight = 0;
                chestArmourReduceSpeed = 0;
                chestReduceJump = 0;
                chestKnockBackResistance = 0;
                break;
        }
        switch (LegArmourCondition)
        {
            case ArmourCondition.armour:
                legsWeight = armourStats.weightLegs;
                legsArmourReduceSpeed = armourStats.reduceSpeedLegs;
                legsReduceJump = armourStats.reduceJumpLegs;
                legsKnockBackResistance = armourStats.knockBackRsistanceLegs;
                break;
            case ArmourCondition.none:
                legsWeight = 0;
                legsArmourReduceSpeed = 0;
                legsReduceJump = 0;
                legsKnockBackResistance = 0;
                break;      
        }

        knockBackResistance = chestKnockBackResistance + legsKnockBackResistance;
        reduceJumpForce = chestReduceJump + legsReduceJump;
        armourReduceSpeed = chestArmourReduceSpeed + legsArmourReduceSpeed;
        armourWeight = chestWeight + legsWeight;
    }

    public void SetAllArmourOff()
    {
        SetArmourOff(ArmourPlacement.Legs);
        SetArmourOff(ArmourPlacement.Chest);
    }
    public void SetAllArmourOn()
    {
        SetArmourOn(ArmourPlacement.Legs, ArmourCondition.armour);
        SetArmourOn(ArmourPlacement.Chest, ArmourCondition.armour);
    }
    public void SetArmourOn(ArmourPlacement placement, ArmourCondition type)
    {
        switch (placement)
        {
            case ArmourPlacement.Chest:
                for (int i = 0; i < ChestArmourMesh.Length; i++)
                {
                    ChestArmourMesh[i].SetActive(true);
                    ChestArmourCondition = type;
                }
                break;
            case ArmourPlacement.Legs:
                for (int i = 0; i < LegArmourMesh.Length; i++)
                {
                    LegArmourMesh[i].SetActive(true);
                    LegArmourCondition = type;
                }
                break;
        }
    }

    public void DestroyArmour(ArmourCheck.ArmourPlacement placement, Player defendingPlayer, AttackType attackType)
    {
        if (placement == ArmourCheck.ArmourPlacement.Head)
        {
            if (HeadArmourCondiditon == ArmourCheck.ArmourCondition.none)
            {
                defendingPlayer.PlayArmourHitSound(false, attackType);
                return;
            }
            defendingPlayer.PlayArmourHitSound(true, attackType);
        }

        else if (placement == ArmourCheck.ArmourPlacement.Chest)
        {
            if (ChestArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                defendingPlayer.PlayArmourHitSound(false, attackType);
                return;
            }

            RemoveChestArmour();
            defendingPlayer.PlayArmourHitSound(true, attackType);
        }

        else if (placement == ArmourCheck.ArmourPlacement.Legs)
        {
            if (LegArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                defendingPlayer.PlayArmourHitSound(false, attackType);
                return;
            }

            RemoveLegArmour();
            defendingPlayer.PlayArmourHitSound(true, attackType);
        }
    }
    public void SetArmourOff(ArmourPlacement armourPlacement)
    {
        switch (armourPlacement)
        {
            case ArmourPlacement.Chest:
                for (int i = 0; i < ChestArmourMesh.Length; i++)
                {
                    ChestArmourMesh[i].SetActive(false);
                    ChestArmourCondition = ArmourCondition.none;
                }
                break;
            case ArmourPlacement.Legs:
                for (int i = 0; i < LegArmourMesh.Length; i++)
                {
                    LegArmourMesh[i].SetActive(false);
                    LegArmourCondition = ArmourCondition.none;
                }
                break;
            default:
                break;
        }
    }
    public ArmourCondition GetChestArmourCondiditon()
    {
        return ChestArmourCondition;
    }
    public ArmourCondition GetLegArmourCondition()
    {
        return LegArmourCondition;
    }

    public void RemoveLegArmour()
    {
        SetArmourOff(ArmourPlacement.Legs);
    }

    public void RemoveChestArmour()
    {
        SetArmourOff(ArmourPlacement.Chest);
    }

    void ChangeArmourInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAllArmourOff();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAllArmourOn();
        }
    }
}
