using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] public int maxJumps;


    [SerializeField] public float horizontal;
    [SerializeField] private float horizontalInput;

    [SerializeField] private bool leftKey;
    [SerializeField] private bool rightKey;
    [SerializeField] public int canJump;

    [SerializeField] private Player player;
    private void Start()
    {
        player = GetComponent<Player>();
    }
    private void FixedUpdate()
    {
        HorizontalInput();
        AttackInput();
    }
    public void HorizontalInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        horizontal = (horizontalInput);

        if (horizontalInput < 0)
        {
            leftKey = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            leftKey = false;
        }
        if (horizontalInput > 0)
        {
            rightKey = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            rightKey = false;
        }
    }

    public void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.attack.JabAttack();
        }
    }
}
