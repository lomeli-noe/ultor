using UnityEngine;
using System;

[RequireComponent(typeof(Platformer2DUserControl))]
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
        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;
	}

    private void Update()
    {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer(99999);
        }

	}

    void OnUpgradeMenuToggle(bool active)
    {
        GetComponent<Platformer2DUserControl>().enabled = !active;
        
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
