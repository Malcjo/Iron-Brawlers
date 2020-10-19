using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderState { Closed, Open, Colliding }
public class HurtBoxManager : MonoBehaviour
{
    public LayerMask mask;
    public float radius;
    public Color inactiveColour;
    public Color collisionOpenColour;
    public Color collidingColour;

    private ColliderState _state;

    public bool viewHurtBoxes;

    public Transform[] locator;
    public GameObject hurtbox;
    private PlayerControls playerControls;

    private void CheckGizmoColour()
    {
        switch (_state)
        {
            case ColliderState.Closed:
                Gizmos.color = inactiveColour;
                break;
            case ColliderState.Open:
                Gizmos.color = collisionOpenColour;
                break;
            case ColliderState.Colliding:
                Gizmos.color = collidingColour;
                break;
        }
    }
    private void Awake()
    {
        playerControls = GetComponentInParent<PlayerControls>();
    }
    private void Start()
    {
        for(int i = 0; i < locator.Length; i++)
        {
            float tempLocatorRadius = locator[i].GetComponent<Locator>().radius;
            radius = tempLocatorRadius * 5;
            GameObject tempHurtBox = Instantiate(hurtbox, locator[i].transform.position, Quaternion.identity, locator[i]);
            HurtBox tempHurtBoxScript = tempHurtBox.GetComponent<HurtBox>();
            tempHurtBoxScript.SetLayers(playerControls.playersLayer, playerControls.opponentLayer);
            tempHurtBoxScript.SetRadius(radius);
            tempHurtBox.transform.localScale = Vector3.one * (radius);
        }
    }
}
