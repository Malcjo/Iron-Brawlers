using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxLocation : MonoBehaviour
{
    public Transform Location;
    public Vector3 PositionOffset;
    public Vector3 RotationOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Location.transform.position + PositionOffset;
        transform.rotation = new Quaternion (Location.rotation.x, Location.rotation.y,Location.rotation.z,0);
    }
}
