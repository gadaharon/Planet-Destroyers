using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    [SerializeField] List<ProjectilePoolTypeSO> poolTypes = new List<ProjectilePoolTypeSO>();

    Dictionary<string, GameObject[]> projectilePools = new Dictionary<string, GameObject[]>();
    Dictionary<string, int> poolIndexes = new Dictionary<string, int>();



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        CreatePoolsByType();
    }


    void CreatePoolsByType()
    {
        foreach (ProjectilePoolTypeSO poolType in poolTypes)
        {
            GameObject[] projectilePool = new GameObject[poolType.poolSize];
            for (int i = 0; i < poolType.poolSize; i++)
            {
                GameObject projectile = Instantiate(poolType.projectilePrefab, transform);
                projectile.SetActive(false);
                projectilePool[i] = projectile;
            }

            projectilePools.Add(poolType.projectileType, projectilePool);
            poolIndexes.Add(poolType.projectileType, 0);
        }
    }

    public GameObject GetProjectile(string projectileType)
    {
        if (projectilePools.ContainsKey(projectileType))
        {
            GameObject[] pool = projectilePools[projectileType];
            int index = poolIndexes[projectileType];

            for (int i = 0; i < pool.Length; i++)
            {
                int nextIndex = (index + i) % pool.Length;
                if (!pool[nextIndex].activeInHierarchy)
                {
                    poolIndexes[projectileType] = (nextIndex + 1) % pool.Length;
                    pool[nextIndex].SetActive(true);
                    return pool[nextIndex];
                }
            }

            Debug.LogWarning("Projectile pool for type " + projectileType + " is empty! Consider increasing the pool size.");
        }
        return null;
    }

    public void ReturnProjectile(GameObject projectile, string projectileType)
    {
        projectile.SetActive(false);
        if (!projectilePools.ContainsKey(projectileType))
        {
            Debug.LogError("Projectile pool for type " + projectileType + " does not exist!");
            Destroy(projectile);
        }
    }
}
