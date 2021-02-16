using System.Collections;
using System.Collections.Generic;
using ToonyColorsPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using static UnityEngine.InputSystem.InputAction;

public class SpawnPlayerSetupController : MonoBehaviour
{
    public GameObject playerSetupMenuPrefab;

    private GameObject rootMenu;
    public PlayerInput input;


    private void Awake()
    {
        rootMenu = GameObject.Find("MainCanvas");
        if(rootMenu != null)
        {
        //    var playercontroller = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
            input.uiInputModule = rootMenu.GetComponentInChildren<InputSystemUIInputModule>();
        //    playercontroller.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(input.playerIndex);
        }

    }
}
