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

    [SerializeField] private ArmourStats stats;
    public GameObject[] ChestArmourMesh;
    public GameObject[] LegArmourMesh;

    private void Awake()
    {
        stats = GetComponent<ArmourStats>();
    }
    private void Start()
    {
    }
    private void Update()
    {
        ArmourStatsCheck();
        ChangeArmourInputs();
    }
    public bool HasArmour()
    {
        if (ChestArmourType == Armour.armour || LegArmourType == Armour.armour)
        {
            return true;
        }
        else if (ChestArmourType == Armour.none && LegArmourType == Armour.none)
        {
            return false;
        }
        return false;
    }
    void ArmourStatsCheck()
    {
        switch (ChestArmourType)
        {
            case Armour.armour:
                chestWeight = stats.weightChest;
                chestArmourReduceSpeed = stats.reduceSpeedChest;
                chestReduceJump = stats.reduceJumpChest;
                chestKnockBackResistance = stats.knockBackResistanceChest;
                break;

            case Armour.none:
                chestWeight = 0;
                chestArmourReduceSpeed = 0;
                chestReduceJump = 0;
                chestKnockBackResistance = 0;
                break;
        }
        switch (LegArmourType)
        {
            case Armour.armour:
                legsWeight = stats.weightLegs;
                legsArmourReduceSpeed = stats.reduceSpeedLegs;
                legsReduceJump = stats.reduceJumpLegs;
                legsKnockBackResistance = stats.knockBackRsistanceLegs;
                break;
            case Armour.none:
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
    public void SetAllArmourOn()
    {
        SetArmourOn(ArmourType.Legs, Armour.armour);
        SetArmourOn(ArmourType.Chest, Armour.armour);
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
        SetArmourOff(ArmourType.Legs);
    }

    public void RemoveChestArmour()
    {
        SetArmourOff(ArmourType.Chest);
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
