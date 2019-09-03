using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float fireRate = 0;
    public float Damage = 10;
    public LayerMask whatToHit;

    float timeToFire = 0;
    Transform firePoint;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firepoint!!!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                shoot();
            }
        }
    }

    void shoot()
    {
        Vector2 dir = new Vector2(firePoint.position.x + 10, firePoint.position.y);
        Vector2 socketPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        if (transform.localScale.x < 0f)
        {
            dir.x = -dir.x;
        }

        RaycastHit2D hit = Physics2D.Raycast(socketPosition, dir - socketPosition, 100, whatToHit);
        Debug.DrawLine(socketPosition, dir);
    }
}
