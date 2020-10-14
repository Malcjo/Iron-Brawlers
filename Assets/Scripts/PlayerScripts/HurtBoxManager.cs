using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxManager : MonoBehaviour
{
    public float radius;
    public Transform[] locator;
    public GameObject hurtbox;
    private PlayerControls playerControls;
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
