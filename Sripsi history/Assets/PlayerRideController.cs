using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerRideController : PhysicsObject
{
    // jumping
    public float jumpTakeOffSpeed;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool stoppedJumping;
    //private bool canDoubleJump;

    // move
    private float moveSpeedStore;
    public float moveSpeed;
    public float speedMultiplier;
    public float maxSpeedAllowed;

    // increase speed
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

    // kebal
    private float invicibiltyCounter;
    public float invicibiltyLength;
    public float flashLength = 0.1f;
    private float flashCounter;

    public int playerLife = 3;
    Player player;

    public LayerMask enemyLayer;
    public GameObject respawn, finishUI;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    PlatformGameManager thePlatformGameManager;
    private float playerX;

    // ded script
    FinishGame finishGame;
    FinishRunnerMiniGame finishRunnerMiniGame;
    bool die = false;
    private float lifeCounter;

    private void Awake()
    {
        if (thePlatformGameManager == null)
            thePlatformGameManager = FindObjectOfType<PlatformGameManager>();

        player = new Player(playerLife);
        lifeText.text = player.Life + " X";

        spriteRenderer = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();

        finishGame = finishUI.GetComponent<FinishGame>();
        if (finishGame == null)
            finishRunnerMiniGame = finishUI.GetComponent<FinishRunnerMiniGame>();

        jumpTimeCounter = jumpTime;
        stoppedJumping = true;

        moveSpeedStore = moveSpeed;

        speedMilestoneCount = speedIncreaseMilestone;
        speedMilestoneCountStore = speedMilestoneCount;
        speedIncreaseMilestoneStore = speedIncreaseMilestone;
    }

    protected override void ComputeVelocity()
    {
        //=============================
        // HEART UI SHOW
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < player.MaxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
            if (i < player.Health)
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
            if (invicibiltyCounter <= 0 || player.Life <= 0)
            {
                spriteRenderer.enabled = true;
            }
        }

        //Running Horse
        Vector2 move = Vector2.zero;

        move.x = 1;
        anim.SetFloat("Move", Mathf.Abs(moveSpeed));
        if(transform.position.x > speedMilestoneCount)
        {
            speedMilestoneCount += speedIncreaseMilestone;

            speedIncreaseMilestone = speedIncreaseMilestone * speedMultiplier;
            moveSpeed *= speedMultiplier;
            if (moveSpeed > maxSpeedAllowed) moveSpeed = maxSpeedAllowed;
        }

        targetVelocity = move * moveSpeed;

        // Jumping Horse
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                velocity.y = jumpTakeOffSpeed;
                stoppedJumping = false;
            }

            // double jump
            //if(!grounded && canDoubleJump)
            //{
            //    velocity.y = jumpTakeOffSpeed;
            //    jumpTimeCounter = jumpTime;
            //    stoppedJumping = false;
            //    canDoubleJump = false;
            //}
        }

        if (CrossPlatformInputManager.GetButton("Jump") && !stoppedJumping)
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
            stoppedJumping = true;
        }

        if (grounded)
        {
            jumpTimeCounter = jumpTime;
            //canDoubleJump = true;
        }
        anim.SetBool("Grounded", grounded);
    }

    public void TakeDamage(float damage, bool fallDamage = false)
    {
        if (invicibiltyCounter <= 0 || fallDamage)
        {
            player.Health -= damage;
            if (player.Health <= 0)
            {
                player.Life -= 1;
                lifeText.text = player.Life + " X";
                if (player.Life > 0)
                {
                    Invoke("Respawn", 1);
                }
                else
                {
                    moveSpeed = 0;
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
        thePlatformGameManager.RestartEndlessRun();
        moveSpeed = 0;
        lifeCounter = 3f;
        player.Health = player.MaxHealth;
        moveSpeed = moveSpeedStore;
        speedMilestoneCount = speedMilestoneCountStore;
        speedIncreaseMilestone = speedIncreaseMilestoneStore;
        transform.position = respawn.transform.position;

        MakePlayerMove();
    }

    void Die()
    {
        die = true;
        if (finishGame != null)
            finishGame.GameOver();
        else
            finishRunnerMiniGame.GameOver();
    }

    public void MakePlayerMove()
    {
        if (moveSpeed == 0)
        {
            moveSpeed = 8;
        }
        anim.SetFloat("Move", Mathf.Abs(moveSpeed));
    }

    public void MakePlayerNotMoving()
    {
        if (moveSpeed != 0)
        {
            moveSpeed = 0;
            velocity.x = 0;
        }
        if (velocity.y != 0)
            velocity.y = 0;
        anim.SetFloat("Move", Mathf.Abs(moveSpeed));
    }

    public bool HealtUp(int healt)
    {
        if (player.Health >= player.MaxHealth)
        {
            return false;
        }
        else
        {
            player.Health += healt;
            return true;
        }
    }

    public Player givePlayerStatus()
    {
        return player;
    }
}
