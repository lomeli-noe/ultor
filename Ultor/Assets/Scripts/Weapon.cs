using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0f;
    public int Damage = 10;
    public LayerMask whatToHit;
    float timeToSpawnEffect = 0f;
    public float effectSpawnRate = 10;
    float timeToFire = 0f;
    Transform firePoint;
    public Transform MuzzleFlashPrefab;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No Firepoint!!!!");
        }
     }

    private void Update()
    {
        if(fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }         
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector2 dir = new Vector2(Camera.main.ScreenToWorldPoint(firePoint.position).x + 100, Camera.main.ScreenToWorldPoint(firePoint.position).y + 5);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        if (transform.localScale.x < 0f)
        {
            dir.x = -dir.x;
        }

        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, dir - firePointPosition, 100, whatToHit);

        if(Time.time >= timeToSpawnEffect)
        {
            Effect();
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }

        if (hit.collider != null)
        {
            //Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.DamageEnemy(Damage);

            }

        }
    }

    void Effect()
    {
        Debug.Log("Effect!!!");
        Transform clone = Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(-size, size, size);
        Destroy(clone.gameObject, 0.1f);
    }
}