using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "Scriptable Objects/Level/New Wave")]
public class WaveSO : ScriptableObject
{
    public int numberOfEnemiesInWave;
    public List<GameObject> enemyPrefabList;
    public float spawnInterval;
    public int numberOfEnemiesPerSpawn;

    public GameObject bossPrefab;
    public bool hasBoss;
    public Vector2 bossSpawnPosition;

}
