using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPatrol : MoveObject
{
    SpriteRenderer spriteRenderer;
    Animator animator;

    public static PlayerPatrol instance;
    // Start is called before the first frame update
    void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (instance == null)
        {
            if(scene.name == "MainMenu" || scene.name == "SelectLevel")
            instance = this;
            else
            {
                Destroy(transform.parent.gameObject);
                return;
            }
        }
        else
        {
            Destroy(transform.parent.gameObject);
            return;
        }
        if (scene.name == "MainMenu" || scene.name == "SelectLevel")
            DontDestroyOnLoad(transform.parent.gameObject);
        Debug.Log(scene.name);
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Move", 1);
    }

    protected override void ComputeAnimation()
    {
        if (Mathf.Round(transform.position.x) == Mathf.Round(endPos.position.x))
            spriteRenderer.flipX = true;
        if (Mathf.Round(transform.position.x) == Mathf.Round(startPos.position.x))
            spriteRenderer.flipX = false;
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
}
