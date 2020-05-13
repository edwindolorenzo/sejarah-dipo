using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValidator : MonoBehaviour
{
    //validator check player edge camera
    public bool moveLeft, moveRight;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.GetComponent<PlayerController>().Moveable(moveLeft,moveRight);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.GetComponent<PlayerController>().Moveable();
        }
    }

}
