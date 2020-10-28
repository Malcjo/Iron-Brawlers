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
        //transform.rotation = Quaternion.Euler(location.rotation.x, location.rotation.y, location.rotation.z);
        transform.rotation = location.transform.rotation;
        transform.position = location.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        armourTransform = transform.position;
        armourRotation = transform.rotation;
        locationTransform = location.transform.position;
        locationRotation = location.transform.rotation;

        transform.position = location.transform.position;
    }
}
