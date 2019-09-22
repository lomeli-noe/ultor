using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBarRect;

    private void Start()
    {
        if(healthBarRect == null)
        {
            Debug.LogError("No health bar Status Indicator");
        }
    }

    public void SetHealth(int _cur, int _max)
    {
        float _value = (float)_cur / _max;
        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);

    }
}
