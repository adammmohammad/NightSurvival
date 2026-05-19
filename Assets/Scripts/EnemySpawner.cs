using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float timeBtwSpawns = 5f;
    private float spawnCounter;
    public int maxEnemies = 10;
    public int totalEnemiesSpawned = 0;
    public int totalEnemiesToSpawn = 20;

    void Start()
    {
        spawnCounter = timeBtwSpawns;
    }

    void Update()
    {
        int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (currentEnemies < maxEnemies && totalEnemiesSpawned < totalEnemiesToSpawn)
        {
            spawnCounter -= Time.deltaTime;
            if (spawnCounter <= 0)
            {
                spawnCounter = timeBtwSpawns;
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[randomIndex].position, spawnPoints[randomIndex].rotation);
        totalEnemiesSpawned++;
    }
}