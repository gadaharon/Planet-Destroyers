using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeteorSpawner : Singleton<MeteorSpawner>
{
    [SerializeField] List<MeteoorWaveSO> meteoorWaveList;

    int damage = 5;
    int currentWaveIndex = 0;
    float speedMultiplier = 1f;
    bool isWaveActive = false;

    float minTimeToSpawnMeteors = 30f;
    float maxTimeToSpawnMeteors = 90f;

    float meteorsSpawnerCooldown;

    protected override void Awake()
    {
        base.Awake();
        ResetWaveCooldown();
    }

    void OnEnable()
    {
        EnemySpawner.OnWaveChanged += OnEnemyWaveChangedHandler;
    }

    void OnDisable()
    {
        EnemySpawner.OnWaveChanged -= OnEnemyWaveChangedHandler;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     SpawnMeteorWave();
        // }
        SpawnByTimer();
    }

    void OnEnemyWaveChangedHandler(EnemySpawner spawner)
    {
        if (!isWaveActive && spawner.CurrentWaveIndex > 0)
        {
            SpawnMeteorWave();
        }
    }

    void SpawnByTimer()
    {
        meteorsSpawnerCooldown -= Time.deltaTime;
        if (meteorsSpawnerCooldown <= 0)
        {
            if (!isWaveActive)
            {
                SpawnMeteorWave();
            }
        }
    }


    void SpawnMeteorWave()
    {
        isWaveActive = true;
        MeteoorWaveSO currentWave = meteoorWaveList[currentWaveIndex];
        StartCoroutine(ShootMeteors(currentWave));
    }

    IEnumerator ShootMeteors(MeteoorWaveSO currentWave)
    {
        GameObject prefab = currentWave.meteorPrefabList[0];
        foreach (Transform path in currentWave.pathList)
        {
            Transform[] waypoints = GetWaypointsInPath(path).ToArray();
            GameObject meteor = Instantiate(prefab, transform);
            if (meteor.GetComponent<Meteor>() != null)
            {
                meteor.GetComponent<Meteor>().Initialize(waypoints, currentWave.speed * speedMultiplier, damage);
                yield return new WaitForSeconds(currentWave.timeBetweenSpawn);
            }
        }

        if (currentWaveIndex < meteoorWaveList.Count - 1)
        {
            currentWaveIndex++;
            isWaveActive = false;
            ResetWaveCooldown();
        }
        else
        {
            currentWaveIndex = 0;
            speedMultiplier += .2f;
            isWaveActive = false;
            ResetWaveCooldown();
        }
    }

    void ResetWaveCooldown()
    {
        meteorsSpawnerCooldown = Random.Range(minTimeToSpawnMeteors, maxTimeToSpawnMeteors);

    }

    IEnumerable<Transform> GetWaypointsInPath(Transform path)
    {
        foreach (Transform waypoint in path)
        {
            yield return waypoint;
        }
    }




}
