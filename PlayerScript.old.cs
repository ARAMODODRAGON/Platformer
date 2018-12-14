using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // scene to load when level ends
    public string nextLevel;
    
    // camera
    GameObject cam;

    // components
    Rigidbody2D rb;
    Transform t;

    // collision check
    private bool bottom = false;
    private bool top = false;
    private bool left = false;
    private bool right = false;

    // key hold vars
    private bool isKeyHeld = false;

    // physics variables
    public float gravity;
    public float jumpDeccel;
    public float riseDeccel;
    public float maxGrav;
    public float wallSlideSpeed;
    public float walkSpeed;
    public float jumpHeight;
    public float wallJumpSpeed_;
    public float dubTimer_;

    // private physics variables
    private bool canDouble = false;
    private float dubTimer = 0.0f;
    private float wallJumpSpeed;

    // input variable
    private float axis = 0.0f;
    public float axisSpeed;
    public float axisSlip;
    public float axisWall;


    // Use this for initialization
    private void Start ()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");

        rb = GetComponent<Rigidbody2D>();
        t = GetComponent<Transform>();
	}

    // Update is called once per frame
    private void Update ()
    {
        HandleInput();
        HandleJump();
	}

    // Called on a fixed timer
    private void FixedUpdate()
    {
        HandleMovement();
        HandleGrav();
    }

    // Called after every update
    private void LateUpdate()
    {
        cam.transform.position = new Vector3(t.position.x, t.position.y, -9.0f);
    }



    // Handles input
    private void HandleInput()
    {
        float tempAxis = axis;

        bool isSet = true;
        if (Mathf.Abs(axis) <= 1.0f)
        {
            isSet = false;
        }

        /*
         * creates an axis based on whether left or right is held
         * reduces to 0 when neither or both is held
         */
        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            tempAxis -= 1.0f * (Time.deltaTime / axisSpeed);
            if (tempAxis <= -1.0f && !isSet)
            {
                tempAxis = -1.0f;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            tempAxis += 1.0f * (Time.deltaTime / axisSpeed);
            if (tempAxis >= 1.0f && !isSet)
            {
                tempAxis = 1.0f;
            }
        }
        else // only runs if neither or both up or down is held
        {
            if (tempAxis < 0.0f)
            {
                tempAxis += 1.0f * (Time.deltaTime / axisSlip);
            }
            else if (tempAxis > 0.0f)
            {
                tempAxis -= 1.0f * (Time.deltaTime / axisSlip);
            }

            if ((tempAxis <= 0.1f) && (tempAxis >= -0.1f))
            {
                tempAxis = 0.0f;
            }
        }

        axis = tempAxis;

        isKeyHeld = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);
    }

    // Handles Jumping
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (left || right)
            {
                if(left)
                {
                    wallJumpSpeed = wallJumpSpeed_;
                }
                else if (right)
                {
                    wallJumpSpeed = -wallJumpSpeed_;
                }
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                rb.MovePosition(transform.position);
            }
            else if (bottom)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
            else if (canDouble && dubTimer <= 0.0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                canDouble = false;
            }
        }
        if(left || right || bottom)
        {
            canDouble = true;
            dubTimer = dubTimer_;
        }
    }

    // Handles movement
    private void HandleMovement()
    {
        float axis_ = axis;

        if (axis < 0.0f && left)
        {
            axis_ = 0.0f;
            axis = 0.0f;
        }
        else if (axis > 0.0f && right)
        {
            axis_ = 0.0f;
            axis = 0.0f;
        }

        if (wallJumpSpeed > 0.0f)
        {
            rb.AddForce(new Vector2(wallJumpSpeed, 0.0f), ForceMode2D.Impulse);
            axis = wallJumpSpeed;
            wallJumpSpeed--;
        }
        else if (wallJumpSpeed < 0.0f)
        {
            rb.AddForce(new Vector2(wallJumpSpeed, 0.0f), ForceMode2D.Impulse);
            axis = wallJumpSpeed;
            wallJumpSpeed++;
        }
        else
        {
            rb.velocity = new Vector2(axis_ * walkSpeed, rb.velocity.y);
        }

        if (!(left || right || bottom) && dubTimer > 0.0f)
        {
            dubTimer--;
        }
    }

    // Handles gravity (FixedUpdate)
    private void HandleGrav()
    {
        Vector2 vel;
        if (rb.velocity.y <= 0.0f)
        {
            vel = new Vector2(rb.velocity.x, rb.velocity.y + -gravity * Time.fixedDeltaTime);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            vel = new Vector2(rb.velocity.x, rb.velocity.y + -jumpDeccel * Time.fixedDeltaTime);
        }
        else
        {
            vel = new Vector2(rb.velocity.x, rb.velocity.y + -riseDeccel * Time.fixedDeltaTime);
        }
        
        if (vel.y < -maxGrav)
        {
            vel.y = -maxGrav;
        }
        if (vel.y > 0.0f && top)
        {
            vel.y = 0.0f;
        }
        if (vel.y < 0.0f && bottom)
        {
            vel.y = 0.0f;
        }

        if (vel.y < 0.0f && (left || right) && isKeyHeld)
        {
            vel.y *= wallSlideSpeed;
        }

        rb.velocity = vel;
    }

    private void OnCollisionStay2D(Collision2D collision) // used to check collisions and the direction they come from
    {
        bool left_, right_, bottom_, top_;
        left_ = right_ = bottom_ = top_ = false;

        ContactPoint2D con;
        float angle;

        for (int i = 0; i < collision.contactCount; i++)
        {
            con = collision.GetContact(i);

            angle = (Mathf.Atan((con.point.y - t.position.y) / (con.point.x - t.position.x)) * Mathf.Rad2Deg);

            if ((con.point.x - t.position.x) < 0.0f)
            {
                angle += 180.0f;
            }
            if (((con.point.y - t.position.y) < 0.0f) && (con.point.x - t.position.x) > 0.0f)
            {
                angle += 360.0f;
            }

            if ((150.0f <= angle) && (angle <= 210.0f))
            {
                left_ = true;
            }
            else if ((225.0f <= angle) && (angle <= 315.0f))
            {
                bottom_ = true;
            }
            else if (((0.0f <= angle) && (angle <= 30.0f)) || ((330.0f <= angle) && (angle <= 360.0f)))
            {
                right_ = true;
            }
            else top_ |= ((45.0f <= angle) && (angle <= 135.0f));


            left = left_;
            right = right_;
            bottom = bottom_;
            top = top_;

            HandleCollisions(collision);
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        left = right = bottom = top = false;
    }

    private void HandleCollisions(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("SpikeMap"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Destroy(gameObject);
        }
        if (collision.gameObject.name.Equals("EndLevel"))
        {
            SceneManager.LoadScene(nextLevel);
            Destroy(gameObject);
        }
    }
}
