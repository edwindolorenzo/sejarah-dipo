using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : PhysicsObject
{
    [SerializeField] public Transform player;
    [SerializeField] Transform castPoint;
    [SerializeField] float agroRange;
    [SerializeField] float maxSpeed;
    [SerializeField] private float jumpTakeOffSpeed = 10;
    [SerializeField] private float chaseRangeY = 4f;
    private bool isAgro = false;
    private bool isSearching = false;
    private bool isPatrol = true;
    private bool died = false;
    private bool reachPatrol = false;
    private bool haveGround = true;
    private CircleCollider2D attackCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float attackCounter;
    private float attackCount = 2f;
    private float jumpCounter;
    private float jumpCount = 2f;
    private bool facingRight = true;
    //private EnemyState _currentState;

    public Transform attackPoint, startPatrol, EndPatrol, groundDetection;
    public float attackRange = 0.5f;
    public LayerMask grondMask;
    public LayerMask enemyLayers;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    Enemy soldier = new Enemy();


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        if (died)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
        if (jumpCounter > 0)
        {
            jumpCounter -= Time.deltaTime;
        }
        //Enemy AGRO RUSH IN DISTANCE
        //// distance to player
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        haveGround = true;

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

        // try with switch case

        //switch (_currentState)
        //{
        //    case EnemyState.Patrol:
        //        {

        //        break;
        //        }
        //    case EnemyState.Chase:
        //        {
        //            break;
        //        }
        //    case EnemyState.Attack:
        //        {
        //            break;
        //        }
        //    case EnemyState.Die:
        //        {
        //            break;
        //        }
        //}

        //if (CanSeePlayer(agroRange))
        RaycastHit2D walkground = Physics2D.Raycast(groundDetection.position, Vector2.down, 10f);
        if(!walkground)
        {
            haveGround = false;
        }
        if (!died)
        {
            if (distToPlayer < agroRange && Mathf.Abs(player.transform.position.y - transform.position.y) <= chaseRangeY)
            {
                isAgro = true;
                isPatrol = false;
            }
            else
            {
                if(isAgro)
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
                if (attackCounter <= 0)
                {
                    //float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
                    if (distToPlayer <= 1f && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
                    {
                        attackCounter = attackCount;
                        animator.SetTrigger("Attack");
                        Attack();
                        //nextAttackTime = Time.time + 1f / attackRate;
                    }
                }
                Chase(player);
            }
        }
        animator.SetFloat("Move", Mathf.Abs(velocity.x));
        animator.SetBool("Grounded", grounded);
    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;

        Vector2 endPos = castPoint.position + Vector3.right * distance;

        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Player"));

        if(hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                val = true;
            }
            else
            {
                val = false;
            }
            Debug.DrawLine(castPoint.position, endPos, Color.yellow);
        }
        else
        {
            Debug.DrawLine(castPoint.position, endPos, Color.blue);
        }
        return val;
    }

    void Chase(Transform target)
    {
        Vector2 move = Vector2.zero;
        if (transform.position.x < target.position.x)
        {
            move.x = 1f;
        }
        else
        {
            move.x = -1f;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Walk") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
        {
            targetVelocity = move * maxSpeed;
        }

        // check jika player di atas dan ada layer platform di atas dengan jarak chaseRangeY
        RaycastHit2D hit = Physics2D.Raycast(groundDetection.position, Vector2.up, chaseRangeY);
        if (hit)
        {
            if (Mathf.Abs(player.transform.position.y - transform.position.y) <= chaseRangeY && hit.transform.gameObject.layer == LayerMask.NameToLayer("Platform") && isAgro && jumpCounter <= 0 && grounded)
            {
                velocity.y = jumpTakeOffSpeed;
                jumpCounter = jumpCount;
            }
        }
        if (move.x > 0.01f && !facingRight)
        {
            Flip();
        }else if (move.x < 0.01f && facingRight)
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
        //    transform.Rotate(new Vector3(0, 180, 0));
            //castPoint.localPosition = new Vector2(-castPoint.localPosition.x, castPoint.localPosition.y);
            //attackPoint.localPosition = new Vector2(-attackPoint.localPosition.x, attackPoint.localPosition.y);
        //}
    }

    void StopChasingPlayer()
    {
        isAgro = false;
        isSearching = false;
        isPatrol = true;
    }

    void Attack()
    {
        //Play an attack animation (check yt MELEE COMBAT in Unity Brackeys)
        // Detect enemies in range attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<PlayerController>().TakeDamage(soldier.Attack);
        }
    }

    public void TakeDamage(float damage)
    {
        soldier.Health -= damage;
        animator.SetTrigger("Hurt");
        if (soldier.Health <= 0)
        {
            died = true;
            animator.SetBool("Died", true);
            Invoke("Die",2);
        }
    }

    void Die()
    {
        Object.Destroy(this.gameObject);
    }

    void Flip()
    {
        facingRight = !facingRight;
            transform.Rotate(new Vector3(0, 180, 0));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, agroRange);
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack,
        Die
    }

}
