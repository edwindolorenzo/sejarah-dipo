﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float invicibiltyLength;
    public float flashLength = 0.1f;
    public GameObject respawn;
    public GameObject dieUI;

    //waktu kecepatan untuk menyerang
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    private float flashCounter;
    private float invicibiltyCounter;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float lifeCounter;
    Player player = new Player();

    //MAKE HEART UI
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Image lifeUI;
    public Text lifeText;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
        targetVelocity = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || animator.GetCurrentAnimatorStateInfo(0).IsTag("Die") && grounded ? Vector2.zero : move * maxSpeed;

    }

    void Attack()
    {
        //Play an attack animation (check yt MELEE COMBAT in Unity Brackeys)
        // Detect enemies in range attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //Damage them
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
        if (invicibiltyCounter <= 0)
        {
            player.Health -= damage;
            animator.SetTrigger("Hurt");
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
        dieUI.SetActive(true);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
