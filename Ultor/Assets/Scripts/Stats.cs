using UnityEngine.UI;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats instance;

    private Image content;
    private float currentHealth;
    private float currentFill;
    public int maxHealth = 100;
    public float healthRegenRate = 2f;
    public float movementSpeed = 8f;

    public float MyCurrentValue
    {
        get { return currentHealth; }
        set
        {
            if (value > maxHealth)
            { currentHealth = maxHealth; }
            else if(value < 0)
            {
                currentHealth = 0;
            }
            else
            {
                currentHealth = value;
            }
            currentFill = currentHealth / maxHealth;
        }
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public float MaxValue { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        currentFill = maxHealth;
        currentHealth = maxHealth;
        content = GetComponent<Image>();
        content.fillAmount = currentFill;
    }
    // Update is called once per frame
    private void Update()
    {

        content.fillAmount = currentFill;
    }
}
