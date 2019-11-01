using UnityEngine;

public class BlueCoin : MonoBehaviour
{
	private GameObject manaObject;
	private ManaStats manaStats;

	private void Start()
	{
		manaObject = GameObject.Find("ManaLevel");
		manaStats = manaObject.GetComponent<ManaStats>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger != true && collision.CompareTag("Player"))
        {
			manaStats.MyCurrentValue += 5f;
			Debug.Log("mana: " + manaStats.MyCurrentValue);
			AudioManager.instance.PlaySound("CoinPickUp");
            Destroy(this.gameObject);
        }
    }
}
