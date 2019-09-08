using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{

    public float fireRate = 0f;
    public int Damage = 25;
    public LayerMask whatToHit;

    public GameObject bullet;
    public Transform firePoint;
    public Transform bulletTrail;
    //public Transform hitPrefab;


    // Ammunition
    public int maxAmmo = 10;
    public int currentAmmo;
    public int ammunition;
    public float reloadTime = 1f;
    private bool isReloading = false;
    public bool isEmpty = false;

    // Handle camera shaking
    public float camShakeAmt = 0.05f;
    public float camShakeLength = 0.1f;
    //CameraShake cameraShake;

    public static Weapon instance;

    public string weaponShootSound = "DefaultShot";


    private float timeToFire = 0f;
    private float timeToSpawnEffect = 0f;

    public float effectSpawnRate = 10f;
    //Transform firePoint;

    // Caching
    //AudioManager audioManager;

    public Vector2 velocity;

    // Use this for initialization
    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firepoint!!!!!!!!");
        }

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        //cameraShake = GameMaster.gameMaster.GetComponent<CameraShake>();
        //if (cameraShake == null)
        //{
        //    Debug.LogError("No cameraShake script found on GM object");
        //}

        //audioManager = AudioManager.instance;
        //if (audioManager == null)
        //{
        //    Debug.LogError("Panic!!! No audiomanager found");
        //}

        currentAmmo = maxAmmo;
        ammunition = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0 && ammunition > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (fireRate == 0)
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
                if (fireRate > 0 && fireRate <= 4)
                    fireRate = 4;
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }

    }

    public IEnumerator Reload()
    {
        isReloading = true;
        //audioManager.PlaySound("Reload");
        yield return new WaitForSeconds(reloadTime);

        currentAmmo += 10;
        ammunition -= 10;
        isReloading = false;
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
            isEmpty = true;
        else
            isEmpty = false;

        if (isEmpty)
        {
            //audioManager.PlaySound("EmptyGun");
            return;
        }

        Vector2 dir = new Vector2(firePoint.position.x + 100, firePoint.position.y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        if (transform.localScale.x < 0f)
        {
            dir.x = -dir.x;
        }

        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, dir - firePointPosition, 100, whatToHit);
        //Debug.DrawLine(firePointPosition, (dir - firePointPosition), Color.cyan);

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitNormal;
            Vector3 hitPos;

            if (hit.collider == null)
            {
                hitNormal = new Vector3(9999, 9999, 9999);
                hitPos = hit.point;
            }
            else
            {
                hitNormal = hit.normal;
                hitPos = hit.point;
            }

            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;

            currentAmmo--;
        }


        //if (hit.collider != null)
        //{
        //    //Enemy enemy = hit.collider.GetComponent<Enemy>();
        //    //if (enemy != null && Mathf.Abs(Player.instance.transform.position.x - enemy.transform.position.x) <= 18)
        //    {
        //        //enemy.DamageEnemy(Damage);
        //    }
        //}
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform clone = Instantiate(bulletTrail, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        //clone.localScale = new Vector3(size, size, size);
        //Destroy(clone.gameObject, 0.04f);

        //if (hitNormal != new Vector3(9999, 9999, 9999))
        //{
        //    Transform hitParticle = Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.up, hitNormal));
        //    Destroy(hitParticle.gameObject, 1f);
        //}

        // Shake the camera
        //cameraShake.Shake(camShakeAmt, camShakeLength);

        // Play shoot sound
        //audioManager.PlaySound(weaponShootSound);
    }
}
