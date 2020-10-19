using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : MonoBehaviour
{
    public Transform location;
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(location);
        transform.rotation = Quaternion.Euler(location.rotation.x, location.rotation.y, location.rotation.z);
        transform.position = location.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = location.transform.position;
    }
}
