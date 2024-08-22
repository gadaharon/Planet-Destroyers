using System.Collections;
using UnityEngine;

public class ShooterManager : MonoBehaviour
{
    float radius = 1f;

    // Spiral rotating pattern;
    float spiralRotatingAngle = 0;
    float spiralRotatingAngleStep = 30;


    void ShootProjectile(Quaternion rotation, Vector3 position, Vector2 moveDirection, CombatData combatData)
    {
        GameObject projectile = ProjectilePool2.Instance.GetProjectile();
        projectile.tag = $"Projectile_{gameObject.tag}";
        // projectile.tag = gameObject.tag;
        projectile.transform.position = position;
        projectile.transform.rotation = rotation;

        moveDirection = moveDirection * (int)combatData.shootDirection;

        projectile.GetComponent<SpriteRenderer>().material = combatData.projectileColor;
        projectile.GetComponent<Projectile>().Init(moveDirection, combatData.projectileSpeed, combatData.damage, combatData.projectileSize);
    }

    void ShootProjectile(Quaternion rotation, CombatData combatData)
    {
        GameObject projectile = ProjectilePool2.Instance.GetProjectile();
        projectile.tag = $"Projectile_{gameObject.tag}";
        // projectile.tag = gameObject.tag;
        projectile.transform.position = transform.position;
        projectile.transform.rotation = rotation;

        Vector2 moveDirection = Vector2.up * (int)combatData.shootDirection;

        projectile.GetComponent<SpriteRenderer>().material = combatData.projectileColor;
        projectile.GetComponent<Projectile>().Init(moveDirection, combatData.projectileSpeed, combatData.damage, combatData.projectileSize);
    }

    void ShootProjectileSpread(CombatData combatData)
    {
        Vector3 startPoint = transform.position;
        int numberOfProjectiles = combatData.numberOfProjectiles;

        // Define thresholds for angle ranges
        int thresholdProjectiles = 36; // Number of projectiles needed to reach a full circle
        float minRange = 10.0f; // Minimum range for a very small number of projectiles
        float maxRange = 360.0f;

        // Calculate the total angle based on the number of projectiles
        float totalAngle = (numberOfProjectiles >= thresholdProjectiles) ? maxRange :
                           Mathf.Lerp(minRange, maxRange, (float)numberOfProjectiles / thresholdProjectiles);

        // Calculate start and end angles
        float startAngle = -totalAngle / combatData.projectileAngleSpread;
        float endAngle = totalAngle / combatData.projectileAngleSpread;

        // Here checking the angles to prevent projectiles overlapping each other
        int actualNumberOfProjectiles = (endAngle - startAngle == 360) ? numberOfProjectiles : numberOfProjectiles - 1;

        float angleStep = (endAngle - startAngle) / actualNumberOfProjectiles;
        float angle = startAngle;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float projectileDirXPosition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
            Vector3 projectileVectorDirection = (projectileVector - startPoint).normalized;

            ShootProjectile(transform.rotation, startPoint, projectileVectorDirection, combatData);

            angle += angleStep;
        }
    }

    void ShootProjectileSpiralRotating(CombatData combatData)
    {
        float projectileDirX = transform.position.x + Mathf.Sin((spiralRotatingAngle * Mathf.PI) / 180f);
        float projectileDirY = transform.position.y + Mathf.Cos((spiralRotatingAngle * Mathf.PI) / 180f);

        Vector3 projectileVector = new Vector3(projectileDirX, projectileDirY, 0f);
        Vector2 projectileDir = (projectileVector - transform.position).normalized;

        ShootProjectile(transform.rotation, transform.position, projectileDir, combatData);

        spiralRotatingAngle += spiralRotatingAngleStep;
    }

    void ShootProjectileSniper(CombatData combatData)
    {
        StartCoroutine(ShootProjectilesSingleLine(combatData));
    }

    IEnumerator ShootProjectilesSingleLine(CombatData combatData)
    {
        for (int i = 0; i < combatData.numberOfProjectiles; i++)
        {
            ShootProjectile(transform.rotation, combatData);
            yield return new WaitForSeconds(.1f);
        }

    }

    /// <summary>
    /// receives ProjectileShootingPatternSO, damage, speed 
    /// </summary>
    public void ShootProjectileByPattern(CombatData combatData)
    {
        switch (combatData.shootingPattern)
        {
            case ShootingPattern.SingleShot:
                ShootProjectile(transform.rotation, combatData);
                break;
            case ShootingPattern.Spread:
                ShootProjectileSpread(combatData);
                break;
            case ShootingPattern.Sniper:
                ShootProjectileSniper(combatData);
                break;
            case ShootingPattern.Spiral:
                ShootProjectileSpiralRotating(combatData);
                break;
            default:
                break;
        }

    }


}
