using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 input;
    private bool inputChanged;
    private bool jumpDown;
    private bool jumpUp;
    [SerializeField] bool isJumping;
    private float timeOfJump;
    private float timeOfLastJumpDown;

    private bool isGrounded;
    public float groundCheckLength;
    public float jumpStrength;
    public float releaseStrength;

    private Rigidbody2D rb;


    private void Start()
    {
        timeOfLastJumpDown = Time.time - 100f;
        rb = GetComponent<Rigidbody2D>();
        prevInput = new Vector2(0.0f, 0.0f);
        input = new Vector2(0, 0);
    }

    private void Update()
    {
        RetrieveInput();
        CheckGrounded();

        CalculateJump();

        CalculateHorizMov();
    }


    private Vector2 prevInput;
    private void OnMove(InputValue input)
    {
        inputChanged = true;
        this.input = input.Get<Vector2>();

        if (this.input.x == 0)
            isMoving = false;
        else
            isMoving = true;
    }

    private void RetrieveInput()
    {
        jumpDown = false;
        jumpUp = false;
        if (!inputChanged)
            return;
        inputChanged = false;

        //Set jumpDown and Up
        if (prevInput.y == 0.0f && input.y == 1.0f)
        {
            jumpDown = true;
            /*timeOfLastJumpDown = Time.time;*/
        }
        else if (prevInput.y == 1.0f && input.y == 0.0f)
        {
            jumpUp = true;
        }

        prevInput = input;
    }

    private bool isMoving;
    public float maxSpeed;
    public float acceleration;
    public float xDrag;
    private void CalculateHorizMov()
    {
        //Apply horizontal force
        rb.AddForce(new Vector2(this.input.x, 0) * acceleration);

        //If velocity is greater than max speed, set it to max speed
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            float newSpeed = Mathf.Abs(rb.velocity.x) / rb.velocity.x * maxSpeed;
            rb.velocity = new Vector2(newSpeed, rb.velocity.y);
        }

        //Apply drag force once movement is released.
        if (!isMoving && rb.velocity.x != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x / xDrag, rb.velocity.y);
        }
    }


    private void CheckGrounded()
    {
        //Shoot a boxcast slightly under the player's collider
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(0.4f, 0.8f), 0f, -Vector3.up, groundCheckLength);
        for (int i = 0; i < hits.Length; i++)
        {
            /*if (hits[i].collider.CompareTag("Ground") || hits[i].collider.CompareTag("Button") || hits[i].collider.CompareTag("Fan")
                || hits[i].collider.CompareTag("Door") || hits[i].collider.CompareTag("Box"))*/
            if (hits[i].collider.GetComponent<Rigidbody2D>() != null && hits[i].collider.gameObject != this.gameObject)
            {
                isGrounded = true;
                if (Time.time - 0.1 > timeOfJump)
                    isJumping = false;
                return;
            }
        }
        isGrounded = false;
    }

    private void CalculateJump()
    {
        //If jumping and jump button is released, apply downward force
        if (jumpUp == true && isJumping == true && isGrounded == false)
        {
            isJumping = false;
            float timeSinceJump = Time.time - timeOfJump;
            float releaseStrConst = (float) (MathF.Pow(releaseStrength, -timeSinceJump + 0.5f));
            rb.AddForce(new Vector2(0.0f, -releaseStrConst));
            return;
        }

        //Apply Upward Jump force
        if ((jumpDown == true && isGrounded == true))
        {
            isJumping = true;
            timeOfJump = Time.time;
            rb.AddForce(new Vector2(0.0f, jumpStrength));
        }
    }

    void OnRestart()
    {
        Projection.simSceneMade = false;
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

}
 