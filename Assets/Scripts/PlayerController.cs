﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 1;

    public Animator anim;

    private Rigidbody2D rBody;
    private Vector2 inputVector;
    private Vector2 movement;
    private Vector2 newPos;

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);

        movement = inputVector * MoveSpeed;

        Vector2 currPos = rBody.position;
        newPos = currPos + movement * Time.fixedDeltaTime;

        SetDirection(inputVector);
        UpdateAnimation(inputVector);
        rBody.MovePosition(newPos);
    }

    private void SetDirection(Vector2 dir)
    {
        if (dir.x <= 0)
        {
            transform.localScale = new Vector2(1f, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(-1f, transform.localScale.y);
        }
    }

    private void UpdateAnimation(Vector2 dir)
    {
        if (dir.x != 0)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag.Equals("Lootable"))
        {
            LootableItem lootable = collision.gameObject.GetComponent<LootableItem>();
            Debug.Log(lootable.ItemType);
        }
    }
}