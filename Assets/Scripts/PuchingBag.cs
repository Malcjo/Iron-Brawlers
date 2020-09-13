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
    // Start is called before the first frame update
    void Start()
    {
        armour = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckArmour();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Jab")
        {
            Vector3 Hit = other.GetComponent<TempHitBox>().HitDirection();
            if(armour == false)
            {
                Knockback(Hit, knockbackResistance);
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
