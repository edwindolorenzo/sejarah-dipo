
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunSoldier : PhysicsObject
{
    //chase range
    public Transform player;
    public float agroRange;
    [SerializeField] private float chaseRangeY = 4f;
    private float distToPlayer;

    //range view enemy
    public Transform castPoint;
    public float attackRange;
    private float xAttack = 1;

    //move enemy
    public float maxSpeed;
    [SerializeField] private float jumpTakeOffSpeed = 10;
    private bool moveLeft = true;
    private bool moveRight = true;
    private float jumpCounter;
    private float jumpCount = 2f;

    public Transform firePoint, startPatrol, EndPatrol, groundDetection, jumpDetection, headDetection;
    public GameObject bulletPrefab, damagedArea;
    private BoxCollider2D damagedArearCollider;

    // attack point
    public Vector3[] firePosition;
    public Transform bulletPoint;
    [SerializeField] private AudioSource gunShotSound, damagedSound;
    private float AttackRotation = 0;

    //attack count
    public float attackLength;
    private float attackCounter;

    //state enemy
    private bool reachPatrol = false;
    private bool facingRight = true;
    private bool died = false;

    // dead enemy
    private SpriteRenderer spriteRenderer;

    private Animator animator;

    [SerializeField] int health = 3;
    Enemy soldier;

    void Awake()
    {
        soldier = new Enemy(health);
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        damagedArearCollider = damagedArea.GetComponent<BoxCollider2D>();
    }

    protected override void ComputeVelocity()
    {
        // check attack counter
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
        if (jumpCounter > 0)
        {
            jumpCounter -= Time.deltaTime;
        }

        // check if front edge
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

        distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        switch (soldier.State)
        {
            case Enemy.StateEnemy.Patrol:
                if (reachPatrol)
                {
                    Chase(startPatrol);
                }
                else
                {
                    Chase(EndPatrol);
                }
                if (distToPlayer <= agroRange && Mathf.Abs(player.transform.position.y - transform.position.y) <= chaseRangeY)
                    soldier.State = Enemy.StateEnemy.Chase;
                break;
            case Enemy.StateEnemy.Chase:
                Chase(player);
                StartCoroutine(ChaseCase());
                break;
            case Enemy.StateEnemy.Attack:
                if(attackCounter <= 0)
                {
                    StartCoroutine(Attacking());
                }
                break;
            case Enemy.StateEnemy.Dead:
                spriteRenderer.enabled = !spriteRenderer.enabled;
                break;
            default:
                break;
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

        int mask1 = 1 << LayerMask.NameToLayer("Player");
        int mask2 = 1 << LayerMask.NameToLayer("Platform");
        int combinedMask = mask1 | mask2;

        RaycastHit2D hitStraight = Physics2D.Linecast(castPoint.position, endPosStraight, combinedMask);
        RaycastHit2D hitUp = Physics2D.Linecast(castPoint.position, endPosUp, combinedMask);
        RaycastHit2D hitDown = Physics2D.Linecast(castPoint.position, endPosDown, combinedMask);

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

        // check if enemy can jump
        RaycastHit2D hitPlatform = Physics2D.Raycast(jumpDetection.position, Vector2.up, chaseRangeY/2);
        if (hitPlatform)
        {
            RaycastHit2D headerCheck = Physics2D.Raycast(headDetection.position, Vector2.up, chaseRangeY / 2);
            if(headerCheck.collider == null)
            {
                if (player.transform.position.y - transform.position.y > 0.5f && hitPlatform.transform.gameObject.layer == LayerMask.NameToLayer("Platform") && soldier.State == Enemy.StateEnemy.Chase && jumpCounter <= 0 && grounded)
                {
                    velocity.y = jumpTakeOffSpeed;
                    jumpCounter = jumpCount;
                }
            }
        }

        // check face left or right
        if (move.x > 0.01f && !facingRight)
        {
            Flip();
        }
        else if (move.x < -0.01f && facingRight)
        {
            Flip();
        }

        // if patroling
        if (Mathf.Round(transform.position.x) == Mathf.Round(target.position.x) && soldier.State == Enemy.StateEnemy.Patrol)
        {
            reachPatrol = !reachPatrol;
        }

    }

    void canMove(bool left = false, bool right = false)
    {
        moveLeft = left;
        moveRight = right;
    }

    public void TakeDamage(float damage)
    {
        if (!died)
        {
            soldier.Health -= damage;
            animator.SetTrigger("Hurt");
            damagedSound.Play();
            if (soldier.Health <= 0)
            {
                died = true;
                soldier.State = Enemy.StateEnemy.Dead;
                animator.SetBool("Died", true);
                damagedArearCollider.enabled = false;
                Invoke("Die", 2);
            }
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
    }

    IEnumerator Attacking()
    {
        attackCounter = attackLength;
        yield return new WaitForSeconds(1f);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Die"))
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            gunShotSound.Play();
        }
        firePoint.transform.rotation = Quaternion.Euler(new Vector3(0, AttackRotation, 0));
        firePoint.localPosition = firePosition[0];
        animator.SetBool("ShootUp", false);
        animator.SetBool("ShootMid", false);
        animator.SetBool("ShootDown", false);
        soldier.State = Enemy.StateEnemy.Chase;
        yield return null;
    }

    IEnumerator ChaseCase()
    {
        if (AttackRange(attackRange))
        {
            soldier.State = Enemy.StateEnemy.Attack;
            yield return null;
        }
        if (!(distToPlayer <= agroRange) && !(Mathf.Abs(player.transform.position.y - transform.position.y) <= chaseRangeY))
        {
            yield return new WaitForSeconds(3f);
            soldier.State = Enemy.StateEnemy.Patrol;
            yield return null;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, agroRange);
    }
}
