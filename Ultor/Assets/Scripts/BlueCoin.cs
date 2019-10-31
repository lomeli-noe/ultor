using UnityEngine;

public class BlueCoin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.isTrigger != true && collision.CompareTag("Player"))
        {
            collision.SendMessageUpwards("ManaIncrease");
            AudioManager.instance.PlaySound("CoinPickUp");
            Destroy(this.gameObject);
        }
    }
}
