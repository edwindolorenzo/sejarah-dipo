using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerController playerController = hitInfo.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(1);
        }
        if(hitInfo.gameObject.layer == LayerMask.NameToLayer("Player") || hitInfo.gameObject.layer == LayerMask.NameToLayer("Platform"))
            Destroy(gameObject);
    }

}
