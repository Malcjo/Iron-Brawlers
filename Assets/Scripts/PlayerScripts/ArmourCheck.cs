using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourCheck : MonoBehaviour
{
    public Armour LegArmourType;
    public Armour ChestArmourType;
    public enum ArmourType { Chest, Legs}
    public ArmourType armourType;
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
        SetArmourMeshOff(ArmourType.Legs);
        SetArmourMeshOff(ArmourType.Chest);
    }
    public void SetAllArmourOn()
    {
        hasArmour = false;
        SetArmourMeshOn(ArmourType.Legs);
        SetArmourMeshOn(ArmourType.Chest);
    }
    public void SetArmourMeshOn(ArmourType type)
    {
        switch (type)
        {
            case ArmourType.Chest:
                for (int i = 0; i < ChestArmourMesh.Length; i++)
                {
                    ChestArmourMesh[i].SetActive(true);
                }
                break;
            case ArmourType.Legs:
                for (int i = 0; i < LegArmourMesh.Length; i++)
                {
                    LegArmourMesh[i].SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    public void SetArmourMeshOff(ArmourType type)
    {
        switch (type)
        {
            case ArmourType.Chest:
                for (int i = 0; i < ChestArmourMesh.Length; i++)
                {
                    ChestArmourMesh[i].SetActive(false);
                }
                break;
            case ArmourType.Legs:
                for (int i = 0; i < LegArmourMesh.Length; i++)
                {
                    LegArmourMesh[i].SetActive(false);
                }
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        ArmourStatsCheck();
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
