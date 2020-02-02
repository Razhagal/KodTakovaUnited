using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : Projectile
{
    private void Update()
    {
        if (isAlive)
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);

            LifeTime -= Time.deltaTime;
            if (LifeTime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public override void FireAtTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, 90) * direction;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, rotatedDirection);
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
