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
            Vector3 Hit = other.transform.forward;
            punchingBag.knockbackResistance = knockBackResistance();
        }
    }

    private float knockBackResistance()
    {
        if (punchingBag.armour == true)
        {
            this.gameObject.SetActive(true);
            return 200;
        }
        else
        {
            this.gameObject.SetActive(false);
            return 0;
        }

    }
}
