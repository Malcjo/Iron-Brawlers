using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField] private float radius;
    public GameObject hitBoxMesh;
    HurtBoxManager manager;
    [SerializeField] private int playerLayer;
    [SerializeField] private int opponentLayer;
    [SerializeField] private int activeLayer;

    private void Awake()
    {

    }
    private void Start()
    {
        manager = GetComponentInParent<HurtBoxManager>();
        radius = (manager.radius);
        transform.localScale = Vector3.one * radius;
        this.gameObject.layer = playerLayer;
    }
    private void FixedUpdate()
    {

    }
    
    public void SetRadius(float externalRadius)
    {
        radius = externalRadius;
    }
    public void SetLayers(int player, int opponent)
    {
        playerLayer = player;
        opponentLayer = opponent;
        activeLayer = 1 << opponent;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius * 0.1f);
    }
}
