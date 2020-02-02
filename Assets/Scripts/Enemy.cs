﻿using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public int minusHealthPoints = 10;
    [HideInInspector]
    public int healthPoints = 100;
    //public float moveSpeed;
    //public float moveRotate;
    //private Coroutine moveEnemyCourotine;
    public float range = 2f;
    


    private Transform target = null;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            target = collision.transform;
            anim.SetTrigger("Attack");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            target = null;
            anim.SetTrigger("Idle");
        }
    }

    public void RemoveHealthPoints()
    {
        healthPoints = healthPoints - minusHealthPoints;
    }

}