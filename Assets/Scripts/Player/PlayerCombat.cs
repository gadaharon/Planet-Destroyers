using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public bool CanFireSpecialAttack => currentSpecialAttackValue == specialAttackTargetValue;

    [Tooltip("Determines the screen shake when player takes damage")]
    [SerializeField] ShakeSettingsSO takeDamageShakeDetails;

    [SerializeField] CombatData combatData;


    Flash flash;
    ShooterManager shooterManager;
    Health health;
    Shield playerShield;

    const int specialAttackAmountToAddPerKill = 10;
    const int specialAttackTargetValue = 100;
    int currentSpecialAttackValue = 0;

    float lastTimeFire = 0;


    void Awake()
    {
        shooterManager = GetComponent<ShooterManager>();
        health = GetComponent<Health>();
        playerShield = GetComponent<Shield>();
        flash = GetComponent<Flash>();
    }

    void Start()
    {
        if (combatData.shootingPattern == ShootingPattern.SingleShot && combatData.numberOfProjectiles > 1)
        {
            combatData.shootingPattern = ShootingPattern.Spread;
        }
        UIManager.Instance.SetPlayerHealthSliderMaxValue(health.MaxHealth);
        UIManager.Instance.SetSpecialAttackSliderMaxValue(specialAttackTargetValue);
    }

    void OnEnable()
    {
        health.OnDeath += Die;
        EnemyBommerr.OnEnemyExplode += HandleExplosionDamage;
    }

    void OnDisable()
    {
        health.OnDeath -= Die;
        EnemyBommerr.OnEnemyExplode -= HandleExplosionDamage;
    }

    public void AddBulletSpread()
    {
        if (combatData.shootingPattern == ShootingPattern.SingleShot)
        {
            combatData.shootingPattern = ShootingPattern.Spread;
        }
        combatData.numberOfProjectiles += 1;
    }

    public void ProcessFire()
    {
        if (Time.time > lastTimeFire + combatData.fireRate)
        {
            lastTimeFire = Time.time;
            shooterManager.ShootProjectileByPattern(combatData);
        }
    }

    public void ChangeShootingPattern(ShootingPattern shootingPattern)
    {
        combatData.shootingPattern = shootingPattern;
    }

    public void AddHealth(int amount)
    {
        health.AddHealth(amount);
        UIManager.Instance.UpdatePlayerHealthSliderValue(health.CurrentHealth);
    }

    public void ResetHealth()
    {
        health.ResetHealth();
        UIManager.Instance.UpdatePlayerHealthSliderValue(health.CurrentHealth);
    }

    public void TakeDamage(string damageDealerTag, int damage, bool canTakeDamage)
    {
        // if (damageDealerTag != "Player" && canTakeDamage)
        if (damageDealerTag != "Projectile_Player" && canTakeDamage)
        {
            if (playerShield.hasShield)
            {
                playerShield.TakeDamage(damage);
            }
            else
            {
                flash.StartFlash();
                health.TakeDamage(damage);
                ScreenShakeHandler.Instance.ShakeCamera(takeDamageShakeDetails);
                UIManager.Instance.UpdatePlayerHealthSliderValue(health.CurrentHealth);
            }
        }
    }

    void HandleExplosionDamage(string damageDealerTag, int damage)
    {
        TakeDamage(damageDealerTag, damage, PlayerController.Instance.CanTakeDamage);
    }

    public void ResetSpecialAttackBar()
    {
        currentSpecialAttackValue = 0;
        UIManager.Instance.UpdateSpecialAttackSliderValue(currentSpecialAttackValue);
        UIManager.Instance.ToggleSpecialAttackIsFullIndicator(false);
    }

    public void IncreaseSpecialAttackBar()
    {
        currentSpecialAttackValue += specialAttackAmountToAddPerKill;
        if (currentSpecialAttackValue >= specialAttackTargetValue)
        {
            currentSpecialAttackValue = specialAttackTargetValue;
            UIManager.Instance.ToggleSpecialAttackIsFullIndicator(true);
        }
        UIManager.Instance.UpdateSpecialAttackSliderValue(currentSpecialAttackValue);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
