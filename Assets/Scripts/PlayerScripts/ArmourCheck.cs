using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourCheck : MonoBehaviour
{
    public Armour LegArmourType;
    public Armour ChestArmourType;
    public enum ArmourType { Chest, Legs}
    public ArmourType ArmourPlacement;
    public enum Armour { none, armour};

    public float knockBackResistance, armourWeight, armourReduceSpeed, reduceJumpForce;
    float chestKnockBackResistance, legsKnockBackResistance;
    float chestWeight, legsWeight;
    float chestArmourReduceSpeed, legsArmourReduceSpeed;
    float chestReduceJump, legsReduceJump;

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
            case Armour.armour:
                LegArmourType = Armour.armour;
                chestWeight = 3;
                chestArmourReduceSpeed = 1;
                chestReduceJump = 0.5f;
                chestKnockBackResistance = 3;
                break;

            case Armour.none:
                LegArmourType = Armour.none;
                chestWeight = 0;
                chestArmourReduceSpeed = 0;
                chestReduceJump = 0;
                chestKnockBackResistance = 0;
                break;
            default:
                LegArmourType = Armour.none;
                armourWeight = 0;
                chestArmourReduceSpeed = 0;
                chestReduceJump = 0;
                chestKnockBackResistance = 0;
                break;
        }
        switch (LegArmourType)
        {
            case Armour.armour:
                LegArmourType = Armour.armour;
                legsWeight = 4;
                legsArmourReduceSpeed = 1f;
                legsReduceJump = 0.5f;
                legsKnockBackResistance = 3;
                break;
            case Armour.none:
                LegArmourType = Armour.none;
                legsWeight = 0;
                legsArmourReduceSpeed = 0;
                legsReduceJump = 0;
                legsKnockBackResistance = 0;
                break;      
            default:
                LegArmourType = Armour.none;
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

    public void RemoveLegArmour()
    {
        LegArmourType = ArmourCheck.Armour.none;
        SetArmourOff(ArmourCheck.ArmourType.Legs);
    }
    public void RemoveChestArmour()
    {
        ChestArmourType = ArmourCheck.Armour.none;
        SetArmourOff(ArmourCheck.ArmourType.Chest);
    }

    void ChangeArmourInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LegArmourType = Armour.none;
            SetAllArmourOff();

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LegArmourType = Armour.armour;
            SetArmourOn(ArmourType.Legs,Armour.armour);
            SetArmourOn(ArmourType.Chest, Armour.armour);
        }
    }
}
