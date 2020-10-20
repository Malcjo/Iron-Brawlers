using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuchingBag : MonoBehaviour
{
    private Rigidbody rb;
    public float knockbackResistance;
    public bool armour;
    public GameObject armourMesh;
    public bool hitArmour;
    private PunchingBagArmour ArmourStats;

    private void Awake()
    {
        ArmourStats = GetComponentInChildren<PunchingBagArmour>();
    }
    void Start()
    {
        armour = true;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckArmour();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Jab")
        {
            Vector3 Hit = other.GetComponent<Hitbox>().HitDirection();

            if (armour == true)
            {
                Debug.Log("Hit Armour!");
                return;
            }
            else if (armour == false)
            {
                Debug.Log("Hit Body!");
                knockbackResistance = 0;
                Knockback(Hit, ArmourStats.knockBackResistance());
            }
            knockbackResistance = 0;
            
            
        }
    }
    void CheckArmour()
    {
        if (armour == true)
        {
            knockbackResistance = 200;
            armourMesh.SetActive(true);
        }
        else
        {
            knockbackResistance = 0;
            armourMesh.SetActive(false);
        }
    }
    public void Knockback(Vector3 direction, float knockbackResistance)
    {
        rb.AddForce(direction * (250 - knockbackResistance));
    }
}
