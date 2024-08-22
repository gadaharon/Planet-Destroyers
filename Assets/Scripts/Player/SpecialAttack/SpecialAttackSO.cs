using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialAttack_", menuName = "Scriptable Objects/SpecialAttack")]
public class SpecialAttackSO : ScriptableObject
{
    public string specialAttackName;
    public SpecialAttackType attackType;
    public GameObject prefab;
    public bool hasAttackDuration;
    public int attackDuration;
}
