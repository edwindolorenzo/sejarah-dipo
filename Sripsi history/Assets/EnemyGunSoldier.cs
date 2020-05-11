
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunSoldier : PhysicsObject
{
    public Transform player;
    public Transform castPoint;
    public float agroRange;
    public float attackRange;
    public float maxSpeed;
    public Transform firePoint, startPatrol, EndPatrol, groundDetection;
    public GameObject bulletPrefab, damagedArea;
    public Transform bulletPoint;
    public float attackLength;
    public Vector3[] firePosition;


    [SerializeField] private float jumpTakeOffSpeed = 10;
    [SerializeField] private float chaseRangeY = 4f;
    [SerializeField] private AudioSource gunShotSound, damagedSound;
    private float distToPlayer;
    private float attackCounter;
    private float xAttack = 1;
    private float AttackRotation = 0;
    private bool moveLeft = true;
    private bool moveRight = true;
    private bool isAgro = false;
    private bool isSearching = false;
    private bool isAttacking = false;
    private bool died = false;
    private bool facingRight = true;
    private bool isPatrol = true;
    private bool reachPatrol = false;
    private float jumpCounter;
    private float jumpCount = 2f;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D damagedArearCollider;

    Enemy soldier = new Enemy();

    void Awake()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        damagedArearCollider = damagedArea.GetComponent<BoxCollider2D>();
    }

    protected override void ComputeVelocity()
    {
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
            //Enemy AGRO RUSH IN DISTANCE
            //// distance to player
            distToPlayer = Vector2.Distance(transform.position, player.transform.position);

        //if(distToPlayer < agroRange)
        //{
        //    //chase player
        //    ChasePlayer();
        //}
        //else
        //{
        //    StopChasingPlayer();
        //}

        //FOR ATTACKING PLAYER

        canMove(true, true);
        RaycastHit2D hit = Physics2D.Raycast(groundDetection.position, Vector2.down, chaseRangeY);
        if (hit.collider == null)
        {
            if (Mathf.Round(transform.rotation.y) < 0)
            {
                canMove(false, true);
            }
            if (Mathf.Round(transform.rotation.y) >= 0)
            {
                canMove(true, false);
            }
        }


        if (!died)
        {

            if (isAttacking)
            {
                if(attackCounter <= 0)
                {
                    attackCounter = attackLength;
                    Invoke("Attack",1);
                }
            }
            else {
                isAttacking = AttackRange(attackRange);
                if (distToPlayer <= agroRange && Mathf.Abs(player.transform.position.y - transform.position.y) <= chaseRangeY)
                {
                    isAgro = true;
                    isPatrol = false;
                }
                else
                {
                    if (isAgro)
                    {
                        if (!isSearching)
                        {
                            isSearching = true;
                            Invoke("StopChasingPlayer", 3);
                        }
                    }
                    if (isPatrol)
                    {
                        if (reachPatrol)
                        {
                            Chase(startPatrol);
                        }
                        else
                        {
                            Chase(EndPatrol);
                        }
                    }
                }

                if (isAgro)
                {

                    //if (Time.time >= nextAttackTime)
                    //{
                    //    float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
                    //    if (distToPlayer <= 1.5f)
                    //    {
                    //        //animator.SetTrigger("Attack");
                    //        Attack();
                    //    }
                    //}
                    Chase(player);
                }
            }
        }
        else
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
        animator.SetFloat("Move", Mathf.Abs(velocity.x));
        animator.SetBool("Grounded", grounded);
    }

    bool AttackRange(float distance)
    {
        bool val = false;
        float castDist = distance;

        Vector2 endPosStraight = castPoint.position + new Vector3(xAttack,0,0) * distance;
        Vector2 endPosUp = castPoint.position + new Vector3(xAttack, 1,0) * distance;
        Vector2 endPosDown = castPoint.position + new Vector3(xAttack, -1,0) * distance;


        RaycastHit2D hitStraight = Physics2D.Linecast(castPoint.position, endPosStraight, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hitUp = Physics2D.Linecast(castPoint.position, endPosUp, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hitDown = Physics2D.Linecast(castPoint.position, endPosDown, 1 << LayerMask.NameToLayer("Player"));

        if(hitStraight.collider != null)
        {
            if (hitStraight.collider.gameObject.CompareTag("Player"))
            {
                firePoint.transform.rotation = Quaternion.Euler(new Vector3(0, AttackRotation, 0));
                val = true;
                animator.SetBool("ShootMid", true);
            }
            else
            {
                val = false;
            }
            Debug.DrawLine(castPoint.position, endPosStraight, Color.yellow);
        }
        else if (hitUp.collider != null)
        {
            if (hitUp.collider.gameObject.CompareTag("Player"))
            {
                firePoint.transform.rotation = Quaternion.Euler(new Vector3(0, AttackRotation, 45));
                firePoint.localPosition = firePosition[1];
                val = true;
                animator.SetBool("ShootUp", true);
            }
            else
            {
                val = false;
            }
            Debug.DrawLine(castPoint.position, endPosUp, Color.yellow);
        }
        else if (hitDown.collider != null)
        {
            if (hitDown.collider.gameObject.CompareTag("Player"))
            {
                firePoint.transform.rotation = Quaternion.Euler(new Vector3(0, AttackRotation, - 45));
                firePoint.localPosition = firePosition[2];
                animator.SetBool("ShootDown", true);
                val = true;
            }
            else
            {
                val = false;
            }
            Debug.DrawLine(castPoint.position, endPosDown, Color.yellow);
        }
        else
        {
            Debug.DrawLine(castPoint.position, endPosStraight, Color.blue);
            Debug.DrawLine(castPoint.position, endPosUp, Color.blue);
            Debug.DrawLine(castPoint.position, endPosDown, Color.blue);
        }

        return val;
    }

    void Chase(Transform target)
    {
        Vector2 move = Vector2.zero;
        if (!grounded)
        {
            canMove(true, true);
        }
        if (transform.position.x < target.position.x && moveRight)
        {
            move.x = 1f;
        }
        if (transform.position.x > target.position.x && moveLeft)
        {
            move.x = -1f;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Walk") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
        {
            targetVelocity = move * maxSpeed;
        }
        RaycastHit2D hit = Physics2D.Raycast(groundDetection.position, Vector2.up, chaseRangeY);
        if (hit)
        {
            if (player.transform.position.y - transform.position.y > 0.5f && hit.transform.gameObject.layer == LayerMask.NameToLayer("Platform") && isAgro && jumpCounter <= 0 && grounded)
            {
                velocity.y = jumpTakeOffSpeed;
                jumpCounter = jumpCount;
            }
        }
        if (move.x > 0.01f && !facingRight)
        {
            Flip();
        }
        else if (move.x < -0.01f && facingRight)
        {
            Flip();
        }
        if (Mathf.Round(transform.position.x) == Mathf.Round(target.position.x) && isPatrol)
        {
            reachPatrol = !reachPatrol;
        }

        //bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        //if (flipSprite)
        //{
        //    spriteRenderer.flipX = !spriteRenderer.flipX;
        //}
    }

    void canMove(bool left = false, bool right = false)
    {
        moveLeft = left;
        moveRight = right;
    }

    void StopChasingPlayer()
    {
        isAgro = false;
        isSearching = false;
        isPatrol = true;
    }

    void Attack()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Die"))
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            gunShotSound.Play();
        }
        firePoint.transform.rotation = Quaternion.Euler(new Vector3(0, AttackRotation, 0));
        firePoint.localPosition = firePosition[0];
        animator.SetBool("ShootUp", false);
        animator.SetBool("ShootMid", false);
        animator.SetBool("ShootDown", false);
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        soldier.Health -= damage;
        animator.SetTrigger("Hurt");
        damagedSound.Play();
        if (soldier.Health <= 0)
        {
            died = true;
            animator.SetBool("Died", true);
            damagedArearCollider.enabled = false;
            Invoke("Die", 2);
        }
    }

    void Die()
    {
        Object.Destroy(this.gameObject);
    }

    void Flip()
    {
        facingRight = !facingRight;
        AttackRotation = facingRight ? 0 : 180;
        transform.Rotate(new Vector3(0, 180, 0));
        castPoint.localPosition = new Vector2(-castPoint.localPosition.x, castPoint.localPosition.y);
        xAttack = -xAttack;
        //firePoint.localPosition = new Vector2(-firePoint.localPosition.x, firePoint.localPosition.y);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, agroRange);
    }
}
