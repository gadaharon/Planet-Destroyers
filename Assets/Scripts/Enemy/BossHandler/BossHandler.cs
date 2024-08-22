using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ShooterManager))]
public class BossHandler : EnemyBase
{
    public Action OnBossDefeated;

    [SerializeField] float combatDataChangeCooldown = 30f;
    [SerializeField] CombatData defaultAttackData;
    [SerializeField] CombatData specialAttackData;

    CombatData currentCombatData;

    Bounds bounds;
    Health health;
    ShooterManager shooterManager;
    Vector3 targetPosition;

    bool isSpecialAttackActive = false;

    float secondaryAttackTimer = 20f;

    float currentCombatDataTimer;

    float lastTimeShoot = 0f;


    void Awake()
    {
        health = GetComponent<Health>();
        shooterManager = GetComponent<ShooterManager>();
        currentCombatDataTimer = combatDataChangeCooldown;
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
        bounds = GameManager.GetCameraBounds();
        targetPosition = transform.position;
        currentCombatData = defaultAttackData;
    }

    public override void HandleDamage()
    {
        // Boss attacks goes here
        if (Time.time > lastTimeShoot + currentCombatData.fireRate)
        {
            lastTimeShoot = Time.time;
            shooterManager.ShootProjectileByPattern(currentCombatData);
        }

        HandleCombatDataChange();
    }

    void HandleCombatDataChange()
    {
        currentCombatDataTimer -= Time.deltaTime;
        if (currentCombatDataTimer <= 0 && !isSpecialAttackActive)
        {
            // change combatdata
            StartCoroutine(ChangeCombatDataRoutine());
        }
    }

    IEnumerator ChangeCombatDataRoutine()
    {
        currentCombatData = specialAttackData;
        isSpecialAttackActive = true;

        yield return new WaitForSeconds(secondaryAttackTimer);

        currentCombatDataTimer = combatDataChangeCooldown;
        currentCombatData = defaultAttackData;
        isSpecialAttackActive = false;
    }



    public override void Move()
    {
        // Boss movement goes here
        if (transform.position == targetPosition)// reach destination
        {
            targetPosition = new Vector3(Random.Range(bounds.min.x + 7, bounds.max.x - 7), transform.position.y, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }

    public override void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }

    void Die()
    {
        OnBossDefeated?.Invoke();
        Destroy(gameObject);
    }
}
