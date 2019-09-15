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
    public Transform HitPrefab;

    public float camShakeAmt = 0.05f;
    public float camShakeLength = 0.1f;
    CameraShake camShake;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No Firepoint!!!!");
        }
     }

    private void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if(camShake == null)
        {
            Debug.LogError("No camera shake script found on GM object.");
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

        

        if (hit.collider != null)
        {
            //Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.DamageEnemy(Damage);

            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitNormal;
            Vector3 hitPos;

            if(hit.collider == null)
            {
                hitNormal = new Vector3(9999, 9999, 9999);
                hitPos = hit.point;
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            StartCoroutine(Effect(hitPos, hitNormal));
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    IEnumerator Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        if(hitNormal != new Vector3(9999, 9999, 9999))
        {
            Transform hitParticle = Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(hitParticle.gameObject, 1f);
        }

		yield return new WaitForSeconds(.07f);
        Transform clone = Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(-size, size, size);
        Destroy(clone.gameObject, 0.05f);

        camShake.Shake(camShakeAmt, camShakeLength);
    }
}