using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Debugging")] 
    [SerializeField] bool debug;

    [Header("References")]
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;
    [SerializeField] private LayerMask groundLayer;
    private PlayerController controller;

    [Header("Movement")]
    [SerializeField] bool enableRunning = true;
    public float movementSpeed = 0f;
    [SerializeField] [Range(0f, 20f)] private float defaultMoveSpeed = 10;
    [SerializeField] [Range(10f, 20f)] private float runSpeed = 0f;
    [SerializeField] [Range(0f, 30f)] private float jumpForce = 0f;
    public float horizontalMovement { get; private set; }
    private bool run;
    private bool jump;
    public bool isGrounded;
    private const float extraCapsuleHeight = .2f; //Value for the capsuleCast to check if player is grounded.

    //Slope Management
    public bool isOnSlope { get; private set; }
    private float slopeAngle;
    private float slopeOldAngle;
    private Vector2 slopeNormalPerp;
    private float slopeSlideAngle;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();        
    }

    private void Start()
    {
        movementSpeed = defaultMoveSpeed;
    }

    void Update()
    {
        isGrounded = IsGrounded();
        HandleInput();
        SlopeCheck();
    }
    private void FixedUpdate()
    {
        Move();
        Run();
        Jump();
    }


    void HandleInput()
    {
        //Moving
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        //Running
        if (Input.GetKeyDown(KeyCode.LeftShift) && enableRunning)
        {
            run = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
        }


        //Jumping
        if (Input.GetButtonDown("Jump"))
            jump = true;
    }
    void Move()
    {
        controller.rb2D.velocity = new Vector2(horizontalMovement * movementSpeed, controller.rb2D.velocity.y);

    }
    void Run()
    {
        //Running
        if (run)
            movementSpeed = runSpeed;
        else if (!run)
            movementSpeed = defaultMoveSpeed;

    }
    void Jump()
    {    
        if (jump && IsGrounded())
        {        
            controller.rb2D.AddForce(new Vector2(0f, jumpForce),ForceMode2D.Impulse);
            jump = false;
        }

    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, controller.capCollider2D.size.y / 2);

        SlopeCheckVertical(checkPos);
        SlopeCheckHorizontal(checkPos);
    }
    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, 0.5f, groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, 0.5f, groundLayer);

        if(slopeHitFront)
        {
            isOnSlope = true;
            slopeSlideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if(slopeHitBack)
        {
            isOnSlope = true;
            slopeSlideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSlideAngle = 0.0f;
            isOnSlope = false;
        }

    }
    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, 0.5f);

        if(hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeAngle != slopeOldAngle)
            {
                isOnSlope = true;
            }

            slopeOldAngle = slopeAngle;

            if (debug) 
            {
                Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
                Debug.DrawRay(hit.point, hit.normal, Color.green);
            }
                
        }

        if(isOnSlope && horizontalMovement == 0.0f)
        {
            controller.rb2D.sharedMaterial = fullFriction;

        }
        else
        {
            controller.rb2D.sharedMaterial = noFriction;
        }

    }
    
    private bool IsGrounded()
    {      
        RaycastHit2D rHit = Physics2D.CapsuleCast(controller.capCollider2D.bounds.center, controller.capCollider2D.bounds.size, capsuleDirection: 0f, 0f, Vector2.down, extraCapsuleHeight, groundLayer);

        if(debug)
        {
            Color rColor;

            if (rHit.collider != null)
            {
                rColor = Color.green;
            }
            else
            {
                rColor = Color.red;
            }

            Debug.DrawRay(controller.capCollider2D.bounds.center, Vector2.down * (controller.capCollider2D.bounds.extents.y + extraCapsuleHeight), rColor);

            Debug.Log(rHit.collider);
        }

        return rHit.collider != null;
    }

    
}


