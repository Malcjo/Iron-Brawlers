﻿using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private Player player;
    private PlayerInput playerInput;
    //private PlayerControls playerControls;
    private Player.PlayerIndex _PlayerNumber;
    [SerializeField] GameObject playerPrefab;

    [SerializeField] Scene currentScene;
    [SerializeField] Scene menuScene;
    GameObject playerCharacter = null;

    [SerializeField]
    private bool JumpInputQueued;
    [SerializeField]
    private bool BlockInputQueued;
    [SerializeField]
    private bool AttackInputQueued;
    [SerializeField]
    private bool ArmourBreakQueued;
    [SerializeField]
    private bool CrouchInputHeld;
    [SerializeField]
    private bool blockInputHeld;
    [SerializeField]
    private bool ArmourBreakInputQueued;
    [SerializeField]
    private float HorizontalValue;
    [SerializeField]
    private float horizontalInput;
    [SerializeField]
    private bool heavyQueued;
    [SerializeField]
    private bool UpDirectionHeld;

    [SerializeField] Player.Wall currentWall;
    [SerializeField] private bool Standalone = false;

    private CameraScript cameraScript;

    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        menuScene = SceneManager.GetSceneByBuildIndex(0);
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        //currentWall = player.GetCurrentWall();
        if(playerCharacter == null)
        {
            if(SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(1).buildIndex)
            {
                currentScene = SceneManager.GetActiveScene();
                StartGame();
            }
        }
    }
    private void StartGame()
    {
        playerCharacter = Instantiate(playerPrefab);
        GameManager.instance.AddPlayerToList(playerCharacter);
        cameraScript = GameManager.instance.GetCameraScript();
        cameraScript.AddPlayers(playerCharacter);
        player = playerCharacter.GetComponent<Player>();
        player.SetUpInputDetectionScript(this);
        player.playerNumber = _PlayerNumber;
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
    }
    public void SetInput(PlayerInput input)
    {
        this.playerInput = input;
    }

    public void SetPlayerNumber(PlayerInputManager inputManager)
    {
        if (inputManager.playerCount == 1)
        {
            _PlayerNumber = Player.PlayerIndex.Player1;
        }
        else if (inputManager.playerCount == 2)
        {
            _PlayerNumber = Player.PlayerIndex.Player2;
        }
    }
    public float GetHorizontal()
    {
        return HorizontalValue;
    }
    public bool ShouldJump(){
        if (JumpInputQueued)
        {
            JumpInputQueued = false;
            return true;
        }
        return false;
    }
    public bool ShouldAttack(){
        if (AttackInputQueued)
        {
            AttackInputQueued = false;
            return true;
        }
        return false;
    }

    public bool ShouldCrouch(){
        return CrouchInputHeld;
    }
    public bool ShouldBlock()
    {
        return blockInputHeld;
    }
    public bool ShouldArmourBreak()
    {
        if (ArmourBreakInputQueued)
        {
            ArmourBreakInputQueued = false;
            return true;
        }
        return false;
    }
    public bool ShouldHeavy()
    {
        if (heavyQueued)
        {
            heavyQueued = false;
            return true;
        }
        return false;
    }

    public bool ShouldUpDirection()
    {
        return UpDirectionHeld;
    }


    public void HorizontalInput(CallbackContext context)
    {
        if(player != null)
        {
            horizontalInput = context.ReadValue<float>();
            if(horizontalInput <= 0.35f && horizontalInput >= -0.35f)
            {
                if(horizontalInput < 0 && horizontalInput >= -0.35f)
                {
                    horizontalInput = -0;
                }
                else if (horizontalInput > 0 && horizontalInput <= 0.35f)
                {
                    horizontalInput = 0;
                }
            }
            HorizontalValue = horizontalInput;
            player.GetPlayerInputFromInputScript(HorizontalValue);
            WallCheck();
        }
    }
    public void JumpInput(CallbackContext context)
    {
        if (context.started)
        {
            JumpInputQueued = true;
        }
    }

    public void AttackInput(CallbackContext context)
    {
        if (context.started)
        {
            AttackInputQueued = true;
        }
    }
    public void CrouchInput(CallbackContext context)
    {
        if (context.started)
        {
            CrouchInputHeld = true;
        }
        if (context.canceled)
        {
            CrouchInputHeld = false;
        }
    }

    public void BlockInput(CallbackContext context)
    {
        if (context.started)
        {
            blockInputHeld = true;
        }
        if (context.canceled)
        {
            blockInputHeld = false;
        }
    }

    public void ArmourBreakInput(CallbackContext context)
    {
        if (context.started)
        {
            ArmourBreakInputQueued = true;
        }
    }
    public void HeavyInput(CallbackContext context)
    {
        if (context.started)
        {
            heavyQueued = true;
        }
    }

    public void UpDirectionInput(CallbackContext context)
    {
        if (context.started)
        {
            UpDirectionHeld = true;
        }
        if (context.canceled)
        {
            UpDirectionHeld = false;
        }
    }

    public void WallCheck()
    {
        switch (player.GetCurrentWall())
        {
            case Player.Wall.none:
                break;
            case Player.Wall.leftWall:
                if (HorizontalValue < 0)
                {
                    HorizontalValue = 0;
                }
                break;
            case Player.Wall.rightWall:
                if (HorizontalValue > 0)
                {
                    HorizontalValue = 0;
                }
                break;
        }
    }
}   

