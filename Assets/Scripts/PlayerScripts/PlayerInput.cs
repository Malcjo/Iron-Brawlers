using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private PlayerSetup controls;
    [SerializeField]
    private Player player;

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

    private void Update()
    {
        currentWall = player.GetCurrentWall();
        HorizontalInput();
        BlockInput();
        JumpInput();
        CrouchInput();
        AttackInput();
        ArmourBreakInput();
    }
    
    public void HorizontalInput()
    {
        horizontalInput = Input.GetAxisRaw(controls.horizontalKeys);
        HorizontalValue = horizontalInput;
        WallCheck();
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
    private void CrouchInput()
    {
        CrouchInputQueued = false;
        if (Input.GetKey(controls.crouchKey))
        {
            CrouchInputQueued = true;
        }
    }
    public void JumpInput()
    {
        JumpInputQueued = false;
        if (Input.GetKeyDown(controls.jumpKey))
        {
            JumpInputQueued = true;
        }
    }

    private void AttackInput()
    {
        AttackInputQueued = false;
        if (Input.GetKeyDown(controls.attackKey))
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

    public float GetHorizontal()
    {
        return HorizontalValue;
    }

    public bool ShouldJump()
    {
        return JumpInputQueued;
    }

    public bool ShouldAttack()
    {
        return AttackInputQueued;
    }
    public bool ShouldBlock()
    {
        return BlockInputQueued;
    }
    public bool ShouldCrouch()
    {
        return CrouchInputQueued;
    }
    public bool ShouldArmourBreak()
    {
        return ArmourBreakInputQueued;
    }
}   

