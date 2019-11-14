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
    bool canSpawn = false;

    void Update()
    {

        StartCoroutine(FirstSpawn());

        if (Time.time > nextSpawn && canSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            randX = Random.Range(-7f, 7f);
            spawnRate = Random.Range(5f, spawnRate);
            whereToSpawn = new Vector2(transform.position.x + randX, transform.position.y);
            GameObject clone = Instantiate(enemy, whereToSpawn, Quaternion.identity);
            StartCoroutine(ResetAI(clone));
        }
    }

    IEnumerator ResetAI(GameObject clone)
    {
        clone.GetComponent<EnemyAI>().enabled = false;
        yield return new WaitForSeconds(1f);
        clone.GetComponent<EnemyAI>().enabled = true;
    }

    IEnumerator FirstSpawn()
    {
        yield return new WaitForSeconds(spawnRate);
        canSpawn = true;
    }

}
