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
    public float projectileSpeed;
    public float projectileAngleSpread = 2;
    public float projectileSize = 1;
    public ShootDirection shootDirection = ShootDirection.Up;
    public int damage;
    public Material projectileColor;
}
