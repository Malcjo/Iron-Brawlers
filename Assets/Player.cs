using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]private Vector3 horizontal;
    [SerializeField] private float horizontalInput;

    [SerializeField] private float weight;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int maxJumps;

    [SerializeField] private bool leftKey;
    [SerializeField] private bool rightKey;
    [SerializeField] private int canJump;

    [SerializeField] private bool grounded;

    [SerializeField]private PlayerAttack attack;

    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<PlayerAttack>();
        //canJump = maxJumps;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            canJump -= 1;
            if(canJump < 0)
            {
                canJump = 0;
            }
        }
        if(canJump > maxJumps && grounded == true)
        {
            canJump = maxJumps;
        }
    }
    private void FixedUpdate()
    {

        HorizontalInput();
        MoveCall();
        Gravity();


        JumpInput();
        AttackInput();

    }
    void MoveCall()
    {
        rb.velocity = horizontal;
    }
    void HorizontalInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        horizontal = new Vector3 ( horizontalInput * speed, rb.velocity.y, 0);



        if (horizontalInput < 0)
        {
            leftKey = true;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            leftKey = false;
        }
        if (horizontalInput > 0)
        {
            rightKey = true;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            rightKey = false;
        }



    }

    void JumpInput()
    {
        if(canJump> 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.velocity = (new Vector3(rb.velocity.x, jumpForce, rb.velocity.z));

            }
        }

    }
    void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attack.JabAttack();
        }
    }

    void Gravity()
    {
        rb.AddForce(Physics.gravity * (weight / 10));
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            canJump = maxJumps;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
