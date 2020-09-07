using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuchingBag : MonoBehaviour
{
    private Rigidbody rb;
    public float knockback;
    public bool armour;
    public GameObject armourMesh;
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
            Vector3 Hit = other.transform.forward;
            rb.AddForce(Hit * (250 - knockback));
            armour = false;
        }
    }
    void CheckArmour()
    {
        if (armour == true)
        {
            knockback = 150;
            armourMesh.SetActive(true);
        }
        else
        {
            knockback = 0;
            armourMesh.SetActive(false);
        }
    }
}
