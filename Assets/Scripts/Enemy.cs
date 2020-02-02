using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public int hpModifier = 1;
    [HideInInspector]
    public int healthPoints = 100;
    public Rigidbody2D enemyRigidBody;
    public bool isMovable = false;
    public float range = 2f;
    private bool isHitByPlayer = false;
    private Transform target = null;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            target = collision.transform;
            anim.SetTrigger("Attack");
            //RemoveHealthPoints(0.5f);
            if (isMovable && enemyRigidBody != null)
            {
                enemyRigidBody.isKinematic = true;
                enemyRigidBody.velocity = Vector3.zero;
                isHitByPlayer = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            target = null;
            anim.SetTrigger("Idle");

            if (isMovable && enemyRigidBody != null)
            {
                if (enemyRigidBody.isKinematic == true)
                {
                    enemyRigidBody.isKinematic = false;
                }
            }
        }
    }

    public void RemoveHealthPoints(float playerDamage)
    {
        if (healthPoints > 0)
        {
            healthPoints = healthPoints - (int)(hpModifier * playerDamage);
        }

        //Debug.Log(healthPoints);

        if (healthPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}