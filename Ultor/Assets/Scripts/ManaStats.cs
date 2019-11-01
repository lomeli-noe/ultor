using UnityEngine.UI;
using UnityEngine;

public class ManaStats : MonoBehaviour
{
    public static ManaStats instance;

    private Image content;
    private float currentMana;
    private float currentFill;
    public int maxFill = 100;

    public float MyCurrentValue
    {
        get { return currentMana; }
        set
        {
            if (value > maxFill)
            { currentMana = maxFill; }
            else if (value < 0)
            {
                currentMana = 0;
            }
            else
            {
                currentMana = value;
            }
            currentFill = currentMana / maxFill;
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
        currentFill = 0;
        currentMana = 0;
        content = GetComponent<Image>();
        content.fillAmount = currentFill;
    }
    // Update is called once per frame
    private void Update()
    {

        content.fillAmount = currentFill;
    }
}
