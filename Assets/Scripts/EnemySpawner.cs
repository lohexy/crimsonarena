using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySettings
    {
        public string name;
        public GameObject prefab;
        public int minWaveToSpawn = 1;
        [Range(1, 100)] public int spawnWeight = 50;
    }

    [Header("Список ворогів та їхні хвилі")]
    public EnemySettings[] enemyPool;

    [Header("Точки появи за картою")]
    public Transform[] spawnPoints; 

    [Header("Налаштування балансу")]
    public float spawnRate = 1.0f;
    public int baseMaxEnemies = 10;
    public float percentIncreasePerWave = 5f;
    public float waveDuration = 30f;

    private int currentWave = 1;
    private int maxEnemiesInThisWave;
    private int enemiesSpawnedInCurrentWave = 0;
    private float nextSpawnTime = 0f;
    private float waveTimer = 0f;

    void Start()
    {
        waveTimer = waveDuration;
        CalculateWaveLimits();
        Debug.Log($"<color=cyan>=== ГРА ПОЧАЛАСЯ! ХВИЛЯ {currentWave} ===</color>");
    }

    void Update()
    {
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0)
        {
            NextWave();
        }

        if (Time.time >= nextSpawnTime && enemiesSpawnedInCurrentWave < maxEnemiesInThisWave)
        {
            nextSpawnTime = Time.time + spawnRate;
            SpawnEnemy();
        }
    }

    void CalculateWaveLimits()
    {
        float multiplier = 1f + (percentIncreasePerWave / 100f) * (currentWave - 1);
        maxEnemiesInThisWave = Mathf.RoundToInt(baseMaxEnemies * multiplier);
        enemiesSpawnedInCurrentWave = 0;
    }

    void NextWave()
    {
        currentWave++;
        waveTimer = waveDuration;
        CalculateWaveLimits();

        Debug.Log($"<color=gold>--- НОВА ХВИЛЯ: {currentWave} ---</color>");
        Debug.Log($"Цільова кількість ворогів: {maxEnemiesInThisWave}");
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPool.Length == 0) return;

        List<EnemySettings> availableEnemies = new List<EnemySettings>();
        int totalWeight = 0;

        foreach (var enemy in enemyPool)
        {
            if (currentWave >= enemy.minWaveToSpawn)
            {
                availableEnemies.Add(enemy);
                totalWeight += enemy.spawnWeight;
            }
        }

        GameObject selectedPrefab = null;

        if (availableEnemies.Count == 0)
        {
            selectedPrefab = enemyPool[0].prefab;
        }
        else
        {
            int randomWeightValue = Random.Range(0, totalWeight);
            int cursor = 0;
            
            foreach (var enemy in availableEnemies)
            {
                cursor += enemy.spawnWeight;
                if (randomWeightValue <= cursor)
                {
                    selectedPrefab = enemy.prefab;
                    break;
                }
            }
        }

        if (selectedPrefab == null) return;

        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(selectedPrefab, randomPoint.position, Quaternion.identity);
        enemiesSpawnedInCurrentWave++;

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            int hpBonus = currentWave / 5;
            enemyScript.maxHealth += hpBonus;
        }
    }
}