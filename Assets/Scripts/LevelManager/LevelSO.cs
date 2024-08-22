using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "Scriptable Objects/Level/New Level")]
public class LevelSO : ScriptableObject
{
    public List<WaveSO> waveList;
}
