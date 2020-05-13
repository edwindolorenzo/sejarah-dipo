using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Transform bulletPoint;

    // Update is called once per frame
    void Update()
    {
        // USE WHEN SHOOTING
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Shoot();
        //}
        // USE IF FLIP GO LEFT OR RIGHT (CHECK IN ENEMYSCRIPT OR PLAYER CONTROLLER)
        //bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        //if (flipSprite)
        //{
        //    bulletPoint.localPosition = new Vector2(-bulletPoint.localPosition.x, bulletPoint.localPosition.y);
        //    bulletPoint.Rotate(0f, 180f, 0f);
        //}

    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
