using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtUp : MonoBehaviour
{
    public int healt = 1;
    public float fadeOutTime = 2f;
    private Animator animator;
    private AudioSource audioSource;
    private bool added = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.GetComponent<PlayerRideController>())
            {
                added = collision.GetComponent<PlayerRideController>().HealtUp(healt);
            }
            if (collision.GetComponent<PlayerController>())
            {
                added = collision.GetComponent<PlayerController>().HealtUp(healt);
            }
            if (added)
                StartCoroutine(animated());
        }
    }

    IEnumerator animated()
    {
        audioSource.Play();
        GetComponent<Collider2D>().enabled = false;
        SpriteRenderer tmpSprite = GetComponent<SpriteRenderer>();
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = tmpSprite.material.color;
            c.a = f;
            tmpSprite.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
    }
}
