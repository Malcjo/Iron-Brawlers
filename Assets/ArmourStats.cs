using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourStats : MonoBehaviour
{
    public Armour armourType;
    [Range(1, 3)]
    public int armourIndex;
    [SerializeField] private Player player;
    public enum Armour {none, light, heavy };

    private void Start()
    {
        player = GetComponent<Player>();
        armourIndex = 1;
    }


    public int armourWeightStat()
    {
        switch (armourType)
        {
            case Armour.heavy:
                armourType= Armour.heavy;
                return 20;
            case Armour.light:
                armourType = Armour.light;
                return 10;
            case Armour.none:
                armourType = Armour.none;
                return 0;
            default:
                armourType = Armour.none;
                return 0;
        }
    }
    public int armourSpeedReduceStat()
    {
        switch (armourType)
        {
            case Armour.heavy:
                armourType = Armour.heavy;
                return 6;
            case Armour.light:
                armourType = Armour.light;
                return 3;
            case Armour.none:
                armourType = Armour.none;
                return 0;
            default:
                armourType = Armour.none;
                return 0;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.hasArmour = false;
            armourType = Armour.none;
            armourIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.hasArmour = true;
            armourType = Armour.light;
            armourIndex = 2;

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.hasArmour = true;
            armourType = Armour.heavy;
            armourIndex = 3;
        }
    }
}
