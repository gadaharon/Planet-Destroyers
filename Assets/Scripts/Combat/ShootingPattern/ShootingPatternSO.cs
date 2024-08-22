using UnityEngine;

[CreateAssetMenu(fileName = "Shooting Pattern", menuName = "Scriptable Objects/Shooting Pattern")]
public class ShootingPatternSO : ScriptableObject
{
    // The type of the projectile, so the projectile pool will know where to get the projectile from
    public string projectileType;
    public ShootingPattern shootingPattern;
    public int numberOfProjectiles;
    public float fireRate;
    public float projectileSpeed;
}
