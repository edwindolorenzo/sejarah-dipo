using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKeris : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
                StartCoroutine(animated());
        }
    }

    IEnumerator animated()
    {
        GetComponent<Collider2D>().enabled = false;
        SpriteRenderer tmpSprite = GetComponent<SpriteRenderer>();
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = tmpSprite.material.color;
            c.a = f;
            tmpSprite.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        Object.Destroy(this.gameObject);
    }
}
