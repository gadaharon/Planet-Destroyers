using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    public static Action OnEnemyDeath;

    [SerializeField] bool lookAtPlayer = true;

    Transform player;

    // Knockback knockback;
    EnemyBase enemyBase;

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
    }

    void Start()
    {
        player = PlayerController.Instance.transform;
    }

    void Update()
    {
        enemyBase.Move();

        if (lookAtPlayer)
        {
            LookAtPlayer();
        }

        if (enemyBase.isRangeAttackEnemy)
        {
            enemyBase.HandleDamage();
        }
    }

    void LookAtPlayer()
    {
        float angleOffset = -90f;
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
    }

    public void TakeDamage(string damageDealerTag, int damage)
    {
        if (damageDealerTag != "Projectile_Enemy")
        {
            // I separated the take damage func so that the enemy and boss will have different functionalities
            // maybe the boss on certain amount of health start do something
            enemyBase.TakeDamage(damage);
        }
    }
}
