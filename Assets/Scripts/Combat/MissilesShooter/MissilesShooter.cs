using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissilesShooter : MonoBehaviour
{
    Transform player;
    // For testing remove SerializeField later
    // Should be taken from the EnemySpawner
    [SerializeField] Transform enemiesParent;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] GameObject missilePrefab;

    int numberOfMissiles = 5;
    float spreadRadius = 5f;

    void Start()
    {
        player = PlayerController.Instance.gameObject.transform;

        FireMissiles();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            FireMissiles();
        }
    }

    IEnumerable<GameObject> GetAllActiveEnemies()
    {
        if (enemiesParent == null)
        {
            enemiesParent = EnemySpawner.Instance.transform;
        }
        foreach (Transform target in enemiesParent)
        {
            if (target.gameObject.activeSelf)
            {
                yield return target.gameObject;
            }
        }
    }

    void FireMissile(GameObject target, int missileIndex, GameObject targetCrosshair = null)
    {
        Vector3 offsetPosition = player.position + GetMissileOffset(missileIndex, numberOfMissiles);

        GameObject missile = Instantiate(missilePrefab, offsetPosition, Quaternion.identity);

        if (missile != null)
        {
            missile.GetComponent<Missile>().Initialize(target, targetCrosshair);
        }
    }

    void FireMissiles()
    {
        // Sort the targets by distance from the player
        List<GameObject> sortedTargets = GetAllActiveEnemies().OrderBy(target => Vector2.Distance(player.position, target.transform.position)).ToList();

        // Take the closest targets (5 targets or less)
        List<GameObject> closestTargets = sortedTargets.Take(5).ToList();

        if (closestTargets.Count > 0)
        {
            int missilePerTarget = numberOfMissiles / closestTargets.Count;
            // incase there are less than 5 targets
            int extraMissiles = numberOfMissiles % closestTargets.Count;

            int missileIndex = 0;

            foreach (GameObject target in closestTargets)
            {
                // Fire the allocated number of missiles at this target
                for (int i = 0; i < missilePerTarget; i++)
                {
                    // Set Target sprite
                    GameObject targetCrosshair = Instantiate(targetPrefab, target.transform.position, Quaternion.identity, transform);
                    // Fire missile
                    FireMissile(target, missileIndex, targetCrosshair);
                    missileIndex++;
                }
            }

            // Fire remaining missiles if there are extras evenly across targets
            for (int i = 0; i < extraMissiles; i++)
            {
                FireMissile(closestTargets[i], missileIndex);
                missileIndex++;
            }
        }
    }

    Vector3 GetMissileOffset(int index, int totalMissiles)
    {
        float angle = (360f / totalMissiles) * index;
        float xOffset = Mathf.Cos(angle * Mathf.Deg2Rad) * spreadRadius;
        float yOffset = Mathf.Sin(angle * Mathf.Deg2Rad) * spreadRadius;

        return new Vector3(xOffset, yOffset, 0);
    }
}
