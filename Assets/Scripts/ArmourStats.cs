using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourStats
{
    public Armour armourType;
    public enum Armour { none, light, heavy };
    public float knockBackResistance;

    public int armourWeight, armourReduceSpeed, reduceJumpForce;

}
