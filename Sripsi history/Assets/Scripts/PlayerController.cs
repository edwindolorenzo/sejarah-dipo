using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : PhysicsObject
{
    //movement
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    private bool moveLeft = true;
    private bool moveRight = true;

    //attack range
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    //time invicible
    public float invicibiltyLength;
    private float invicibiltyCounter;
    public float flashLength = 0.1f;
    private float flashCounter;
    private SpriteRenderer spriteRenderer;

    public GameObject respawn, finishUI;

    //script if dead
    FinishGame finishGame;
    FinishMiniGame finishMiniGame;
    bool die = false;
    private float lifeCounter;

    //waktu kecepatan untuk menyerang
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    [SerializeField] private AudioSource stepSound, damagedSound, damageSound;
    private Animator animator;

    //validate player life
    public int playerLife = 3;
    Player player;

    //MAKE HEART UI
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Image lifeUI;
    public Text lifeText;


    // Start is called before the first frame update
    void Awake()
    {
        player = new Player(playerLife);
        lifeText.text = player.Life+" X";
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        finishGame = finishUI.GetComponent<FinishGame>();
        if (finishGame == null)
            finishMiniGame = finishUI.GetComponent<FinishMiniGame>();
    }
    protected override void ComputeVelocity()
    {
        //=============================
        // HEART UI SHOW
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < player.MaxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
            if(i < player.Health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
        //if (lifeCounter > 0)
        //{
        //    lifeCounter -= Time.deltaTime;
        //}
        //else
        //{
        //    for (float i = 1; i >= 0; i -= Time.deltaTime)
        //    {
        //        lifeUI.color = new Color(1, 1, 1, i);
        //        lifeText.color = new Color(50, 50, 50, i);
        //    }
        //}
        // Invicible after take damage
        if (invicibiltyCounter > 0)
        {
            invicibiltyCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                flashCounter = flashLength;
            }
            if(invicibiltyCounter <= 0 || player.Life <= 0)
            {
                spriteRenderer.enabled = true;
            }
        }
        Vector2 move = Vector2.zero;
        move.x = CrossPlatformInputManager.GetAxis("Horizontal");
        //Attack enemies
        if (Time.time >= nextAttackTime)
        {
            if (CrossPlatformInputManager.GetButtonDown("Fire1") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Die"))
            {
                animator.SetTrigger("Attack");
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }


        if (CrossPlatformInputManager.GetButtonDown("Jump") && grounded && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Die"))
        {
            velocity.y = jumpTakeOffSpeed;
        }
        //TIDAK DI PAKAI DI SINI
        //else if (CrossPlatformInputManager.GetButtonUp("Jump"))
        //{
        //    if (velocity.y > 0)
        //        velocity.y = velocity.y * .5f;
        //}

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            attackPoint.localPosition = new Vector2(-attackPoint.localPosition.x, attackPoint.localPosition.y);
        }

        animator.SetBool("Grounded", grounded);
        animator.SetFloat("Move", Mathf.Abs(velocity.x));
        if (!moveLeft)
            if (move.x <= -0.1)
                move.x = 0;
        if (!moveRight)
            if (move.x >= -0.1)
                move.x = 0;
        targetVelocity = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Die") && grounded ? Vector2.zero : move * maxSpeed;
        if (Mathf.Abs(velocity.x) > 0.1 && grounded && stepSound.isPlaying == false)
            stepSound.Play();
    }

    void Attack()
    {
        //Play an attack animation (check yt MELEE COMBAT in Unity Brackeys)
        // Detect enemies in range attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //Damage them
        damageSound.Play();
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<EnemyScript>())
            enemy.GetComponent<EnemyScript>().TakeDamage(player.Attack);
            if (enemy.GetComponent<EnemyGunSoldier>())
            enemy.GetComponent<EnemyGunSoldier>().TakeDamage(player.Attack);
        }
    }

    public void TakeDamage(float damage)
    {
        if (invicibiltyCounter <= 0 && !die)
        {
            player.Health -= damage;
            animator.SetTrigger("Hurt");
            damagedSound.Play();
            // membuat kena damage mundur belum bisa
            //if (spriteRenderer.flipX)
            //{
            //    transform.position += Vector3.left;
            //}
            //else
            //{
            //    transform.position += Vector3.right;
            //}
            if (player.Health <= 0)
            {
                animator.SetTrigger("Die");
                animator.SetBool("Died", true);
                player.Life -= 1;
                lifeText.text = player.Life+" X";
                if (player.Life > 0)
                {
                    //lifeText.text = player.Life + " x";
                    //for (float i = 0; i >= 1; i += Time.deltaTime)
                    //{
                    //    lifeUI.color = new Color(1, 1, 1, i);
                    //    lifeText.color = new Color(0, 0, 0, i);
                    //}
                    Invoke("Respawn", 1);
                }
                else
                {
                    Invoke("Die", 1);
                }
            }
            invicibiltyCounter = invicibiltyLength;

            spriteRenderer.enabled = false;

            flashCounter = flashLength;
        }
    }

    void Respawn()
    {
        animator.SetBool("Died", false);
        lifeCounter = 3f;
        player.Health = player.MaxHealth;
        transform.position = respawn.transform.position;
    }

    void Die()
    {
        die = true;
        if (finishGame != null)
            finishGame.GameOver();
        else
            finishMiniGame.GameOver();
    }

    public Player givePlayerStatus()
    {
        return player;
    }

    public bool HealtUp(int healt)
    {
        if(player.Health >= player.MaxHealth)
        {
            return false;
        }
        else
        {
            player.Health += healt;
            return true;
        }
    }

    public void Moveable(bool left = true, bool right = true)
    {
        moveLeft = left;
        moveRight = right;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
