using UnityEngine;
using System;

public class Player : MonoBehaviour
{

    public Transform enemySpawnPoint;
	public float manaValue;
	public float healthValue;

    [SerializeField]
	private Stats health;

	[SerializeField]
	private Stats mana;

    public int fallBoundary = -20;

	private void Start()
	{
		mana.MyCurrentValue = 0;
	}

    private void Update()
    {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer(99999);
        }

		mana.MyCurrentValue += manaValue;
		health.MyCurrentValue += healthValue;
	}



    public void DamagePlayer(int damage)
    {
		health.MyCurrentValue -= damage;
        if(health.MyCurrentValue <= 0)
        {
            GameMaster.KillPlayer(this);
		}

	}
    

}
