using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{

    private float movementInputDirection;
    private float CountJump;
    private float valueSlideUse;
    private float valueSpeedUse;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;

    private int facingDirection = 1;


    private bool isFacingRight = true;
    private bool isRunning;
    private bool isGrounding;
    private bool isTouchingWall;
    private bool canJump;
    private bool isWallSliding;
    private bool isDashing;
    private bool isDeath;
    public bool canMove = true;

    private Rigidbody2D rb;
    private Animator anim;

    public float movementSpeed = 10.5f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float nbJump = 2;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCooldown;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform StromPointDeath;


    public LayerMask whatIsGround;
    public LayerMask whatIsDeath;

    public GameObject GameOver;
    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        music.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        if(canMove){
            CheckMovementDirection();
            UpdateAnimations();
            CheckIfCanJump();
            CheckIfWallSliding();
            CheckDash();
            CheckIfDeath();
        }

    }

    private void FixedUpdate(){
        ApplayMovement();
        CheckSurroundings();
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounding && rb.velocity.y < 0)
        {
            canJump = true;
            CountJump = 0;
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }


    private void CheckSurroundings(){
        isGrounding = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isDeath = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsDeath);
    }



    private void CheckIfCanJump(){
        if(isGrounding){
            canJump = true;
            CountJump = 1;
        }else{
            if(nbJump-1>=CountJump){
                canJump = true;
            }else{
                canJump = false;
            }
            
        }
    }

    private void CheckIfDeath(){
        if(StromPointDeath.position.x>this.transform.position.x){
            isDeath=true;
        }
        if (isDeath){
            movementSpeed = 0;
            canMove = false;
            music.Play();
            rb.velocity = new Vector2(0, 0);
            GameOver.SetActive(true);
        }
    }


    private void CheckMovementDirection(){
        if(isFacingRight && movementInputDirection < 0){
            Flip();
        }else if(!isFacingRight && movementInputDirection >0){
            Flip();
        }
        if(movementInputDirection != 0 ){
            isRunning = true;
            
        }else{
            isRunning = false;
        }
    }

    private void UpdateAnimations(){
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded",isGrounding);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding",isWallSliding);
    }

    private void CheckInput(){
        if(canMove){
            movementInputDirection = Input.GetAxisRaw("Horizontal");
            if(Input.GetButtonDown("Jump")){
                Jump();
            }
            if(Input.GetKeyDown(KeyCode.S) && !isGrounding){
                valueSlideUse = wallSlideSpeed*3;
            }else{
                if(isGrounding || Input.GetKeyUp(KeyCode.S)){
                    valueSlideUse = wallSlideSpeed;
                }
            }

            if(Input.GetButtonDown("Dash")){
                if(Time.time >= (lastDash + dashCooldown))
                valueSpeedUse = movementSpeed;
                AttemptToDash();
            }    

        }

        if(Input.GetKeyDown(KeyCode.R)){
            Debug.Log("Restart");
            SceneManager.LoadScene("Game");
        }      
    }

    private void AttemptToDash(){
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            
            if(dashTimeLeft > 0)
            {
                movementSpeed = dashSpeed;
                dashTimeLeft -= Time.deltaTime;
                
                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if(dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;

                movementSpeed = valueSpeedUse;
            }
            
        }
    }





    private void Jump(){
        if(canJump && nbJump-1>=CountJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            CountJump += 1;
        }
    }

    private void ApplayMovement(){
        
        if (isGrounding)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        else if(!isGrounding && !isWallSliding && movementInputDirection != 0)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirection, 0);
            rb.AddForce(forceToAdd);

            if(Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
            }
        }
        else if(!isGrounding && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }

        if (isWallSliding)
        {
            if(rb.velocity.y < -valueSlideUse)
            {
                rb.velocity = new Vector2(rb.velocity.x, -valueSlideUse);
            }
        }
    }

    private void Flip(){
        if (!isWallSliding)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }




    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }


}
