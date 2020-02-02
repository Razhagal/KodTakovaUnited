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
    public float AttackRange = 1f;
    public float AttackCooldown = 0.7f;
    public float AttackDamage = 40f;

    public GameObject attackFx;
    public GameObject[] completedStateObjects;
    public Animator[] completedStateAnimators;

    private Animator anim;
    private Rigidbody2D rBody;
    private Vector2 inputVector;
    private Vector2 movement;
    private Vector2 newPos;

    private PlayerState playerState;
    private PlayerCompletenesState completenesIndex;

    private Vector2 lastDirection = Vector2.left;
    private bool canAttack = false;
    private float attackAvailableAfter = 0f;

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        playerState = ServiceLocator.Instance.GetInstanceOfType<PlayerState>();
        completenesIndex = PlayerCompletenesState.TwoLegsTwoArms;
        canAttack = true;

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

        UpdateAnimation();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack && attackAvailableAfter <= 0f)
        {
            PerformAttack();
        }

        if (attackAvailableAfter > 0f)
        {
            attackAvailableAfter -= Time.fixedDeltaTime;
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);

        if (inputVector.x != 0 || inputVector.y != 0)
        {
            lastDirection = inputVector;
        }

        movement = inputVector * MoveSpeed;

        Vector2 currPos = rBody.position;
        newPos = currPos + movement * Time.fixedDeltaTime;

        SetDirection(inputVector);
        UpdateAnimation(inputVector);
        rBody.MovePosition(newPos);
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
        attackAvailableAfter = AttackCooldown;

        StartCoroutine(InstantiateAttackFxObject(0.2f));

        Ray2D ray = new Ray2D(transform.position, lastDirection);
        //Debug.DrawRay(ray.origin, ray.direction * AttackRange, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, ray.direction * AttackRange, distance: AttackRange);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag.Equals("Enemy"))
            {
                hit.collider.gameObject.GetComponent<Enemy>().RemoveHealthPoints(AttackDamage);
            }
        }
    }

    private IEnumerator InstantiateAttackFxObject(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Vector3 spawnPos = transform.position + new Vector3(lastDirection.x, lastDirection.y, 0f);
        Vector3 direction = spawnPos - transform.position;
        GameObject.Instantiate(attackFx, spawnPos, Quaternion.LookRotation(Vector3.forward, direction)).SetActive(true);
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
                        UpdateAnimation();
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

        if (collision.gameObject.tag.Equals("Ship"))
        {
            if (playerState != null)
            {
                if (playerState.carriedItem.Value != null)
                {
                    if (ServiceLocator.Instance.GetInstanceOfType<ShipState>().ReceiveItem(playerState.carriedItem.Value))
                    {
                        playerState.carriedItem.Value = null;
                    }
                }
            }
        }
    }

    private void UpdateAnimation()
    {
        if (completenesIndex != PlayerCompletenesState.TwoLegsTwoArms)
        {
            if (playerState.parts[RobotPartType.LeftHand] &&
                playerState.parts[RobotPartType.RightHand] &&
                playerState.parts[RobotPartType.LeftLeg] &&
                playerState.parts[RobotPartType.RightLeg])
            {
                UpdatePlayerCompleteness(PlayerCompletenesState.TwoLegsTwoArms);
                canAttack = true;
            }
            else if ((playerState.parts[RobotPartType.LeftHand] ||
                playerState.parts[RobotPartType.RightHand]) &&
                playerState.parts[RobotPartType.LeftLeg] &&
                playerState.parts[RobotPartType.RightLeg])
            {
                UpdatePlayerCompleteness(PlayerCompletenesState.TwoLegsOneArm);
                canAttack = true;
            }
            else if (playerState.parts[RobotPartType.LeftHand] &&
                    playerState.parts[RobotPartType.RightHand] &&
                    (playerState.parts[RobotPartType.LeftLeg] ||
                    playerState.parts[RobotPartType.RightLeg]))
            {
                UpdatePlayerCompleteness(PlayerCompletenesState.OneLegTwoArms);
                canAttack = true;
            }
            else if ((playerState.parts[RobotPartType.LeftHand] &&
                    playerState.parts[RobotPartType.RightLeg]) ||
                    (playerState.parts[RobotPartType.RightHand] &&
                    playerState.parts[RobotPartType.LeftLeg]) ||
                    (playerState.parts[RobotPartType.LeftHand] &&
                    playerState.parts[RobotPartType.LeftLeg]) ||
                    (playerState.parts[RobotPartType.RightHand] &&
                    playerState.parts[RobotPartType.RightLeg]))
            {
                UpdatePlayerCompleteness(PlayerCompletenesState.OneLegOneArm);
                canAttack = true;
            }
            else if (playerState.parts[RobotPartType.RightLeg] &&
                     playerState.parts[RobotPartType.LeftLeg])
            {
                UpdatePlayerCompleteness(PlayerCompletenesState.TwoLegs);
                canAttack = false;
            }
            else if (playerState.parts[RobotPartType.RightLeg] || playerState.parts[RobotPartType.LeftLeg])
            {
                UpdatePlayerCompleteness(PlayerCompletenesState.OneLeg);
                canAttack = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        playerState.hp.Value -= damage;
    }

    /*public enum PlayerCompletenesState
{
    OneLeg = 0,
    TwoLegs,
    TwoLegsOneArm,
    OneLegOneArm,
    OneLegTwoArms,
    TwoLegsTwoArms // Complete player
}*/
}
 