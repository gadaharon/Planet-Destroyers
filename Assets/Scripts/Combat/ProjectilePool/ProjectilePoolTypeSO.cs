using UnityEngine;

[CreateAssetMenu(fileName = "ProjectilePoolType_", menuName = "Scriptable Objects/Projectile Pool Type")]
public class ProjectilePoolTypeSO : ScriptableObject
{
    public string projectileType;
    public GameObject projectilePrefab;
    public int poolSize;
}
