using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleProjectile : Projectile
{
    private Vector3 direction;

    private void Update()
    {
        if (isAlive)
        {
            transform.Translate(direction * Speed * Time.deltaTime);

            LifeTime -= Time.deltaTime;
            if (LifeTime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public override void FireAtTarget(Transform target)
    {
        direction = target.position - transform.position;
        isAlive = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("waaa");

            isAlive = false;
            Destroy(gameObject);
            // Player is hitted - apply damage done
        }
    }
}
