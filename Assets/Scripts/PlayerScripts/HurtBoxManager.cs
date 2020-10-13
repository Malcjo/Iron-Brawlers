using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxManager : MonoBehaviour
{
    public float radius;
    public Transform[] locator;
    public GameObject hurtbox;
    private void Start()
    {
        for(int i = 0; i < locator.Length; i++)
        {
            float tempLocatorRadius = locator[i].GetComponent<Locator>().radius;
            radius = tempLocatorRadius * 5;
            GameObject tempHurtBox = Instantiate(hurtbox, locator[i].transform.position, Quaternion.identity, locator[i]);
            tempHurtBox.transform.localScale = Vector3.one * (radius);
        }
    }
}
