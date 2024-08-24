using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : Singleton<EnemySpawner>
{
    public static Action<EnemySpawner> OnWaveChanged;
    public int CurrentWaveIndex => currentWaveIndex;

    [SerializeField] BoxCollider2D spawnAreaCollider;
    [SerializeField] LevelSO level;

    WaveSO currentWave;

    //For test
    [Header("FOR TESTING")]
    [SerializeField] int numberOfEnemies = 0;
    int currentWaveIndex = 0;

    Bounds bounds;
    Coroutine spawnEnemiesCoroutine;

    protected override void Awake()
    {
        base.Awake();
        bounds = spawnAreaCollider.bounds;
    }

    void OnEnable()
    {
        EnemyController.OnEnemyDeath += RemoveEnemyFromWave;
    }

    void OnDisable()
    {
        EnemyController.OnEnemyDeath -= RemoveEnemyFromWave;
    }

    void Start()
    {
        currentWave = level.waveList[currentWaveIndex];
        numberOfEnemies = currentWave.numberOfEnemiesInWave;
        StartNextWave();
    }

    void StartNextWave()
    {
        if (currentWaveIndex < level.waveList.Count)
        {
            StopSpawnEnemiesCoroutine();
            currentWave = level.waveList[currentWaveIndex];
            numberOfEnemies = currentWave.numberOfEnemiesInWave;
            OnWaveChanged?.Invoke(this);

            if (currentWave.hasBoss)
            {
                SpawnBoss();
            }
            else
            {
                spawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
            }
        }
        else
        {
            // All waves completed
            Debug.Log("All waves completed!");
            StopSpawnEnemiesCoroutine();
        }

    }

    void SpawnBoss()
    {
        GameObject boss = Instantiate(currentWave.bossPrefab, currentWave.bossStartPosition, Quaternion.identity, transform);
        BossHandler bossHandler = boss.GetComponent<BossHandler>();
        if (bossHandler != null)
        {
            bossHandler.OnBossDefeated += OnBossDefeated;
            bossHandler.OnBossCombatReady += OnBossCombatReady;
            bossHandler.BossEnter(currentWave.bossEndPosition);
        }
    }



    IEnumerator SpawnEnemies()
    {
        while (numberOfEnemies > 0)
        {
            if (currentWave.numberOfEnemiesPerSpawn > 1)
            {
                for (int i = 0; i < currentWave.numberOfEnemiesPerSpawn; i++)
                {
                    SpawnEnemy();
                }
            }
            else
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(currentWave.spawnInterval);
        }

        while (numberOfEnemies > 0)
        {
            yield return null;
        }

        if (!currentWave.hasBoss)
        {
            currentWaveIndex++;
            StartNextWave();
        }
    }

    void SpawnEnemy()
    {
        GameObject randomEnemyPrefab = currentWave.enemyPrefabList[Random.Range(0, currentWave.enemyPrefabList.Count)];
        Vector2 spawnPosition = GetEnemySpawnPosition();
        Instantiate(randomEnemyPrefab, spawnPosition, Quaternion.identity, transform);
    }

    Vector2 GetEnemySpawnPosition()
    {
        Vector2 spawnPosition = Vector2.zero;
        float randomValue = Random.value;

        if (randomValue < 0.25f)
        {
            // spawn left
            spawnPosition = new Vector2(bounds.min.x, Random.Range(bounds.min.y, bounds.max.y));
        }
        else if (randomValue < 0.5f)
        {
            // Spawn right
            spawnPosition = new Vector2(bounds.max.x, Random.Range(bounds.min.y, bounds.max.y));
        }
        else if (randomValue < 0.75f)
        {
            // Spawn bottom
            spawnPosition = new Vector2(Random.Range(bounds.min.x, bounds.max.x), bounds.min.y);
        }
        else
        {
            // Spawn top
            spawnPosition = new Vector2(Random.Range(bounds.min.x, bounds.max.x), bounds.max.y);
        }

        return spawnPosition;
    }

    void StopSpawnEnemiesCoroutine()
    {
        if (spawnEnemiesCoroutine != null)
        {
            StopCoroutine(spawnEnemiesCoroutine);
        }
    }

    public void RemoveEnemyFromWave()
    {
        numberOfEnemies--;
    }

    void OnBossCombatReady()
    {
        if (currentWave.numberOfEnemiesInWave > 0)
        {
            spawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
        }
    }

    void OnBossDefeated()
    {
        currentWaveIndex++;
        StartNextWave();
    }

}
