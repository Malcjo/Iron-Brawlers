using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField] private float radius;
    public GameObject hitBoxMesh;
    HurtBoxManager manager;
    private void Awake()
    {

    }
    private void Start()
    {
        manager = GetComponentInParent<HurtBoxManager>();
        radius = manager.radius;
        transform.localScale = Vector3.one * radius;
    }
    private void FixedUpdate()
    {
        
    }

}
