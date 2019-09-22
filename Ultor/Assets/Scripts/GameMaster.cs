using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
	private bool canSpawn;

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

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;
    public AudioClip respawnAudio;

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
		gm.StartCoroutine(gm.RespawnPlayer());  
	}

    public static void KillEnemy(Enemy enemy)
	{
		Destroy(enemy.gameObject); 
	}
}
