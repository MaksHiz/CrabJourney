using UnityEngine;
using System;
using UnityEngine.UIElements;

public class characterMovement : MonoBehaviour
{
    private Rigidbody2D body;
    //[SerializeField]
    private Animator animator;
    private GrabObjects grabObjects;

    private bool loopedAudio=false;
    private bool animateOnce = false;
    
    [SerializeField]

    enum State
    {
        InAir,
        OnWall,
        OnGround
    }
    enum LastPressed
    {
        Left,
        Right
    }

    [Header("Ground")]
    public bool onGround;
    [SerializeField] private float groundLength = 0.5f;
    [SerializeField] private Vector3 colliderOffset = new Vector3(0.5f, 0f, 0f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Horizontal")]
    [SerializeField, Range(0f, 30f)] public float maxGroundSpeed = 10f;
    [SerializeField, Range(0f, 100f)] public float maxGroundAcceleration = 50f;
    [SerializeField, Range(0f, 100f)] public float maxGroundDecceleration = 50f;
    [SerializeField, Range(0f, 100f)] public float maxGroundTurnSpeed = 80f;

    [Header("Air")]
    [SerializeField, Range(0f, 30f)] public float maxAirSpeed = 10f;
    [SerializeField, Range(0f, 100f)] public float maxAirAcceleration = 50f;
    [SerializeField, Range(0f, 100f)] public float maxAirDeceleration = 50f;
    [SerializeField, Range(0f, 100f)] public float maxAirTurnSpeed = 80f;

    [Header("Wall")]
    [SerializeField, Range(0f, 100f)] public float maxWallAcceleration = 50f;
    [SerializeField, Range(0f, 100f)] public float maxWallDecceleration = 50f;
    [SerializeField, Range(0f, 100f)] public float maxWallTurnSpeed = 80f;
    [SerializeField, Range(0f, 30f)] public float maxWallSpeed = 10f;

    public bool onWallLeft;
    public bool onWallRight;
    public bool onWall;
    [SerializeField] private float wallLength = 0.5f;
    [SerializeField] private Vector3 wallColliderOffset = new Vector3(0f, 0.5f, 0f);
    [SerializeField] private LayerMask wallLayer;

    [Header("Wall Jump")]
    [SerializeField, Range(1f, 10f)] public float strongWallJumpPushHorizontal = 5f;
    [SerializeField, Range(1f, 10f)] public float wallJumpHeight = 5f;
    [SerializeField, Range(0f, 0.3f)] public float wallWaitBuffer = 0.15f;
    [SerializeField, Range(0f, 30f)] public float wallJumpSpeedHorizontal = 10f;
    [SerializeField, Range(0f, 30f)] public float wallJumpSpeedVertical = 5f;


    [Header("Jump")]
    [SerializeField, Range(1f, 10f)] public float jumpHeight = 5f;
    [SerializeField, Range(0.1f, 2f)] public float timeToJumpApex = 0.5f;
    [SerializeField, Range(0f, 5f)] public float upwardMovementMultiplier = 1f;
    [SerializeField, Range(1f, 10f)] public float downwardMovementMultiplier = 5f;

    [SerializeField, Range(1f, 10f)] public float jumpCutOff;
    [SerializeField] public float fallSpeedLimit = 3f;
    [SerializeField, Range(0f, 0.3f)] public float coyoteTime = 0.15f;
    [SerializeField, Range(0f, 0.3f)] public float jumpBuffer = 0.15f;

    [Header("Hide")]
    [SerializeField, Range(1f, 30f)] public float shellFallingMultiuplier = 10f;


    [Header("State")]
    private bool desiredJump;
    private float jumpBufferCounter;
    private float coyoteTimeCounter = 0;
    private bool pressingJump;
    private bool currentlyJumping;

    private float wallWaitCounter;


    // private State prevState = State.InAir;
    // private bool isFacingRight = true;
    public bool pressingKeyHorizontal;
    public bool pressingKeyWall;

    public bool pressingJumpWall;
    public bool pressingOpposite;
    public bool holdingWall;

    public bool pressingLeft;
    public bool pressingRight;

    private LastPressed lastPressed;

    public bool inShell;


    [Header("Other")]
    private Vector2 desiredVelocity;
    public Vector2 velocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;

    private float horizontal;
    private float vertical;

    public float jumpSpeed;
    private float defaultGravityScale = 1f;
    public float gravMultiplier;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        wallWaitCounter = wallWaitBuffer + 1f;
        grabObjects = GetComponent<GrabObjects>();
        if(GameSave.CurrentSave != null)
        {
                if (GameSave.CurrentSave.LoadCrabToPosition == true)
                {
                    this.gameObject.transform.position=GameSave.CurrentSave.CrabPosition;
                    GameSave.CurrentSave.LoadCrabToPosition = false;
                }
        }
        
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("onGround", true);
        DateTime currentTime = DateTime.Now;
        int timeAsInt = (currentTime.Hour * 10000) + (currentTime.Minute * 100) + currentTime.Second;
        UnityEngine.Random.InitState(timeAsInt+1); //just for a different random seed than the one used in collectable Trash
    }

