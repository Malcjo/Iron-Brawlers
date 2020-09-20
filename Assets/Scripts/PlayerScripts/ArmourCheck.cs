using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourCheck : MonoBehaviour
{
    public Armour armourType;
    public enum Armour { none, light, heavy };
    [Range(1, 3)]
    public int armourIndex;
    [SerializeField] private Player player;
    public float knockBackResistance;

    public int armourWeight, armourReduceSpeed, reduceJumpForce;

    public GameObject[] ArmourMesh;
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
                armourWeight = 8;
                armourReduceSpeed = 3;
                reduceJumpForce = 2;
                knockBackResistance = 6;
                player.hasArmour = true;
                SetArmourMeshOff();
                break;

            case Armour.light:
                armourType = Armour.light;
                armourWeight = 5;
                armourReduceSpeed = 1;
                reduceJumpForce = 1;
                knockBackResistance = 3;
                player.hasArmour = true;
                SetArmourMeshOff();
                break;

            case Armour.none:
                armourType = Armour.none;
                armourWeight = 5;
                armourReduceSpeed = 0;
                reduceJumpForce = 0;
                knockBackResistance = 0;
                player.hasArmour = false;
                SetArmourMeshOn();
                break;
                    
            default:
                armourType = Armour.none;
                armourWeight = 0;
                armourReduceSpeed = 0;
                reduceJumpForce = 0;
                knockBackResistance = 0;
                player.hasArmour = false;
                break;
        }
    }
    void SetArmourMeshOn()
    {
        for (int i = 0; i < ArmourMesh.Length; i++)
        {
            ArmourMesh[i].SetActive(false);
        }
    }
    void SetArmourMeshOff()
    {
        for (int i = 0; i < ArmourMesh.Length; i++)
        {
            ArmourMesh[i].SetActive(true);
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
