using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private PlayerSetup controls;
    [SerializeField]
    private Player self;

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

    [SerializeField] Player.Wall currentWall;
    public float GetHorizontal(){return HorizontalValue;}
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
        //HorizontalInput();
        BlockInput();
        //JumpInput();
        CrouchInput();
        //AttackInput();
        ArmourBreakInput();
    }
    
    public void HorizontalInput(CallbackContext context)
    {
        horizontalInput = context.ReadValue<float>();
        HorizontalValue = horizontalInput;
        self.GetPlayerInputFromInputScript(HorizontalValue);
        WallCheck();
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
        if (Input.GetKey(controls.crouchKey))
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
        if (Input.GetKey(controls.blockKey))
        {
            BlockInputQueued = true;
        }
    }
    void ArmourBreakInput()
    {
        ArmourBreakInputQueued = false;
        if (Input.GetKey(controls.armourKey) && Input.GetKey(controls.crouchKey))
        {
            ArmourBreakInputQueued = true;
        }
    }


}   

