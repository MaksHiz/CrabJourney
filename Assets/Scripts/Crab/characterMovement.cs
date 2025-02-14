﻿using UnityEngine;
using System;
using UnityEngine.UIElements;

public class characterMovement : MonoBehaviour
{
    private Rigidbody2D body;
    //[SerializeField]
    public Animator animator;
    public SpriteRenderer sprite;
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

    [SerializeField, Range(0f, 30f)] public float topOfWallPush = 5f;

    public bool onWallLeft;
    public bool onWallRight;
    public bool onWall;

    public bool middleWallLeft;
    public bool middleWallRight;
    public bool middleWall;

    public bool aboveWallLeft;
    public bool aboveWallRight;
    public bool aboveWall;

    public bool wasOnWall;


    [SerializeField] private float wallLength = 0.5f;
    [SerializeField] private Vector3 wallColliderOffset1 = new Vector3(0f, 0.5f, 0f);
    [SerializeField] private Vector3 wallColliderOffset2 = new Vector3(0f, 0.1f, 0f);
    [SerializeField] private Vector3 wallColliderOffset3 = new Vector3(0f, 0.8f, 0f);
    // [SerializeField] private Vector3 wallColliderOffset4 = new Vector3(0f, 0.54f, 0f);
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

    public bool lookingRight = true;

    private float[] posXs;
    private float[] colXs;


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

        posXs = new float[transform.childCount];
        colXs = new float[transform.childCount];

