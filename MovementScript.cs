using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    // components
    private Transform t;
    private Rigidbody2D rb;
    private CollisionCheck cc;

    // variables
    private Vector2 vel;
    private bool hadJumped;
    private bool hadJumpedOffWall;
    private int numberOfJumps;

    int i = 0;
    int ii = 0;
    
	// initialization
	private void Start ()
    {
        // sets the components
        t = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CollisionCheck>();

        
        vel = Vector2.zero;
        hadJumped = false;
        hadJumpedOffWall = false;
        numberOfJumps = 0;

    }

    private void Update()
    {
        Debug.Log("frame " + ++i);
        // if the player had just pressed the jump button, sets the variable to true
        if (Input.GetKeyDown(KeyCode.C))
        {
            hadJumped = true;
        }
    }

    private bool Jump()
    {
        // if they had just pressed the jump button, sets it back to false and returns true;
        if (hadJumped)
        {
            hadJumped = false;
            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        Debug.Log("fixed " + ++ii);
        vel = rb.velocity;

        HandleGravity();
        HandleMovement();

        rb.velocity = vel;
    }

    private void HandleGravity()
    {
        if (cc.Bottom() || cc.Left() || cc.Right())
        {
            numberOfJumps = 1;
        }

        if (Jump() && numberOfJumps != 0)
        {
            numberOfJumps--;
            vel.y = 21.5f;
            if (cc.Left() || cc.Right())
            {
                hadJumpedOffWall = true;
                if (cc.Left())
                {
                    vel.x = 15f;
                }
                if (cc.Right())
                {
                    vel.x = -15f;
                }
            }
        }

        if (Input.GetKey(KeyCode.C) && vel.y > 0.0f)
        {
            vel.y -= 32f * Time.fixedDeltaTime;
        }
        else if (vel.y > 0.0f)
        {
            vel.y -= 50f * Time.fixedDeltaTime;
        }

        if (vel.y <= 0.0f)
        {
            vel.y -= 47f * Time.fixedDeltaTime;
        }
        
        if ((cc.Left() || cc.Right()) && vel.y < 0.0f)
        {
            vel.y *= 0.99f;
        }

        if (vel.y <= -21.5f && Input.GetKey(KeyCode.C))
        {
            vel.y = -21.5f;
        }
        else if (vel.y <= -24f)
        {
            vel.y = -24f;
        }

        if (vel.y <= 0.0f && cc.Bottom())
        {
            vel.y = 0.0f;
        }
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow) ^ Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (vel.x == 0.0f)
                {
                    vel.x = -7f;
                }
                if (vel.x > 2.0f && !hadJumpedOffWall)
                {
                    vel.x = 2f;
                }
                
                if (vel.x > -10f)
                {
                    vel.x -= 30f * Time.fixedDeltaTime;
                }
                else
                {
                    vel.x += 45.0f * Time.fixedDeltaTime;
                }
                
                if (cc.Left() && vel.x <= 0.0f)
                {
                    vel.x = 0.0f;
                }
                if (vel.x < -10.0f && !hadJumpedOffWall)
                {
                    vel.x = -10.0f;
                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (vel.x == 0.0f)
                {
                    vel.x = 7f;
                }
                if (vel.x < -2.0f && !hadJumpedOffWall)
                {
                    vel.x = -2f;
                }

                if (vel.x < 10f)
                {
                    vel.x += 30f * Time.fixedDeltaTime;
                }
                else
                {
                    vel.x -= 45.0f * Time.fixedDeltaTime;
                }

                if (cc.Right() && vel.x >= 0.0f)
                {
                    vel.x = 0.0f;
                }
                if (vel.x > 10.0f && !hadJumpedOffWall)
                {
                    vel.x = 10.0f;
                }
            }
        }
        else
        {
            if (vel.x > 0.0f)
            {
                vel.x -= 45.0f * Time.fixedDeltaTime;
            }
            if (vel.x < 0.0f)
            {
                vel.x += 45.0f * Time.fixedDeltaTime;
            }
            //Debug.Log(vel);
            if ((vel.x < 2f) && (vel.x > -2f))
            {
                vel.x = 0;
            }
        }

        if ((vel.x < 10.0f) && (vel.x > -10.0f))
        {
            hadJumpedOffWall = false;
        }
    }
}
