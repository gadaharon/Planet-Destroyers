using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ShooterManager))]
public class BossHandler : EnemyBase
{
    public Action OnBossDefeated;
    public Action OnBossCombatReady;

    [SerializeField] float combatDataChangeCooldown = 30f;
    [SerializeField] CombatData defaultAttackData;
    [SerializeField] List<CombatData> secondaryAttacks;

    [Header("SPECIAL ATTACK PARAMETERS")]
    [SerializeField] float minSpecialAttackColldown = 10f;
    [SerializeField] float maxSpecialAttackCooldown = 20f;

    CombatData currentCombatData;
    ISpecialAttack[] specialAttacks;

    Bounds bounds;
    Health health;
    ShooterManager shooterManager;

    Vector3 targetPosition;

    [Header("FOR TESTING")]
    [SerializeField] bool bossReady = false;

    bool isSecondaryAttackActive = false;
    bool isSpecialAttackActive = false;

    float secondaryAttackTimer = 20f;

    float currentCombatDataTimer;
    float currentSpecialAttackTimer;

    float lastTimeShoot = 0f;

    const float specialAttackDuration = 5f;


    void Awake()
    {
        health = GetComponent<Health>();
        shooterManager = GetComponent<ShooterManager>();
        specialAttacks = GetComponents<ISpecialAttack>();
        currentCombatDataTimer = combatDataChangeCooldown;
        ResetSpecialAttackTimer();
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
        currentCombatData = defaultAttackData;
    }

    public void BossEnter(Vector3 endPosition)
    {
        StartCoroutine(BossEnterRoutine(endPosition));
    }

    IEnumerator BossEnterRoutine(Vector3 endPosition)
    {
        while (transform.position != endPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPosition, Time.deltaTime * movementSpeed);

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        targetPosition = transform.position;
        OnBossCombatReady?.Invoke();
        bossReady = true;
    }

    public override void HandleDamage()
    {
        if (!bossReady) { return; }

        // Boss attacks goes here
        if (Time.time > lastTimeShoot + currentCombatData.fireRate)
        {
            lastTimeShoot = Time.time;
            shooterManager.ShootProjectileByPattern(currentCombatData);
        }

        HandleCombatDataChange();
        HandleSpecialAttack();
    }

    void HandleCombatDataChange()
    {
        currentCombatDataTimer -= Time.deltaTime;
        if (currentCombatDataTimer <= 0 && !isSecondaryAttackActive)
        {
            // change combatdata
            CombatData randomSecondaryAttack = GetRandomSecondaryAttack();
            if (randomSecondaryAttack != null)
            {
                StartCoroutine(ChangeCombatDataRoutine(randomSecondaryAttack));
            }
        }
    }

    void HandleSpecialAttack()
    {
        currentSpecialAttackTimer -= Time.deltaTime;
        if (currentSpecialAttackTimer <= 0)
        {
            FireRandomSpecialAttack();
            Invoke(nameof(ResetSpecialAttackTimer), specialAttackDuration);
        }
    }

    void ResetSpecialAttackTimer()
    {
        currentSpecialAttackTimer = Random.Range(minSpecialAttackColldown, maxSpecialAttackCooldown);
    }

    IEnumerator ChangeCombatDataRoutine(CombatData attackData)
    {
        currentCombatData = attackData;
        isSecondaryAttackActive = true;

        yield return new WaitForSeconds(secondaryAttackTimer);

        currentCombatDataTimer = combatDataChangeCooldown;
        currentCombatData = defaultAttackData;
        isSecondaryAttackActive = false;
    }

    CombatData GetRandomSecondaryAttack()
    {
        if (secondaryAttacks.Count > 0)
        {
            int randomSecondaryAttackIndex = Random.Range(0, secondaryAttacks.Count);
            return secondaryAttacks[randomSecondaryAttackIndex];
        }
        return null;
    }
    void FireRandomSpecialAttack()
    {
        if (specialAttacks.Length > 0)
        {
            int randomSpecialAttackIndex = Random.Range(0, specialAttacks.Length);
            specialAttacks[randomSpecialAttackIndex].FireSpecialAttack();
        }
    }



    public override void Move()
    {
        if (!bossReady) { return; }

        // Boss movement goes here
        if (transform.position == targetPosition)// reach destination
        {
            targetPosition = new Vector3(Random.Range(bounds.min.x + 7, bounds.max.x - 7), transform.position.y, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }

    public override void TakeDamage(int damage)
    {
        if (!bossReady) { return; }
        health.TakeDamage(damage);
    }

    void Die()
    {
        OnBossDefeated?.Invoke();
        Destroy(gameObject);
    }
}
