using UnityEngine;

public class KickTrigger : MonoBehaviour
{
    public int damage = 20;
    bool hitFromLeft;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitFromLeft = transform.position.x < collision.transform.position.x ? true : false;

        if (collision.isTrigger != true && collision.CompareTag("Enemy"))
        {
            collision.SendMessageUpwards("KickEnemy", hitFromLeft);
        }
    }
}