        for (int i=0; i<transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            posXs[i] = child.transform.localPosition.x;

            BoxCollider2D collider = child.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                // collider.offset = new Vector2(-collider.offset.x, collider.offset.y);
                colXs[i] = collider.offset.x;
            }
            
        } 
    }
    private void Start()
    {
        //animator = this.gameObject.GetComponentInChildren<Animator>();
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

         if (middleWall) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
         Gizmos.DrawLine(transform.position + wallColliderOffset1, transform.position + wallColliderOffset1 + Vector3.left * wallLength);
         Gizmos.DrawLine(transform.position - wallColliderOffset1, transform.position - wallColliderOffset1 + Vector3.left * wallLength);

         Gizmos.DrawLine(transform.position + wallColliderOffset1, transform.position + wallColliderOffset1 + Vector3.right * wallLength);
         Gizmos.DrawLine(transform.position - wallColliderOffset1, transform.position - wallColliderOffset1 + Vector3.right * wallLength);

         Gizmos.DrawLine(transform.position + wallColliderOffset2, transform.position + wallColliderOffset2 + Vector3.left * wallLength);
         Gizmos.DrawLine(transform.position - wallColliderOffset2, transform.position - wallColliderOffset2 + Vector3.left * wallLength);

         Gizmos.DrawLine(transform.position + wallColliderOffset2, transform.position + wallColliderOffset2 + Vector3.right * wallLength);
         Gizmos.DrawLine(transform.position - wallColliderOffset2, transform.position - wallColliderOffset2 + Vector3.right * wallLength);

         if (aboveWall) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }

         Gizmos.DrawLine(transform.position - wallColliderOffset3, transform.position - wallColliderOffset3 + Vector3.left * wallLength);

         Gizmos.DrawLine(transform.position - wallColliderOffset3, transform.position - wallColliderOffset3 + Vector3.right * wallLength);

        //  Gizmos.DrawLine(transform.position - wallColliderOffset4, transform.position - wallColliderOffset4 + Vector3.left * wallLength);

        //  Gizmos.DrawLine(transform.position - wallColliderOffset4, transform.position - wallColliderOffset4 + Vector3.right * wallLength);
    } 
    private void checkCollision()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer)
                || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        middleWallLeft = Physics2D.Raycast(transform.position + wallColliderOffset1, Vector2.left, wallLength, wallLayer)
                || Physics2D.Raycast(transform.position - wallColliderOffset1, Vector2.left, wallLength, wallLayer)
                || Physics2D.Raycast(transform.position + wallColliderOffset2, Vector2.left, wallLength, wallLayer)
                || Physics2D.Raycast(transform.position - wallColliderOffset2, Vector2.left, wallLength, wallLayer);

        middleWallRight = Physics2D.Raycast(transform.position + wallColliderOffset1, Vector2.right, wallLength, wallLayer)
                || Physics2D.Raycast(transform.position - wallColliderOffset1, Vector2.right, wallLength, wallLayer)
                || Physics2D.Raycast(transform.position + wallColliderOffset2, Vector2.right, wallLength, wallLayer)
                || Physics2D.Raycast(transform.position - wallColliderOffset2, Vector2.right, wallLength, wallLayer);

        middleWall = middleWallLeft || middleWallRight;

        // onWall = onWallLeft || onWallRight;


        aboveWallLeft = Physics2D.Raycast(transform.position - wallColliderOffset3, Vector2.left, wallLength, wallLayer);
                // || Physics2D.Raycast(transform.position - wallColliderOffset4, Vector2.left, wallLength, wallLayer);

        aboveWallRight = Physics2D.Raycast(transform.position - wallColliderOffset3, Vector2.right, wallLength, wallLayer);
                // || Physics2D.Raycast(transform.position - wallColliderOffset4, Vector2.right, wallLength, wallLayer);

        aboveWall = aboveWallLeft || aboveWallRight;
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
    public void rotateAll(bool changeDirection) // true = right, false = left
    {
        if ((lookingRight && !changeDirection) || (!lookingRight && changeDirection))
            lookingRight = !lookingRight;
            for (int i=0; i<transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                // Debug.Log(child.name);
                if (child.name != "Sprite")
                {
                    // Debug.Log("child poss");
                    // Debug.Log((lookingRight?1:-1)*posXs[i] + " " + child.transform.localPosition.y);
                    child.transform.localPosition = new Vector2((lookingRight?1:-1)*posXs[i], child.transform.localPosition.y);
                    // Debug.Log((lookingRight?1:-1)*posXs[i] + " " + child.transform.localPosition.y);

                    // child.transform.localPosition = new Vector2(-child.transform.localPosition.x, child.transform.localPosition.y);

                    BoxCollider2D collider = child.GetComponent<BoxCollider2D>();
                    if (collider != null)
                    {
                        collider.offset = new Vector2((lookingRight?1:-1)*colXs[i], collider.offset.y);
                        // collider.offset = new Vector2(-collider.offset.x, collider.offset.y);
                    }
                }
            }
    }
    private void Update()
    {
        updateHorizontal();
        checkCollision(); 

        if (middleWall) 
        {
            wasOnWall = true;
        }
        if ((!middleWall && !aboveWall) || onGround)
        {
            wasOnWall = false;
        }

        // Debug.Log("onWallLeft");
        // Debug.Log(middleWallLeft + " " + wasOnWall + " " +  aboveWallLeft);
        // Debug.Log(middleWallLeft + " " +  (wasOnWall?aboveWallLeft:false));

        // Debug.Log("onWallRight");
        // Debug.Log(middleWallRight + " " + wasOnWall + " " +  aboveWallRight);
        // Debug.Log(middleWallRight + " " +  (wasOnWall?aboveWallRight:false));

        onWallLeft = middleWallLeft || (wasOnWall?aboveWallLeft:false);

        onWallRight = middleWallRight || (wasOnWall?aboveWallRight:false);

        onWall = onWallLeft || onWallRight;

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
        } //
        else if(((onGround && horizontal==0 && loopedAudio) || (onWall && vertical==0 && loopedAudio))
         || (!onWall && AudioManager.Instance.loopSoundSource.clip != null 
            && AudioManager.Instance.loopSoundSource.clip.name == "Climb")
         || (!onGround && AudioManager.Instance.loopSoundSource.clip != null 
            && AudioManager.Instance.loopSoundSource.clip.name == "Walk"))
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
                // Debug.Log("Stopped 2");
            }
            if (onWallLeft || onWallRight)
            {
                if (onWallLeft)
                {
                    // transform.rotation = Quaternion.Euler(0, 180, 0);
                    rotateAll(false);
                    sprite.flipX=true;
                }
                else if (onWallRight)
                {
                    // transform.rotation = Quaternion.Euler(0, 0, 0);
                    rotateAll(true);
                    sprite.flipX=false;
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
		            loopedAudio = false;
            	    AudioManager.Instance.StopLoopedSound();	
                    // Debug.Log("Stopped 3");
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

                if (!middleWall && aboveWall)
                {
                    desiredVelocity = new Vector2((onWallLeft?-1:1) * topOfWallPush, vertical * maxWallSpeed);
                }
                else
                {
                    desiredVelocity = new Vector2(0f, vertical * maxWallSpeed);
                }

                return;
            }
        }

        holdingWall = false;
        
        setGravity();

        if (vertical < 0)
        {
            inShell = true;
            grabObjects.ReleaseGrabbedObject();
	        loopedAudio = false;
            AudioManager.Instance.StopLoopedSound();
            // Debug.Log("Stopped 4");
            return;
        }
        else
        {
            inShell = false;
           animator.SetBool("inShell", false);
        }

        if (horizontal < 0)
        {
	    // transform.rotation = Quaternion.Euler(0, 180, 0);
            rotateAll(false);
            sprite.flipX=true;
        }
        else if (horizontal > 0)
        {
        //    transform.rotation = Quaternion.Euler(0, 0, 0);
           rotateAll(true);
            sprite.flipX=false;
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
        updateTimer();
        if (onGround && animateOnce)
        {
           animator.SetBool("isJumping", false);
            animateOnce = false;
        }
        velocity = body.velocity;

        if (onWall && holdingWall)
        {
           animator.SetBool("isJumping", false);
            animateOnce = false;
            moveWall();
            body.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -fallSpeedLimit, 100));
        }
        else //if (onGround)
        {
            if (inShell){
                animator.SetBool("inShell", true);
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
        // Debug.Log("Tried jumnping");
        if (onGround || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime))
        {
            if (UnityEngine.Random.Range(1, 3) == 1) { AudioManager.Instance.PlaySFX("jump"); }
            else { AudioManager.Instance.PlaySFX("jump2"); }
            if (!animateOnce)
            {
                animator.SetBool("isJumping", true);
                animateOnce = true;
            }
            
            // Debug.Log("Jumnped");
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

            
            // Debug.Log(velocity.y + " += " + jumpSpeed);
            // Debug.Log("vel = " + velocity.y);
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
        // Debug.Log("Strong jumnping");
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
        // Debug.Log(velocity.y + " += " + jumpSpeed);
        // Debug.Log("vel = " + velocity.y);
        velocity.y += jumpSpeed;
    }

    public void moveWall()
    {
        acceleration = maxWallAcceleration;
        deceleration = maxWallDecceleration;
        turnSpeed = maxWallTurnSpeed;

        if (pressingJumpWall && !currentlyJumping)
        {
            if (UnityEngine.Random.Range(1, 3) == 1) { AudioManager.Instance.PlaySFX("jump"); }
            else { AudioManager.Instance.PlaySFX("jump2"); }
            if (!animateOnce)
            {
                animator.SetBool("isJumping", true);
                animateOnce = true;
            }

            currentlyJumping = true;
            // Debug.Log("wall moving vel" + velocity.x + " to " + desiredVelocity.x);
            velocity.x = desiredVelocity.x;
            // Debug.Break();
            // Debug.Log("y wall jump");
            // Debug.Log("= " + velocity.y);
            // Debug.Log(velocity.y + " => " + desiredVelocity.y);
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

            // Debug.Log("y wall");
            if (desiredVelocity.x != 0) 
            {
                velocity.x = desiredVelocity.x;
            }
            velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
            velocity.y = Mathf.Clamp(velocity.y, -maxWallSpeed, maxWallSpeed);
            animator.SetFloat("yVelocity", Math.Abs(body.velocity.y));
            
            // velocity.x = (onWallLeft ? -1 : 1) * 0.1;
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

    void updateTimer(){
        GameSave.CurrentSave.TimeSpentPlaying += Time.fixedDeltaTime;
    }
}