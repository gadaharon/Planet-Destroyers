using System;
using UnityEngine;

[RequireComponent(typeof(ShooterManager))]
public class EnemyCombatRange : EnemyBase
{
    [SerializeField] float stoppingPoint = 10f;
    [SerializeField] float closestPlayerPoint = 3f;
    [SerializeField] CombatData combatData;

    Flash flash;
    ShooterManager shooterManager;
    Health health;
    Bounds bounds;

    float lastTimeShoot = 0;

    void Awake()
    {
        shooterManager = GetComponent<ShooterManager>();
        health = GetComponent<Health>();
        flash = GetComponent<Flash>();
    }

    void OnEnable()
    {
        health.OnDeath += Die;
    }

    void OnDisable()
    {
        health.OnDeath -= Die;
    }

    void Start()
    {
        bounds = GameManager.Instance.GetCameraBounds();
    }

    public override void TakeDamage(int damage)
    {
        flash.StartFlash();
        health.TakeDamage(damage);
    }

    public override void HandleDamage()
    {
        if (Time.time > lastTimeShoot + combatData.fireRate)
        {
            lastTimeShoot = Time.time;
            shooterManager.ShootProjectileByPattern(combatData);
        }
    }

    Vector2 MoveTowardsClamp(Vector2 position)
    {
        position.x = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
        position.y = Mathf.Clamp(position.y, bounds.min.y, bounds.max.y);

        return position;
    }

    public override void Move()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > stoppingPoint)
        {
            Vector2 position = Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, movementSpeed * Time.deltaTime);
            transform.position = MoveTowardsClamp(position);
        }
        else if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < closestPlayerPoint)
        {
            Vector2 position = Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, -movementSpeed * Time.deltaTime);
            transform.position = MoveTowardsClamp(position);
        }
    }

    void Die()
    {
        EnemyController.OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }
}
