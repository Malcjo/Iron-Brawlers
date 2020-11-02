using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : MonoBehaviour
{
    public Transform location;
    // Start is called before the first frame update
    public Vector3 armourTransform, locationTransform;
    public Quaternion armourRotation, locationRotation;
    void Start()
    {
        transform.SetParent(location);
    }
}
