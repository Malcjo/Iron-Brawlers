using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEditorInternal;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int playerIndex;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button readyButton;
    [SerializeField] private MultiplayerEventSystem events;
    private Button currentSelectedButton;
    [SerializeField] private ColorBlock player1SelectedColour;
    [SerializeField] private ColorBlock player2SelectedColour;

    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }
    void Update()
    {
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }
    public void ChangedSelectedButton(Button newButton)
    {
        events.SetSelectedGameObject(newButton.gameObject);
    }
    public void SetPlayerCharacter(GameObject character)
    {
        if (!inputEnabled) { return; }

        //PlayerConfigurationManager.Instance.SetPlayerCharacter(playerIndex, character);
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }
    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }

        //PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
