using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    float randX;
    Vector2 whereToSpawn;
    public float spawnRate;
    float nextSpawn = 0.0f;

    void Update()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            randX = Random.Range(-7f, 7f);
            spawnRate = Random.Range(5f, 10);
            whereToSpawn = new Vector2(transform.position.x + randX, transform.position.y);
            Instantiate(enemy, whereToSpawn, Quaternion.identity);
        }
    }
}
