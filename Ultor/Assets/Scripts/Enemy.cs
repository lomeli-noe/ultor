using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;

        private int _curHealth;

        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 20;

        public void Init()
        {
            curHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();
    public float knockBack;
    private Rigidbody2D rb;
    private BoxCollider2D boxCol;
    public Transform HitPrefab;
    AudioManager audioManager;
    string punchEnemySound = "PunchEnemy";

    public int moneyDrop = 10;
    public float enemyDuration = 10f;
    private bool canHurtPlayer;
    private bool canHurtEnemy;
    private Animator m_Anim;

    private void Start()
    {
        m_Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        stats.Init();

        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No audiomanager found!");
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer, true);
        StartCoroutine(DestroyEnemy());
        StartCoroutine(ResetDamage());
    }

    IEnumerator ResetDamage()
    {
        canHurtPlayer = false;
        canHurtEnemy = false;
        yield return new WaitForSeconds(1f);
        canHurtPlayer = true;
        canHurtEnemy = true;
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(enemyDuration);
        StartCoroutine(Descend());
    }

    IEnumerator Descend()
    {
        m_Anim.SetBool("Descend", true);
        yield return new WaitForSeconds(.2f);
        Destroy(this.gameObject);
    }

    void OnUpgradeMenuToggle(bool active)
    {
        GetComponent<EnemyAI>().enabled = !active;
    }

    public void DamageEnemy(int damage)
    {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }
    }

    public void PunchEnemy(bool hitFromLeft)
    {
        if (canHurtEnemy)
        {
            stats.curHealth -= 20;
            if (stats.curHealth <= 0)
            {
                GameMaster.KillEnemy(this);
            }

            if (hitFromLeft)
            {
                rb.velocity = new Vector2(knockBack, knockBack / 2);
            }
            else
            {
                rb.velocity = new Vector2(-knockBack, knockBack / 2);
            }
            audioManager.PlaySound(punchEnemySound);
            Transform hitParticle = Instantiate(HitPrefab, transform.position, Quaternion.FromToRotation(Vector3.right, transform.position)) as Transform;
            Destroy(hitParticle.gameObject, 1f);
            StartCoroutine(ResetCollider());
        }
    }

    public void KickEnemy(bool hitFromLeft)
    {
        if (canHurtEnemy)
        {
            stats.curHealth -= 40;
            if (stats.curHealth <= 0)
            {
                GameMaster.KillEnemy(this);
            }

            if (hitFromLeft)
            {
                rb.velocity = new Vector2(knockBack * 2, knockBack);
            }
            else
            {
                rb.velocity = new Vector2(-knockBack * 2, knockBack);
            }
            audioManager.PlaySound(punchEnemySound);
            Transform hitParticle = Instantiate(HitPrefab, transform.position, Quaternion.FromToRotation(Vector3.right, transform.position)) as Transform;
            Destroy(hitParticle.gameObject, 1f);
            StartCoroutine(ResetCollider());
        }
    }

    IEnumerator ResetCollider()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player _player = collision.collider.GetComponent<Player>();
        if(_player != null && canHurtPlayer)
        {
            _player.DamagePlayer(stats.damage);
        }
    }

    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
