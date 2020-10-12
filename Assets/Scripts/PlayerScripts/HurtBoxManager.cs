using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxManager : MonoBehaviour
{
    public float radius;
    public Transform[] HurtBoxLocation;
    public GameObject hurtbox;
    private void Start()
    {
        for(int i = 0; i < HurtBoxLocation.Length; i++)
        {
            Instantiate(hurtbox, HurtBoxLocation[i].transform.position, Quaternion.identity, HurtBoxLocation[i]);
        }
    }
}
