using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerRideController : PhysicsObject
{
    public float jumpTakeOffSpeed;

    private float moveSpeedStore;
    public float moveSpeed;
    public float speedMultiplier;
    public float maxSpeedAllowed;

    public float speedIncreaseMilestone;
    private float speedIncreaseMilestoneStore;

    private float speedMilestoneCount;
    private float speedMilestoneCountStore;

    public float jumpTime;
    private float jumpTimeCounter;

    //MAKE HEART UI
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Text lifeText;
    public Image lifeUI;
    private float lifeCounter;

    private float invicibiltyCounter;
    public float invicibiltyLength;
    public float flashLength = 0.1f;
    private float flashCounter;

    Player playerRidingHorse = new Player();

    public LayerMask enemyLayer;
    public GameObject respawn, finishUI;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public GameManager theGameManager;

    FinishGame finishGame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        finishGame = finishUI.GetComponent<FinishGame>();
        jumpTimeCounter = jumpTime;
        speedMilestoneCount = speedIncreaseMilestone;
        moveSpeedStore = moveSpeed;
        speedMilestoneCountStore = speedMilestoneCount;
        speedIncreaseMilestoneStore = speedIncreaseMilestone;
    }

    protected override void ComputeVelocity()
    {
        //=============================
        // HEART UI SHOW
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerRidingHorse.MaxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
            if (i < playerRidingHorse.Health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }

        // Blinking Effect
        if (invicibiltyCounter > 0)
        {
            invicibiltyCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                flashCounter = flashLength;
            }
            if (invicibiltyCounter <= 0 || playerRidingHorse.Life <= 0)
            {
                spriteRenderer.enabled = true;
            }
        }

        //Running Horse
        Vector2 move = Vector2.zero;

        move.x = 1;

        if(transform.position.x > speedMilestoneCount)
        {
            speedMilestoneCount += speedIncreaseMilestone;

            speedIncreaseMilestone = speedIncreaseMilestone * speedMultiplier;
            moveSpeed *= speedMultiplier;
            if (moveSpeed > maxSpeedAllowed) moveSpeed = maxSpeedAllowed;
        }

        targetVelocity = move * moveSpeed;

        // Jumping Horse
        if (CrossPlatformInputManager.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }

        if (CrossPlatformInputManager.GetButton("Jump"))
        {
            if(jumpTimeCounter > 0)
            {
                velocity.y = jumpTakeOffSpeed;
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (CrossPlatformInputManager.GetButtonUp("Jump"))
        {
            jumpTimeCounter = 0;
        }

        if (grounded)
        {
            jumpTimeCounter = jumpTime;
        }
        anim.SetBool("Grounded", grounded);
    }

    public void TakeDamage(float damage)
    {
        if (invicibiltyCounter <= 0)
        {
            playerRidingHorse.Health -= damage;
            finishGame.PlayerDamaged();
            //anim.SetTrigger("Hurt");
            //damagedSound.Play();
            if (playerRidingHorse.Health <= 0)
            {
                //anim.SetTrigger("Die");
                //anim.SetBool("Died", true);
                playerRidingHorse.Life -= 1;
                lifeText.text = playerRidingHorse.Life + " X";
                if (playerRidingHorse.Life > 0)
                {
                    //lifeText.text = playerRidingHorse.Life + " x";
                    //for (float i = 0; i >= 1; i += Time.deltaTime)
                    //{
                    //    lifeUI.color = new Color(1, 1, 1, i);
                    //    lifeText.color = new Color(0, 0, 0, i);
                    //}
                    Invoke("Respawn", 2);
                }
                else
                {
                    Invoke("Die", 2);
                }
            }
            invicibiltyCounter = invicibiltyLength;

            spriteRenderer.enabled = false;

            flashCounter = flashLength;
        }
    }

    void Respawn()
    {
        //anim.SetBool("Died", false);
        theGameManager.RestartEndlessRun();
        lifeCounter = 3f;
        playerRidingHorse.Health = playerRidingHorse.MaxHealth;
        moveSpeed = moveSpeedStore;
        speedMilestoneCount = speedMilestoneCountStore;
        speedIncreaseMilestone = speedIncreaseMilestoneStore;
        transform.position = respawn.transform.position;
    }

    void Die()
    {
        finishGame.GameOver();
    }
}
