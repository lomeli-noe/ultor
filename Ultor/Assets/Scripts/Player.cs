using UnityEngine;
using System;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{

    public Transform enemySpawnPoint;
	public float manaValue;
	public float healthValue;

    [SerializeField]
	public Stats health;

	[SerializeField]
	public Stats mana;

    private GameObject healthObject;
    private GameObject manaObject;

    public float invincibleTime = 2f;

    public int fallBoundary = -20;

	private void Start()
	{
        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        healthObject = GameObject.Find("HealthLevel");
        health = healthObject.GetComponent<Stats>();

        manaObject = GameObject.Find("ManaLevel");
        mana = manaObject.GetComponent<Stats>();
        health.MyCurrentValue = health.maxHealth;

        InvokeRepeating("RegenHealth", 1f/health.healthRegenRate, 1f/health.healthRegenRate);
	}



    private void Update()
    {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer(99999);
        }

	}

    void RegenHealth()
    {
        if(GetComponent<Platformer2DUserControl>().enabled)
            health.MyCurrentValue += 1;
        else
            health.MyCurrentValue += 0;

    }

    void OnUpgradeMenuToggle(bool active)
    {
        GetComponent<Platformer2DUserControl>().enabled = !active;
        health.enabled = !active;

    }

    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }

    public void DamagePlayer(int damage)
    {
		health.MyCurrentValue -= damage;
        AudioManager.instance.PlaySound("PunchPlayer");
        if (health.MyCurrentValue <= 0)
        {
            GameMaster.KillPlayer(this);
		}

        GetComponent<PlatformerCharacter2D>().HurtEffect(invincibleTime);

    }
    

}
