using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerRideController : PhysicsObject
{
    public float jumpTakeOffSpeed;
    public float jumpTime;
    private float jumpTimeCounter;

    private float moveSpeedStore;
    public float moveSpeed;
    public float speedMultiplier;
    public float maxSpeedAllowed;

    public float speedIncreaseMilestone;
    private float speedIncreaseMilestoneStore;
    private float speedMilestoneCount;
    private float speedMilestoneCountStore;

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
            if (playerRidingHorse.Health <= 0)
            {
                playerRidingHorse.Life -= 1;
                lifeText.text = playerRidingHorse.Life + " X";
                if (playerRidingHorse.Life > 0)
                {
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
        theGameManager.RestartEndlessRun();
        moveSpeed = 0;
        StartCoroutine(WaitFor(3));
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

    public bool HealtUp(int healt)
    {
        if (playerRidingHorse.Health >= playerRidingHorse.MaxHealth)
        {
            return false;
        }
        else
        {
            playerRidingHorse.Health += healt;
            return true;
        }
    }

    public IEnumerator WaitFor (float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
    }
}
