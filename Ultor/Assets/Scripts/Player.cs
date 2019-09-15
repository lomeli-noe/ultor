using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;

		private int _curHealth;

        public int curHealth
		{
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
		}

        public void Init()
		{
			curHealth = maxHealth;
		}
    }

    public PlayerStats stats = new PlayerStats();

    public int fallBoundary = -20;

    [Header("Optional: ")]
	[SerializeField]
	private StatusIndicator statusIndicator;

    void Start()
	{
		stats.Init();

        if(statusIndicator != null)
		{
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}
	}

    private void Update()
    {
        if(transform.position.y <= fallBoundary)
        {
            DamagePlayer(99999);
        }
    }



    public void DamagePlayer(int damage)
    {
        stats.curHealth -= damage;
		Debug.Log("curhealth: " + stats.curHealth);
        if(stats.curHealth <= 0)
        {
            GameMaster.KillPlayer(this);
		}

		if (statusIndicator != null)
		{
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		} 

	}
    

}
