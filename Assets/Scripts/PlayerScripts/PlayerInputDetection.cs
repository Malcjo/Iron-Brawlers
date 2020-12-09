using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
public class PlayerInputDetection : MonoBehaviour
{
    [SerializeField]
    private PlayerSetup oldControlsSetup;
    [SerializeField]
    private Player self;
    private PlayerInput newInput;
    private PlayerControls playerControls;

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


    public float GetHorizontal(){return HorizontalValue;}
    private void Awake()
    {
        if(Standalone == false)
        {
            self = GetComponent<Player>();
            playerControls = new PlayerControls();
            self.SetUpInputDetectionScript(this);
            self.Standalone(false);
            
        }
        else
        {
            newInput = GetComponent<PlayerInput>();
            var _self = FindObjectsOfType<Player>();
            var index = newInput.playerIndex;
            self = _self.FirstOrDefault(m => m.GetPlayerIndex() == index);
            self.SetUpInputDetectionScript(this);
            self.Standalone(true);
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
        return CrouchInputQueued;
    }
    public bool ShouldArmourBreak(){
        return ArmourBreakInputQueued;
    }


    private void Update()
    {
        currentWall = self.GetCurrentWall();
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
        if(self != null)
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
            self.GetPlayerInputFromInputScript(HorizontalValue);
            WallCheck();
        }
    }

    void WallCheck()
    {
        switch (self.GetCurrentWall())
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
    private void CrouchInput()
    {
        CrouchInputQueued = false;
        if (Input.GetKey(oldControlsSetup.crouchKey))
        {
            CrouchInputQueued = true;
        }
    }
    //[SerializeField] private float viewableContext;
    public void JumpInput(CallbackContext context)
    {
        //viewableContext = context.started ? 1 : 0;
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
    public void BlockInput()
    {
        BlockInputQueued = false;
        if (Input.GetKey(oldControlsSetup.blockKey))
        {
            BlockInputQueued = true;
        }
    }
    void ArmourBreakInput()
    {
        ArmourBreakInputQueued = false;
        if (Input.GetKey(oldControlsSetup.armourKey) && Input.GetKey(oldControlsSetup.crouchKey))
        {
            ArmourBreakInputQueued = true;
        }
    }


}   

