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

    private Stats stats;

    private void OnEnable()
    {
        stats = Stats.instance;
        UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "HEALTH: " + stats.maxHealth.ToString();
        speedText.text = "SPEED: " + stats.movementSpeed.ToString();
    }

    public void UpgradeHealth()
    {
        if(GameMaster.money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);
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

        stats.movementSpeed = Mathf.Round(stats.movementSpeed * movementSpeedMultiplier);
        GameMaster.money -= upgradeCost;
        AudioManager.instance.PlaySound("Money");
        UpdateValues();
    }
}
