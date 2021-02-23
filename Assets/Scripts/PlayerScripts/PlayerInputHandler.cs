using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private Player player;
    private PlayerInput playerInput;
    private PlayerControls playerControls;
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
    private bool CrouchInputQueued;
    [SerializeField]
    private bool ArmourBreakInputQueued;
    [SerializeField]
    private float HorizontalValue;
    [SerializeField]
    private float horizontalInput;

    [SerializeField] private PlayerConfiguration playerConfig;
    [SerializeField] Player.Wall currentWall;
    [SerializeField] private bool Standalone = false;



    private void Awake()
    {
        //if(Standalone == false)
        //{
        //    player = GetComponent<Player>();
        //    playerControls = new PlayerControls();
        //    player.SetUpInputDetectionScript(this);
        //    player.Standalone(false);
            
        //}
        //else
        //{
        //    playerInput = GetComponent<PlayerInput>();
        //    var _self = FindObjectsOfType<Player>();
        //    var index = playerInput.playerIndex;
        //    player = _self.FirstOrDefault(m => m.GetPlayerIndex() == index);
        //    player.SetUpInputDetectionScript(this);
        //    player.Standalone(true);
        //}
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
        player = playerCharacter.GetComponent<Player>();
        player.SetUpInputDetectionScript(this);
        player.playerNumber = _PlayerNumber;
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
    public bool ShouldBlock(){
        return BlockInputQueued;
    }
    public bool ShouldCrouch(){
        if (CrouchInputQueued)
        {
            CrouchInputQueued = false;
            return true;
        }
        return false;
    }
    public bool ShouldArmourBreak(){
        return ArmourBreakInputQueued;
    }
    public float GetHorizontal() 
    { 
        return HorizontalValue; 
    }

    public void InitializePlayer(PlayerConfiguration config)
    {
        if (Standalone == false)
        {
            return;
        }
        playerConfig = config;
        config.input.onActionTriggered += Input_OnActionTrigger;
    }
    private void Input_OnActionTrigger(CallbackContext context)
    {
        if (Standalone == false)
        {
            return;
        }
        if (context.action.name == playerControls.Player.PlayerHorizontalMovement.name)
        {
            HorizontalInput(context);
        }
        if (context.action.name == playerControls.Player.PlayerJump.name)
        {
            JumpInput(context);
        }
        if (context.action.name == playerControls.Player.PlayerAttack.name)
        {
            AttackInput(context);
        }
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
    private void CrouchInput(CallbackContext context)
    {
        if (context.started)
        {
            CrouchInputQueued = true;
        }
    }





    void WallCheck()
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

