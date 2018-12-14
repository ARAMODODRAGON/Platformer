using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    // components
    private Transform t;
    private Collider2D col;

    // collision variables
    private bool top;
    private bool bottom;
    private bool left;
    private bool right;
    private float vAngle;
    private bool hasCollided;

    // initialization
    private void Start ()
    {
        // sets the component
        t = GetComponent<Transform>();
        col = GetComponent<Collider2D>();

        // default values
        top = false;
        bottom = false;
        left = false;
        right = false;
        hasCollided = false;
        vAngle = 0.8f;

    }

    private void FixedUpdate()
    {
        if (hasCollided)
        {
            hasCollided = false;
        }
        else
        {
            left = right = bottom = top = false;
        }
    }

    // used to check collisions and the direction they come from
    private void OnCollisionStay2D(Collision2D collision)
    {
        bool left_, right_, bottom_, top_;
        left_ = right_ = bottom_ = top_ = false;

        ContactPoint2D con;
        Vector2 v = Vector2.zero;

        for (int i = 0; i < collision.contactCount; i++)
        {
            con = collision.GetContact(i);

            v = (con.point - new Vector2(t.position.x, t.position.y)).normalized;
            
            if (v.x < -vAngle)
            {
                left_ = true;
            }

            if (v.x > vAngle)
            {
                right_ = true;
            }

            if (v.y < -vAngle)
            {
                bottom_ = true;
            }

            if (v.y > vAngle)
            {
                top_ = true;
            }

        }

        //Debug.Log(v);
        left = left_;
        right = right_;
        bottom = bottom_;
        top = top_;

        hasCollided = true;
    }
    

    // used for returning the current state of each sides collision
    public bool Top() { return top; }
    public bool Bottom() { return bottom; }
    public bool Left() { return left; }
    public bool Right() { return right; }
}
