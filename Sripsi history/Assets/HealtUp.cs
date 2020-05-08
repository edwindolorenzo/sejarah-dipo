using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtUp : MonoBehaviour
{
    public int healt = 1;
    public float fadeOutTime = 2f;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            bool added = collision.GetComponent<PlayerController>().HealtUp(healt);
            if (added)
                Destroy(gameObject);
                //StartCoroutine(animated());
        }
    }

    IEnumerator animated()
    {
        GetComponent<Collider2D>().enabled = false;
        Color tmpColor = GetComponent<SpriteRenderer>().color;
        while (tmpColor.a > 0f)
        {
            transform.Translate(Vector3.up * Time.deltaTime);
            tmpColor.a -= Time.deltaTime / fadeOutTime;

            if (tmpColor.a <= 0f)
                tmpColor.a = 0.0f;
            yield return null;
        }
        Destroy(gameObject);
    }
}
