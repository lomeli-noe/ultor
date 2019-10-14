using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

	private static int _remainingLives = 3; 
    public static int RemainingLives
	{
        get { return _remainingLives; }
	}

	private bool canSpawn;
	public Transform playerPrefab;
	public Transform spawnPoint;
	public Transform enemyPrefab;
	public float spawnDelay = 2;
	public Transform spawnPrefab;
	public AudioClip respawnAudio;

	private void Awake()
    {
        if(gm == null)
        {
			gm = this;
			Debug.Log("Awake GM");
		}
    }

    void Start()
	{
		canSpawn = true;
	}

    public void EndGame()
	{
		Debug.Log("GAME OVER!!!");
	}

     public IEnumerator RespawnPlayer()
    {
		canSpawn = false;
		yield return new WaitForSeconds(spawnDelay);

		AudioSource.PlayClipAtPoint(respawnAudio, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), 0.5f);
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 3f);
		canSpawn = true;
	}

    public static void KillPlayer(Player player)
    {
		if (gm.canSpawn)
		{
			gm.canSpawn = false;
			gm._KillPlayer(player);
		}

		return;
    }

    public void _KillPlayer(Player player)
	{
		Destroy(player.gameObject);
		_remainingLives -= 1;

        if(_remainingLives <= 0)
		{
			gm.EndGame();
		}
		else
		{

		}
		gm.StartCoroutine(gm.RespawnPlayer());  
	}

    public static void KillEnemy(Enemy enemy)
	{
		Destroy(enemy.gameObject); 
	}
}