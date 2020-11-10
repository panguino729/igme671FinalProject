﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float health;
    private RaycastHit2D rayHit;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool onPlatform;
    //The offset from the center of the enemy for the raycast - the raycast's origin is from the right of the enemy if the enemy is moving right, and vice versa.
    private float xOffset;
    //The layer of the platforms
    private LayerMask lm;
    //The direction in which the object moves - will be reversed
    public Vector2 moveDirection;
    public float moveMagnitude; //The speed at which the object moves
    // Start is called before the first frame update
    void Start()
    {
        if (moveDirection == new Vector2(0, 0))
        {
            moveDirection = new Vector2(1, 0);
        }
        if (moveMagnitude == 0)
        {
            moveMagnitude = 15;
        }
        lm = (1 << LayerMask.NameToLayer("Platform"));
        moveDirection = moveDirection.normalized;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (moveDirection.x > 0)
        {
            xOffset = spriteRenderer.size.x;
        }
        else
        {
            moveDirection.x = -spriteRenderer.size.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //If the enemy is colliding with a platform, check if it has reached the edge of that platform
        if (collision.gameObject.tag.Equals("platform"))
        {
            onPlatform = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        onPlatform = false;
    }
    private void Move()
    {
        if (onPlatform)
        {
            //The distance of the raycast - short so that it doesn't detect platforms below the initial one
            float distance = 1;
            //The origin of the raycast - starts from the bottom left or right of object to determine  if the object is about to reach the edge of the platform
            Vector2 origin = new Vector2(transform.position.x + xOffset, transform.position.y - spriteRenderer.size.y);

            //Raycast downward - collider of rayhit will be null if the object has reached the edge of the platform
            rayHit = Physics2D.Raycast(origin, -Vector3.up, distance, lm); 

            //If the enemy reached the edge of the platform, reverses its direction
            if (rayHit.collider == null)
            {
                moveDirection = new Vector2(moveDirection.x * -1, moveDirection.y * -1);
            }
            rb.velocity = moveDirection * moveMagnitude;
        }
    }
}
