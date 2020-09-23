﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourCheck : MonoBehaviour
{
    public Armour LegArmourType;
    public Armour ChestArmourType;
    public enum ArmourType { Chest, Legs}
    public ArmourType ArmourPlacement;
    public enum Armour { none, light, heavy };

    public bool hasArmour;

    public float knockBackResistance, armourWeight, armourReduceSpeed, reduceJumpForce;
    private float chestKnockBackResistance, legsKnockBackResistance;
    private float chestWeight, legsWeight;
    private float chestArmourReduceSpeed, legsArmourReduceSpeed;
    private float chestReduceJump, legsReduceJump;

    public GameObject[] ChestArmourMesh;
    public GameObject[] LegArmourMesh;

    private void Update()
    {
        ArmourStatsCheck();
        ChangeArmourInputs();
    }
    void ArmourStatsCheck()
    {
        switch (ChestArmourType)
        {
            case Armour.heavy:
                LegArmourType = Armour.heavy;
                chestWeight = 8;
                chestArmourReduceSpeed = 3;
                chestReduceJump = 2;
                chestKnockBackResistance = 6;
                hasArmour = true;
                break;

            case Armour.light:
                LegArmourType = Armour.light;
                chestWeight = 5;
                chestArmourReduceSpeed = 1;
                chestReduceJump = 1;
                chestKnockBackResistance = 3;
                hasArmour = true;
                break;

            case Armour.none:
                LegArmourType = Armour.none;
                chestWeight = 0;
                chestArmourReduceSpeed = 0;
                chestReduceJump = 0;
                chestKnockBackResistance = 0;
                hasArmour = false;
                break;

            default:
                LegArmourType = Armour.none;
                armourWeight = 0;
                chestArmourReduceSpeed = 0;
                chestReduceJump = 0;
                chestKnockBackResistance = 0;
                hasArmour = false;
                break;
        }
        switch (LegArmourType)
        {
            case Armour.heavy:
                LegArmourType = Armour.heavy;
                legsWeight = 8;
                legsArmourReduceSpeed = 3;
                legsReduceJump = 2;
                legsKnockBackResistance = 6;
                hasArmour = true;
                break;

            case Armour.light:
                LegArmourType = Armour.light;
                legsWeight = 5;
                legsArmourReduceSpeed = 1;
                legsReduceJump = 1;
                legsKnockBackResistance = 3;
                hasArmour = true;
                break;

            case Armour.none:
                LegArmourType = Armour.none;
                legsWeight = 0;
                legsArmourReduceSpeed = 0;
                legsReduceJump = 0;
                legsKnockBackResistance = 0;
                hasArmour = false;
                break;
                    
            default:
                LegArmourType = Armour.none;
                legsWeight = 0;
                legsArmourReduceSpeed = 0;
                legsReduceJump = 0;
                legsKnockBackResistance = 0;
                hasArmour = false;
                break;
        }

        knockBackResistance = chestKnockBackResistance + legsKnockBackResistance;
        reduceJumpForce = chestReduceJump + legsReduceJump;
        armourReduceSpeed = chestArmourReduceSpeed + legsArmourReduceSpeed;
        armourWeight = chestWeight + legsWeight;

    }
    public void SetAllArmourOff()
    {
        hasArmour = false;
        SetArmourOff(ArmourType.Legs);
        SetArmourOff(ArmourType.Chest);
    }
    public void SetArmourOn(ArmourType placement, Armour type)
    {
        switch (placement)
        {
            case ArmourType.Chest:
                for (int i = 0; i < ChestArmourMesh.Length; i++)
                {
                    ChestArmourMesh[i].SetActive(true);
                    ChestArmourType = type;
                }
                break;
            case ArmourType.Legs:
                for (int i = 0; i < LegArmourMesh.Length; i++)
                {
                    LegArmourMesh[i].SetActive(true);
                    LegArmourType = type;
                }
                break;
            default:
                break;
        }
    }

    public void SetArmourOff(ArmourType armourPlacement)
    {
        switch (armourPlacement)
        {
            case ArmourType.Chest:
                for (int i = 0; i < ChestArmourMesh.Length; i++)
                {
                    ChestArmourMesh[i].SetActive(false);
                    ChestArmourType = Armour.none;
                }
                break;
            case ArmourType.Legs:
                for (int i = 0; i < LegArmourMesh.Length; i++)
                {
                    LegArmourMesh[i].SetActive(false);
                    LegArmourType = Armour.none;
                }
                break;
            default:
                break;
        }
    }

    void ChangeArmourInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hasArmour = false;
            LegArmourType = Armour.none;
            SetAllArmourOff();

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            hasArmour = true;
            LegArmourType = Armour.light;
            SetArmourOn(ArmourType.Legs,Armour.light);
            SetArmourOn(ArmourType.Chest, Armour.light);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hasArmour = true;
            LegArmourType = Armour.heavy;
            SetArmourOn(ArmourType.Legs, Armour.heavy);
            SetArmourOn(ArmourType.Chest, Armour.heavy);
        }
    }
}
