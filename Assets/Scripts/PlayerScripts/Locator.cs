using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationTag { Legs, Chest, Head}
public class Locator : MonoBehaviour
{
    public LocationTag location;
    public float radius;
    public Transform Location;


    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x;
        transform.SetParent(Location);
        //transform.position = Location.transform.position;
        //transform.rotation = Location.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.SetParent(Location);
        
    }
}
