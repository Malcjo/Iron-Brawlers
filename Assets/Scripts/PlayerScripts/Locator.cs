using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    public float radius;
    public Transform Location;
    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x;
        transform.SetParent(Location);
        transform.rotation = Quaternion.Euler(Location.rotation.x,Location.rotation.y,Location.rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetParent(Location);
    }
}
