using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderState { Closed, Open, Colliding }
public class HurtBoxManager : MonoBehaviour
{
    public float radius;

    private ColliderState _state;

    public bool viewHurtBoxes;

    public GameObject[] locator;
    public GameObject hurtbox;
    private PlayerControls playerControls;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerControls = GetComponentInParent<PlayerControls>();
    }
    private void Start()
    {
        for(int i = 0; i < locator.Length; i++)
        {
            Locator locatorScript = locator[i].GetComponent<Locator>();
            float tempLocatorRadius = locatorScript.radius;
            radius = tempLocatorRadius * 5;

            GameObject tempHurtBox = Instantiate(hurtbox, locator[i].transform.position, Quaternion.identity, locator[i].transform);

            HurtBox tempHurtBoxScript = tempHurtBox.GetComponent<HurtBox>();
            tempHurtBoxScript.location = locatorScript.location;
            tempHurtBoxScript.SetLayers(playerControls.playersLayer, playerControls.opponentLayer);
            tempHurtBoxScript.SetRadius(radius);
            tempHurtBox.transform.localScale = Vector3.one * (radius);
            if(viewHurtBoxes == true)
            {
                tempHurtBox.GetComponent<MeshRenderer>().enabled = true;
            }
            else if (viewHurtBoxes == false)
            {
                tempHurtBox.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
