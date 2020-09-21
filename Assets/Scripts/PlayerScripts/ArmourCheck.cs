using System.Collections;
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

    public float knockBackResistance;
    private float legsKnockBackResistance, chestKnockBackResistance;
    public float armourWeight, armourReduceSpeed, reduceJumpForce;
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
                armourWeight = 8;
                armourReduceSpeed = 3;
                reduceJumpForce = 2;
                knockBackResistance = 6;
                hasArmour = true;
                break;

            case Armour.light:
                LegArmourType = Armour.light;
                armourWeight = 5;
                armourReduceSpeed = 1;
                reduceJumpForce = 1;
                knockBackResistance = 3;
                hasArmour = true;
                break;

            case Armour.none:
                LegArmourType = Armour.none;
                armourWeight = 0;
                armourReduceSpeed = 0;
                reduceJumpForce = 0;
                knockBackResistance = 0;
                hasArmour = false;
                break;

            default:
                LegArmourType = Armour.none;
                armourWeight = 0;
                armourReduceSpeed = 0;
                reduceJumpForce = 0;
                knockBackResistance = 0;
                hasArmour = false;
                break;
        }
        switch (LegArmourType)
        {
            case Armour.heavy:
                LegArmourType = Armour.heavy;
                armourWeight = 8;
                armourReduceSpeed = 3;
                reduceJumpForce = 2;
                knockBackResistance = 6;
                hasArmour = true;
                break;

            case Armour.light:
                LegArmourType = Armour.light;
                armourWeight = 5;
                armourReduceSpeed = 1;
                reduceJumpForce = 1;
                knockBackResistance = 3;
                hasArmour = true;
                break;

            case Armour.none:
                LegArmourType = Armour.none;
                armourWeight = 0;
                armourReduceSpeed = 0;
                reduceJumpForce = 0;
                knockBackResistance = 0;
                hasArmour = false;
                break;
                    
            default:
                LegArmourType = Armour.none;
                armourWeight = 0;
                armourReduceSpeed = 0;
                reduceJumpForce = 0;
                knockBackResistance = 0;
                hasArmour = false;
                break;
        }

        knockBackResistance = chestKnockBackResistance + legsKnockBackResistance;
        reduceJumpForce = chestReduceJump + legsReduceJump;
        armourReduceSpeed = chestArmourReduceSpeed + legsArmourReduceSpeed;

    }
    public void SetAllArmourOff()
    {
        hasArmour = false;
        SetArmourOff(ArmourType.Legs);
        SetArmourOff(ArmourType.Chest);
    }
    public void SetAllArmourOn()
    {
        hasArmour = false;
        SetArmourOn(ArmourType.Legs, Armour.light);
        SetArmourOn(ArmourType.Chest, Armour.light);
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
            SetAllArmourOn();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hasArmour = true;
            LegArmourType = Armour.heavy;
            SetAllArmourOn();
        }
    }
}
