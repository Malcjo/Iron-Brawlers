using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private PlayerSetup controls;

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
    private float HorizontalValue;
    private float horizontalInput;

    private enum Wall
    {
        leftWall,
        rightWall,
        none
    }

    private Wall CurrentWall;

    private void Update()
    {
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
        switch (CurrentWall)
        {
            case Wall.none:
                break;
            case Wall.leftWall:
                if (horizontalInput < 0)
                {
                    horizontalInput = 0;
                }
                break;
            case Wall.rightWall:
                if (horizontalInput > 0)
                {
                    horizontalInput = 0;
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
        animationScript.Jump(true);
            JumpInputQueued = true;
            numberOfJumps --;
            if (numberOfJumps < 0)
            {
                canJump = true;
                numberOfJumps = 0;
            }
        }
        if (numberOfJumps < maxJumps && player.grounded == true)
        {
            numberOfJumps = maxJumps;
        }
    }

    public void AeiralAttackCheck()
    {
        if (CurrentState == State.busy)
        {
            return;
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
        else
        {
            BlockInputQueued = false;
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

