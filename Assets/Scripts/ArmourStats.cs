using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourStats : MonoBehaviour
{
    public string armourType;
    [Range(0,1)]
    public int armourIndex;
    [SerializeField] private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        armourIndex = 1;
    }


    public int armourWeightStat()
    {
        switch (armourIndex)
        {
            case 1:
                armourType = "Heavy";
                return 20;
            default:
                armourType = "Light";
                return 10;
        }
    }
    public int armourSpeedReduceStat()
    {
        switch (armourIndex)
        {
            case 1:
                armourType = "Heavy";
                return 6;
            default:
                armourType = "Light";
                return 3;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.hasArmour = !player.hasArmour;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            armourIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            armourIndex = 1;
        }
    }

}
