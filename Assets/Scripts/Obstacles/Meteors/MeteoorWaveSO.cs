using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeteorWave_", menuName = "Scriptable Objects/Obstacles/MeteorWave")]
public class MeteoorWaveSO : ScriptableObject
{
    public List<GameObject> meteorPrefabList;
    public List<Transform> pathList;
    public float speed;
    public float timeBetweenSpawn;
}
