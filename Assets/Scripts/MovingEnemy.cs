using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 2f;
    private float characterVelocity = 0.5f;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;

    void Start()
    {
        latestDirectionChangeTime = 0f;
        CalcuateNewMovementVector();
    }

    void CalcuateNewMovementVector()
    {
        //create a random direction vector with the magnitude of 1, later multiply it with the velocity of the enemy
        float x = Random.Range(-0.05f, 0.05f);
        float y = Random.Range(-0.05f, 0.05f);
        movementDirection = new Vector2(x, y).normalized;
        movementPerSecond = movementDirection * characterVelocity;
    }

    void Update()
    {
        //if the changeTime was reached, calculate a new movement vector
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        { 
            latestDirectionChangeTime = Time.time;
            CalcuateNewMovementVector();
        }

        ////move enemy: 
        transform.position = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime),
        transform.position.y + (movementPerSecond.y * Time.deltaTime));
    }
}
