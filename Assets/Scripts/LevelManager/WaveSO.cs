using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "Scriptable Objects/Level/New Wave")]
public class WaveSO : ScriptableObject
{
    public int numberOfEnemiesInWave;
    public List<GameObject> enemyPrefabList;
    public float spawnInterval;
    public int numberOfEnemiesPerSpawn;

    [Header("BOSS PROPERTIES")]
    public bool hasBoss;
    public GameObject bossPrefab;
    [Tooltip("Where the boss instantiated")]
    public Vector2 bossStartPosition;
    [Tooltip("Boss entrance target position, where the boss move towards before start attacking")]
    public Vector2 bossEndPosition;

}
