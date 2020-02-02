using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;

    public int hpModifier = 1;
    [HideInInspector] public int healthPoints = 100;
    public float range = 2f;
    public int AttackDamage = 20;

    public Rigidbody2D enemyRigidBody;
    public bool isMovable = false;
    private bool isHitByPlayer = false;
    private Transform target = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(AttackDamage);
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