    private void OnDrawGizmos()
     {
         if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
         Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
         Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);

         if (onWall) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
         Gizmos.DrawLine(transform.position + wallColliderOffset, transform.position + wallColliderOffset + Vector3.left * wallLength);
         Gizmos.DrawLine(transform.position - wallColliderOffset, transform.position - wallColliderOffset + Vector3.left * wallLength);

         Gizmos.DrawLine(transform.position + wallColliderOffset, transform.position + wallColliderOffset + Vector3.right * wallLength);
         Gizmos.DrawLine(transform.position - wallColliderOffset, transform.position - wallColliderOffset + Vector3.right * wallLength);
    }
    private void checkCollision()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer)
                || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        onWallLeft = Physics2D.Raycast(transform.position + wallColliderOffset, Vector2.left, wallLength, wallLayer)
                || Physics2D.Raycast(transform.position - wallColliderOffset, Vector2.left, wallLength, wallLayer);

        onWallRight = Physics2D.Raycast(transform.position + wallColliderOffset, Vector2.right, wallLength, wallLayer)
                || Physics2D.Raycast(transform.position - wallColliderOffset, Vector2.right, wallLength, wallLayer);

        onWall = onWallLeft || onWallRight;
    }

    private void setGravity()
    {
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        body.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
    }
    private void updateHorizontal()
    {
        if (Input.GetButtonDown("Left"))
        {
            pressingLeft = true;
            lastPressed = LastPressed.Left;
        }
        else if (Input.GetButtonUp("Left"))
        {
            pressingLeft = false;
        }

        if (Input.GetButtonDown("Right"))
        {
            pressingRight = true;
            lastPressed = LastPressed.Right;
        }
        else if (Input.GetButtonUp("Right"))
        {
            pressingRight = false;
        }
    }
    public float getHorizontal()
    {
        if (lastPressed == LastPressed.Left)
        {
            if (pressingLeft) return -1;
            else if (pressingRight) return 1;
            else return 0;
        }
        else
        {
            if (pressingRight) return 1;
            else if (pressingLeft) return -1;
            else return 0;
        }
    }
    private void Update()
    {
        updateHorizontal();
        checkCollision(); 

        horizontal = getHorizontal();
        vertical = Input.GetAxisRaw("Vertical");

        if (!onGround) { animator.SetBool("onGround", false); }
        else { animator.SetBool("onGround", true); }

        if (onWall) { animator.SetBool("onWall", true); }
        else { animator.SetBool("onWall", false); }


        if (Input.GetButtonDown("Jump") && (onGround || onWall))
        {
            desiredJump = true;
            pressingJump = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            pressingJump = false;
        }

        pressingKeyWall = vertical != 0 ? true : false;
        pressingKeyHorizontal = horizontal != 0 ? true : false;
        if(onGround && horizontal!=0 && !loopedAudio)
        {
            loopedAudio = true;
            AudioManager.Instance.PlayLoopedSound("Walk");
        }
        else if((onGround || onWall) && horizontal==0 && loopedAudio
         || (!onWall && AudioManager.Instance.loopSoundSource.clip != null 
            && AudioManager.Instance.loopSoundSource.clip.name == "Climb"))
        {
            loopedAudio = false;
            AudioManager.Instance.StopLoopedSound();
        }
        pressingJumpWall = false;
        if(onWall && !inShell)
            animator.SetBool("facingWall", true);
        pressingOpposite = false;

        if (onGround)
        {
            wallWaitCounter = wallWaitBuffer + 1;
        }
        else if (!(wallWaitCounter > wallWaitBuffer))
        {
            wallWaitCounter += Time.deltaTime;
        }

        if (!currentlyJumping && !onGround)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = 0;
        }

        if (onWall && grabObjects.getGrabObject()==null && !inShell)
        {
            if(!loopedAudio && vertical != 0)
            {
                loopedAudio = true;
                AudioManager.Instance.PlayLoopedSound("Climb");
            }
            else if(loopedAudio && (vertical == 0 || onGround ))
            {
                loopedAudio = false;
                AudioManager.Instance.StopLoopedSound();
            }
            if (onWallLeft || onWallRight)
            {
                if (onWallLeft)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (onWallRight)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }


                if (holdingWall == false)
                {
                    pressingJump = false;
                }

                if (onWallLeft && pressingRight || onWallRight && pressingLeft)
                {
                    pressingOpposite = true;
                    animator.SetBool("facingWall", false);

                    vertical = 0;
                }
           
                //if (pressingRight) { transform.rotation = Quaternion.Euler(0, 0, 0); }
                //if (pressingLeft) { transform.rotation = Quaternion.Euler(0, 180, 0); }
                
                holdingWall = true;

                body.gravityScale = 0;

                if (pressingJump)
                {
                    setGravity();

                    desiredJump = false;

                    pressingJumpWall = true;

                    if (pressingOpposite)
                    {
                        desiredVelocity = new Vector2(strongWallJumpPushHorizontal * (onWallLeft ? 1 : -1), 0);
                    }
                    else
                    {
                        desiredVelocity = new Vector2(wallJumpSpeedHorizontal * (onWallLeft ? 1 : -1), wallJumpSpeedVertical);
                    }

                    wallWaitCounter = 0;

                    return;
                }

                desiredVelocity = new Vector2(0f, vertical * maxWallSpeed);

                return;
            }
        }

        holdingWall = false;
        
        setGravity();

        if (vertical < 0)
        {
            inShell = true;
            return;
        }
        else
        {
            inShell = false;
            this.gameObject.GetComponent<Animator>().SetBool("inShell", false);
        }

        if (horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (jumpBuffer > 0)
        {

            if (desiredJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > jumpBuffer)
                {
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            }
        }

        if (onGround)
        {
            desiredVelocity = new Vector2(horizontal * maxGroundSpeed, 0f);

            return;
        }
        else
        {
            desiredVelocity = new Vector2(horizontal * maxAirSpeed, 0f);
        }
    }

    private void FixedUpdate()
    {
        if (onGround && animateOnce)
        {
            this.gameObject.GetComponent<Animator>().SetBool("isJumping", false);
            animateOnce = false;
        }
        velocity = body.velocity;

        if (onWall && holdingWall)
        {
            this.gameObject.GetComponent<Animator>().SetBool("isJumping", false);
            animateOnce = false;
            moveWall();
            body.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -fallSpeedLimit, 100));
        }
        else //if (onGround)
        {
            if (inShell){
                this.gameObject.GetComponent<Animator>().SetBool("inShell", true);
                // sprite change
                calculateGravity();
                // Debug.Log("velocities " + velocity.x + " " + velocity.y + " " + desiredVelocity.x + " " + desiredVelocity.y);
                // return;    
            }
            else if (desiredJump)
            {
                Jump();
                body.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -fallSpeedLimit, 100));
            }
            else
            {
                calculateGravity();
            }

            if (wallWaitCounter > wallWaitBuffer)
            {
                moveHorizontal();
            }
        }
    }
    private void calculateGravity()
    {
        if (body.velocity.y > 0.01f)
        {
            if (onGround)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                if (inShell)
                {
                    gravMultiplier = shellFallingMultiuplier;
                }
                else if (pressingJump && currentlyJumping)
                {
                    gravMultiplier = upwardMovementMultiplier;
                }
                else
                {
                    gravMultiplier = jumpCutOff;
                }
            }
        }
        else if (body.velocity.y < -0.01f)
        {

            if (inShell)
            {
                gravMultiplier = shellFallingMultiuplier;
            }
            else if (onGround)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                gravMultiplier = downwardMovementMultiplier;
            }

        }
        else
        {
            if (onGround)
            {
                currentlyJumping = false;
            }

            gravMultiplier = defaultGravityScale;
        }

        body.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -fallSpeedLimit, 100));
    }

    private void Jump()
    {
        Debug.Log("Tried jumnping");
        if (onGround || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime))
        {
            if (UnityEngine.Random.Range(1, 3) == 1) { AudioManager.Instance.PlaySFX("jump"); }
            else { AudioManager.Instance.PlaySFX("jump2"); }
            if (!animateOnce)
            {
                this.gameObject.GetComponent<Animator>().SetBool("isJumping", true);
                animateOnce = true;
            }
            
            Debug.Log("Jumnped");
            desiredJump = false;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            // Debug.Log("" + Physics2D.gravity.y + " " + body.gravityScale + " " + jumpHeight);

            gravMultiplier = defaultGravityScale;

            setGravity();

            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * body.gravityScale * jumpHeight);

            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(body.velocity.y);
            }

            
            Debug.Log(velocity.y + " += " + jumpSpeed);
            Debug.Log("vel = " + velocity.y);
            velocity.y += jumpSpeed;
            currentlyJumping = true;
        }

        if (jumpBuffer == 0)
        {
            desiredJump = false;
        }
    }
    private void StrongWallJump()
    {
        Debug.Log("Strong jumnping");
        desiredJump = false;
        jumpBufferCounter = 0;
        coyoteTimeCounter = 0;

        gravMultiplier = defaultGravityScale;

        setGravity();

        jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * body.gravityScale * wallJumpHeight);

        if (velocity.y > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
        }
        else if (velocity.y < 0f)
        {
            jumpSpeed += Mathf.Abs(body.velocity.y);
        }

        // Debug.Log("y strong");
        Debug.Log(velocity.y + " += " + jumpSpeed);
        Debug.Log("vel = " + velocity.y);
        velocity.y += jumpSpeed;
    }

    public void moveWall()
    {
        acceleration = maxWallAcceleration;
        deceleration = maxWallDecceleration;
        turnSpeed = maxWallTurnSpeed;

        if (pressingJumpWall && !currentlyJumping)
        {
            currentlyJumping = true;
            Debug.Log("wall moving vel" + velocity.x + " to " + desiredVelocity.x);
            velocity.x = desiredVelocity.x;
            // Debug.Break();
            Debug.Log("y wall jump");
            // Debug.Log("= " + velocity.y);
            Debug.Log(velocity.y + " => " + desiredVelocity.y);
            velocity.y = desiredVelocity.y;
            if (pressingOpposite)
            {
                StrongWallJump();
                // Jump();
                wallWaitCounter = 0.1f;
                return;
            }
        }
        else if (wallWaitCounter > wallWaitBuffer)
        {
            currentlyJumping = false;

            if (pressingKeyWall)
            {
                if (Mathf.Sign(vertical) != Mathf.Sign(velocity.y))
                {
                    maxSpeedChange = turnSpeed * Time.deltaTime;
                }
                else
                {
                    maxSpeedChange = acceleration * Time.deltaTime;
                }
            }
            else
            {
                maxSpeedChange = deceleration * Time.deltaTime;
            }

            Debug.Log("y wall");
            velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
            velocity.y = Mathf.Clamp(velocity.y, -maxWallSpeed, maxWallSpeed);
            animator.SetFloat("yVelocity", Math.Abs(body.velocity.y));
        }
    }

    private void moveHorizontal()
    {
        // Debug.Log("Moving horizontal");

        acceleration = onGround ? maxGroundAcceleration : maxAirAcceleration;
        deceleration = onGround ? maxGroundDecceleration : maxAirDeceleration;
        turnSpeed = onGround ? maxGroundTurnSpeed : maxAirTurnSpeed;

        if (pressingKeyHorizontal)
        {
            if (Mathf.Sign(horizontal) != Mathf.Sign(velocity.x))
            {
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
            else
            {
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else
        {
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        // Debug.Log(desiredVelocity + " " + maxSpeedChange);

        // if (inShell && onGround)
        // {
        //     velocity.x = 0;
        // }
        // else
        // {
            
        // }
        
        // Debug.Log("moving vel" + velocity.x + " towards " + desiredVelocity.x);

        if(inShell)
        {
            if (onGround) velocity.x = 0;
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        }


        // Debug.Log(desiredVelocity);
        body.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -fallSpeedLimit, 100));
        if (onGround)
        {
            animator.SetFloat("xVelocity", Math.Abs(body.velocity.x));
        }
        
    }

}