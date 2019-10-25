using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

	private static int _remainingLives; 
    public static int RemainingLives
	{
        get { return _remainingLives; }
	}


    [SerializeField]
    private int startingMoney;
    public static int money;

	private bool canSpawn;
	public Transform playerPrefab;
	public Transform spawnPoint;
	public Transform enemyPrefab;
	public float spawnDelay = 2;
	public Transform spawnPrefab;
    public string spawnSoundName;

	[SerializeField]
	private int maxLives = 3;

	[SerializeField]
	private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private EnemySpawner batSpawner;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;

    private AudioManager audioManager;

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
		_remainingLives = maxLives;
        money = startingMoney;
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No audiomanager found!");
        }
	}

    public void UpgradeMenuToggle()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        enemySpawner.enabled = !upgradeMenu.activeSelf;
        batSpawner.enabled = !upgradeMenu.activeSelf;
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);

    }

    public void EndGame()
	{
		Debug.Log("GAME OVER!!!");
		gameOverUI.SetActive(true);
	}

     public IEnumerator RespawnPlayer()
    {
		canSpawn = false;

        audioManager.PlaySound(spawnSoundName);
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
			gm.StartCoroutine(gm.RespawnPlayer());
		}
		 
	}

    public static void KillEnemy(Enemy enemy)
	{
        money += enemy.moneyDrop;
        Destroy(enemy.gameObject); 
	}
}