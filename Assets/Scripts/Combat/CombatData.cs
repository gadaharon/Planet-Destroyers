using System;
using UnityEngine;

public enum ShootDirection
{
    Up = 1,
    Down = -1
}

[Serializable]
public class CombatData
{
    public ShootingPattern shootingPattern;
    public int numberOfProjectiles;
    public float fireRate;
    public float projectileAngleSpread = 2;
    public ShootDirection shootDirection = ShootDirection.Up;
    public ProjectileSettings projectileSettings;
}

[Serializable]
public class ProjectileSettings
{
    public float projectileSpeed;
    public float projectileSize = 1;
    public int damage;
    public Material projectileColor;
}
