using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourStats : MonoBehaviour
{
    public Armour armourType;
    public enum Armour { none, light, heavy };
    [Range(1, 3)]
    public int armourIndex;
    [SerializeField] private Player player;

    public int armourWeight, armourReduceSpeed, reduceJumpForce;


    private void Start()
    {
        player = GetComponent<Player>();
        armourIndex = 1;
    }


    void ArmourStatsCheck()
    {
        switch (armourType)
        {
            case Armour.heavy:
                armourType = Armour.heavy;
                armourWeight = 20;
                armourReduceSpeed = 6;
                reduceJumpForce = 2;
                break;

            case Armour.light:
                armourType = Armour.light;
                armourWeight = 10;
                armourReduceSpeed = 3;
                reduceJumpForce = 1;
                break;

            case Armour.none:
                armourType = Armour.none;
                armourWeight = 0;
                armourReduceSpeed = 0;
                reduceJumpForce = 0;
                break;
                    
            default:
                armourType = Armour.none;
                armourWeight = 0;
                armourReduceSpeed = 0;
                reduceJumpForce = 0;
                break;
        }
    }

    private void Update()
    {
        ArmourStatsCheck();
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
