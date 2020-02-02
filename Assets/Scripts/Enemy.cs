using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public int minusHealthPoints = 10;
    [HideInInspector]
    public int healthPoints = 100;
    public Rigidbody2D enemyRigidBody;
    public bool isMovable = false;
    //public float moveSpeed;
    //public float moveRotate;
    //private Coroutine moveEnemyCourotine;
    public float range = 2f;
    private bool isHitByPlayer = false;
    private Transform target = null;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            target = collision.transform;
            anim.SetTrigger("Attack");

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

    public void RemoveHealthPoints()
    {
        healthPoints = healthPoints - minusHealthPoints;
    }

}