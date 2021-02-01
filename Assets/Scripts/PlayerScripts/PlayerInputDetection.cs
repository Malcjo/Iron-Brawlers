using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using System.Runtime.CompilerServices;

public class PlayerInputDetection : MonoBehaviour
{
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
    [SerializeField]
    private float verticalInput;
    [SerializeField] 
    private float VerticalValue;
    [SerializeField] private PlayerConfiguration playerConfig;
    [SerializeField] Player.Wall currentWall;
    [SerializeField] private bool Standalone = false;



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
    public float GetVertical() 
    { 
        return VerticalValue; 
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
        if (context.action.name == playerControls.Player.PlayerVerticalMovement.name)
        {
            VerticalInput(context);
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

    public void VerticalInput(CallbackContext context)
    {
        if (self != null)
        {
            verticalInput = context.ReadValue<float>();
            if (verticalInput <= 0.35f && verticalInput >= -0.35f)
            {
                if (verticalInput < 0 && verticalInput >= -0.35f)
                {
                    verticalInput = -0;
                }
                else if (verticalInput > 0 && verticalInput <= 0.35f)
                {
                    verticalInput = 0;
                }
            }
            VerticalValue = verticalInput;
            //self.GetPlayerInputFromInputScript(VerticalValue);
            //WallCheck();
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

    private void CrouchInput(CallbackContext context)
    {
        if (context.started)
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
    }
    void ArmourBreakInput()
    {
    }


}   

