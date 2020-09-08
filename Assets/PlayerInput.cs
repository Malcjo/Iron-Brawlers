using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerInput : MonoBehaviour
{

    public float horizontal; // Being checked in Player script: MoveCall()
    public int canJump; //Being checked in Player script: JumpMove()

    [SerializeField] private float horizontalInput;
    [SerializeField] public int maxJumps;
    MixamoAnimations animationScript;

    [SerializeField] private Player player;
    [SerializeField] private PlayerControls controls;

    private void Start()
    {
        player = GetComponent<Player>();
        controls = GetComponent<PlayerControls>();
        animationScript = GetComponentInChildren<MixamoAnimations>();
    }
    private void FixedUpdate()
    {
        HorizontalInput();
        AttackInput();
        JumpInput();
    }
    public void HorizontalInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        horizontal = (horizontalInput);
        animationScript.Running();

        if (horizontalInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void JumpInput()
    {
        if (Input.GetKeyDown(controls.jumpKey))
        {
            player.JumpMove();
            canJump -= 1;
            if (canJump < 0)
            {
                canJump = 0;
            }
        }
        if (canJump > maxJumps && player.grounded == true)
        {
            canJump = maxJumps;
        }
    }

    public void AttackInput()
    {
        if (Input.GetKeyDown(controls.jabKey))
        {
            player.attack.JabAttack();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!Input.GetKeyDown(controls.jumpKey))
            {
                canJump++;
            }
            canJump--;
        }
    }

    //resetting number of jumps to max jumps
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = maxJumps;
        }
    }
}
