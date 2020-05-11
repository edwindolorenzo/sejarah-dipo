using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerRideController : PhysicsObject
{
    public float jumpTakeOffSpeed;
    public float startSpeed;

    public float jumpTime;
    private float jumpTimeCounter;

    //MAKE HEART UI
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    Player playerRidingHorse = new Player();

    private Animator anim;
    //private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        jumpTimeCounter = jumpTime;
    }

    protected override void ComputeVelocity()
    {
        //=============================
        // HEART UI SHOW
        //for (int i = 0; i < hearts.Length; i++)
        //{
        //    if (i < playerRidingHorse.MaxHealth)
        //    {
        //        hearts[i].enabled = true;
        //    }
        //    else
        //    {
        //        hearts[i].enabled = false;
        //    }
        //    if (i < playerRidingHorse.Health)
        //    {
        //        hearts[i].sprite = fullHeart;
        //    }
        //    else
        //    {
        //        hearts[i].sprite = emptyHeart;
        //    }
        //}

        Vector2 move = Vector2.zero;

        move.x = 1;

        targetVelocity = move * startSpeed;

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
}
