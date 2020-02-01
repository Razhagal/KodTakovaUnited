using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerCompletenesState
{
    OneLeg = 0,
    TwoLegs,
    TwoLegsOneArm,
    OneLegOneArm,
    OneLegTwoArms,
    TwoLegsTwoArms // Complete player
}

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 1;

    public GameObject[] completedStateObjects;
    public Animator[] completedStateAnimators;

    private Animator anim;
    private Rigidbody2D rBody;
    private Vector2 inputVector;
    private Vector2 movement;
    private Vector2 newPos;

    private PlayerState playerState;
    private PlayerCompletenesState completenesIndex;

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        playerState = ServiceLocator.Instance.GetInstanceOfType<PlayerState>();
        completenesIndex = PlayerCompletenesState.TwoLegsTwoArms;

        for (int i = 0; i < completedStateObjects.Length; i++)
        {
            completedStateObjects[i].SetActive(false);
        }

        for (int i = 0; i < completedStateAnimators.Length; i++)
        {
            completedStateAnimators[i].enabled = false;
        }

        completedStateObjects[(int)completenesIndex].SetActive(true);
        completedStateAnimators[(int)completenesIndex].enabled = true;
        anim = completedStateAnimators[(int)completenesIndex];
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

        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    private void SetDirection(Vector2 dir)
    {
        if (dir.x < 0)
        {
            transform.localScale = new Vector2(1f, transform.localScale.y);
        }
        else if (dir.x > 0)
        {
            transform.localScale = new Vector2(-1f, transform.localScale.y);
        }
    }

    private void UpdateAnimation(Vector2 dir)
    {
        if (dir.x != 0 || dir.y != 0)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }

    private void PerformAttack()
    {
        anim.SetTrigger("Attack");
    }

    private void UpdatePlayerCompleteness(PlayerCompletenesState newCompleteness)
    {
        completedStateObjects[(int)completenesIndex].SetActive(false);
        completedStateAnimators[(int)completenesIndex].enabled = false;

        completenesIndex = newCompleteness;

        completedStateObjects[(int)completenesIndex].SetActive(true);
        completedStateAnimators[(int)completenesIndex].enabled = true;
        anim = completedStateAnimators[(int)completenesIndex];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag.Equals("Lootable"))
        {
            LootableItem lootable = collision.gameObject.GetComponent<LootableItem>();

            if (playerState != null)
            {
                if (lootable.ItemType == LootType.RobotPart)
                {
                    if (playerState.ReceivePart(lootable as RobotPartItem))
                    {
                        Destroy(collision.gameObject);
                    }
                }
                else if (lootable.ItemType == LootType.ShipPart)
                {
                    if (playerState.CarryItem(lootable as ShipPartItem))
                    {
                        Destroy(collision.gameObject);
                    }
                }
            }

            Debug.Log(lootable.ItemType);
        }
    }
}