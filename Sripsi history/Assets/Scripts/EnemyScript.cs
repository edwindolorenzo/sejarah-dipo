using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : PhysicsObject
{
    //chase range
    [SerializeField] public Transform player;
    [SerializeField] float agroRange;
    [SerializeField] private float chaseRangeY = 4f;

    //range view enemy
    [SerializeField] Transform castPoint;

    //move enemy
    [SerializeField] float maxSpeed;
    [SerializeField] private float jumpTakeOffSpeed = 10;
    private bool moveLeft = true;
    private bool moveRight = true;
    private float jumpCounter;
    private float jumpCount = 2f;

    //attack area
    private CircleCollider2D attackCollider;
    private BoxCollider2D damagedArearCollider;
    [SerializeField] private AudioSource SwordSound, damagedSound;
    private float attackCounter;
    private float attackCount = 2f;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public LayerMask enemyLayers;

    //state enemy
    private bool reachPatrol = false;
    private bool facingRight = true;
    private bool died = false;
    float distToPlayer;

    //can be deleted use in if else case
    private bool isAgro = false;
    private bool isSearching = false;
    private bool isPatrol = true;

    //dead enemy
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public Transform attackPoint, startPatrol, EndPatrol, groundDetection, jumpDetection, headDetection;
    public GameObject damagedArea;

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
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
        if (jumpCounter > 0)
        {
            jumpCounter -= Time.deltaTime;
        }
        //Enemy AGRO RUSH IN DISTANCE
        //// distance to player (range chase player)
        distToPlayer = Vector2.Distance(transform.position, player.transform.position);

        canMove(true, true);

        // check front of soldier
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

        // try with switch case

        switch (soldier.state)
        {
            case Enemy.State.Attack:
                attackCounter = attackCount;
                animator.SetTrigger("Attack");
                Attack();
                break;
            case Enemy.State.Chase:
                Chase(player);
                StartCoroutine(ChaseCase());
                    break;
            case Enemy.State.Patrol:
                if (reachPatrol)
                {
                    Chase(startPatrol);
                }
                else
                {
                    Chase(EndPatrol);
                }
                if (distToPlayer < agroRange && Mathf.Abs(player.transform.position.y - transform.position.y) <= chaseRangeY)
                    soldier.state = Enemy.State.Chase;
                break;
            case Enemy.State.Dead:
                spriteRenderer.enabled = !spriteRenderer.enabled;
                break;
            default:
                break;
        }

        // IF ELSE CASE
        //if (!died)
        //{
        //    if (distToPlayer < agroRange && Mathf.Abs(player.transform.position.y - transform.position.y) <= chaseRangeY)
        //    {
        //        isAgro = true;
        //        isPatrol = false;
        //    }
        //    else
        //    {
        //        if(isAgro)
        //        {
        //            if (!isSearching)
        //            {
        //                isSearching = true;
        //                Invoke("StopChasingPlayer", 3);
        //            }
        //        }
        //        if (isPatrol)
        //        {
        //            if (reachPatrol)
        //            {
        //                Chase(startPatrol);
        //            }
        //            else
        //            {
        //                Chase(EndPatrol);
        //            }
        //        }
        //    }

        //    if (isAgro)
        //    {
        //        if (attackCounter <= 0)
        //        {
        //            //float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        //            if (distToPlayer <= 1f && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
        //            {
        //                attackCounter = attackCount;
        //                animator.SetTrigger("Attack");
        //                Attack();
        //                //nextAttackTime = Time.time + 1f / attackRate;
        //            }
        //        }
        //        Chase(player);
        //    }
        //}
        //else
        //{
        //    spriteRenderer.enabled = !spriteRenderer.enabled;
        //}
        animator.SetFloat("Move", Mathf.Abs(velocity.x));
        animator.SetBool("Grounded", grounded);
    }


    void Chase(Transform target)
    {
        if (!grounded)
        {
            canMove(true, true);
        }
        Vector2 move = Vector2.zero;
        if (transform.position.x < target.position.x && moveRight)
        {
            move.x = 1f;
        }
        if(transform.position.x > target.position.x && moveLeft)
        {
            move.x = -1f;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Walk") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
        {
            targetVelocity = move * maxSpeed;
        }

        // check jika player di atas dan ada layer platform di atas dengan jarak chaseRangeY
        RaycastHit2D hitPlatform = Physics2D.Raycast(jumpDetection.position, Vector2.up, chaseRangeY/2);
        if (hitPlatform)
        {
            RaycastHit2D headerCheck = Physics2D.Raycast(headDetection.position, Vector2.up, chaseRangeY / 2);
            if (headerCheck.collider == null)
            {
                if (player.transform.position.y - transform.position.y > 0.5f && hitPlatform.transform.gameObject.layer == LayerMask.NameToLayer("Platform") && soldier.state.Equals(Enemy.State.Chase) && jumpCounter <= 0 && grounded)
                {
                    velocity.y = jumpTakeOffSpeed;
                    jumpCounter = jumpCount;
                }
            }
        }
        if (move.x > 0.01f && !facingRight)
        {
            Flip();
        }else if (move.x < -0.01f && facingRight)
        {
            Flip();
        }        
        if (Mathf.Round(transform.position.x) == Mathf.Round(target.position.x) && soldier.state.Equals(Enemy.State.Patrol))
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

    void canMove(bool left = false, bool right = false)
    {
        moveLeft = left;
        moveRight = right;
    }

    void Attack()
    {
        //Play an attack animation (check yt MELEE COMBAT in Unity Brackeys)
        // Detect enemies in range attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //Damage them
        SwordSound.Play();
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<PlayerController>().TakeDamage(soldier.Attack);
        }
        soldier.state = Enemy.State.Chase;
    }

    IEnumerator ChaseCase()
    {
        if (attackCounter <= 0 && distToPlayer <= 1f && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
        {
            soldier.state = Enemy.State.Attack;
            yield return null;
        }
        if (!(distToPlayer < agroRange) && Mathf.Abs(player.transform.position.y - transform.position.y) <= chaseRangeY)
        {
            yield return new WaitForSeconds(3f);
            soldier.state = Enemy.State.Patrol;
            yield return null;
        }
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
                animator.SetBool("Died", true);
                damagedArearCollider.enabled = false;
                soldier.state = Enemy.State.Dead;
                Invoke("Die",2);
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
            transform.Rotate(new Vector3(0, 180, 0));
    }


    // CAN BE DELETED
    //void StopChasingPlayer()
    //{
    //    isAgro = false;
    //    isSearching = false;
    //    isPatrol = true;
    //}

    // CAN BE DELETED USE IN IF ELSE CASE
    //bool CanSeePlayer(float distance)
    //{
    //    bool val = false;
    //    float castDist = distance;

    //    Vector2 endPos = castPoint.position + Vector3.right * distance;

    //    RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Player"));

    //    if(hit.collider != null)
    //    {
    //        if (hit.collider.gameObject.CompareTag("Player"))
    //        {
    //            val = true;
    //        }
    //        else
    //        {
    //            val = false;
    //        }
    //        Debug.DrawLine(castPoint.position, endPos, Color.yellow);
    //    }
    //    else
    //    {
    //        Debug.DrawLine(castPoint.position, endPos, Color.blue);
    //    }
    //    return val;
    //}

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, agroRange);
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
