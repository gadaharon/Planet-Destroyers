using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackHandler : MonoBehaviour
{
    [SerializeField] List<SpecialAttackSO> specialAttackList = new List<SpecialAttackSO>();

    public GameObject FireRandomSpecialAttack()
    {
        SpecialAttackSO randomSpecialAttack = specialAttackList[Random.Range(0, specialAttackList.Count)];

        if (randomSpecialAttack.attackType == SpecialAttackType.LasersAOE)
        {
            return ShootLaserCanons(randomSpecialAttack);
        }
        else if (randomSpecialAttack.attackType == SpecialAttackType.MissilesAOE)
        {
            return FireMissiles(randomSpecialAttack);
        }

        return null;
    }


    GameObject ShootLaserCanons(SpecialAttackSO laserAttack)
    {
        GameObject attack = Instantiate(laserAttack.prefab, transform.position, Quaternion.identity);
        Destroy(attack, laserAttack.attackDuration);
        return attack;
    }

    GameObject FireMissiles(SpecialAttackSO missilesAttack)
    {
        GameObject attack = Instantiate(missilesAttack.prefab, transform.position, Quaternion.identity);
        Destroy(attack, missilesAttack.attackDuration);
        return attack;
    }


}
