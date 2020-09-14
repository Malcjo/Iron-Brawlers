using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBagArmour : MonoBehaviour
{
    public PuchingBag punchingBag;

    private void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Jab")
        {
            Vector3 Hit = other.GetComponent<TempHitBox>().HitDirection();
            punchingBag.Knockback(Hit, knockBackResistance());
            punchingBag.armour = false;
        }
    }

    public float knockBackResistance()
    {
        if (punchingBag.armour == true)
        {
            return 200;
        }
        else
        {
            return 0;
        }

    }
}
