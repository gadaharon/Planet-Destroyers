using UnityEngine;

public class ProjectilePool : Singleton<ProjectilePool>
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int poolSize;

    GameObject[] projectilePool;
    int index = 0;



    protected override void Awake()
    {
        base.Awake();

        CreatePool();
    }


    void CreatePool()
    {
        projectilePool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform);
            projectile.SetActive(false);
            projectilePool[i] = projectile;
        }
    }

    public GameObject GetProjectile()
    {
        for (int i = 0; i < projectilePool.Length; i++)
        {
            int nextIndex = (index + i) % projectilePool.Length;
            if (!projectilePool[nextIndex].activeInHierarchy)
            {
                index = (nextIndex + 1) % projectilePool.Length;
                projectilePool[nextIndex].SetActive(true);
                return projectilePool[nextIndex];
            }
        }

        Debug.LogWarning("Every projectile in the pool is in use");
        return null;
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
    }
}
