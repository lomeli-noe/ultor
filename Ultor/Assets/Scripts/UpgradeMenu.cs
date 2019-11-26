using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private float healthMultiplier = 1.3f;

    [SerializeField]
    private float movementSpeedMultiplier = 1.3f;

    [SerializeField]
    private int upgradeCost = 50;

	private GameObject healthObject;
	private Stats playerStats;

	private void OnEnable()
    {
		healthObject = GameObject.Find("HealthLevel");
		playerStats = healthObject.GetComponent<Stats>();
		UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "HEALTH: " + playerStats.maxHealth.ToString();
        speedText.text = "SPEED: " + playerStats.movementSpeed.ToString();
    }

    public void UpgradeHealth()
    {
        if(GameMaster.money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

		playerStats.maxHealth = (int)(playerStats.maxHealth * healthMultiplier);
        GameMaster.money -= upgradeCost;
        AudioManager.instance.PlaySound("Money");
        UpdateValues(); 
    }

    public void UpgradeSpeed()
    {
        if (GameMaster.money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

		playerStats.movementSpeed = Mathf.Round(playerStats.movementSpeed * movementSpeedMultiplier);
        GameMaster.money -= upgradeCost;
        AudioManager.instance.PlaySound("Money");
        UpdateValues();
    }
}
