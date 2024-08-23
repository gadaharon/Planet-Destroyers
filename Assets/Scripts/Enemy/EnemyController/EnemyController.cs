using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    public static Action OnEnemyDeath;

    [SerializeField] bool lookAtPlayer = true;
    [SerializeField] Transform sprite;

    Transform player;

    // Knockback knockback;
    EnemyBase enemyBase;
    bool isFacingRight = false;

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
            FlipSprite();
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



    void FlipSprite()
    {
        if (sprite == null) { return; }

        if (transform.position.x < player.position.x)
        {
            isFacingRight = true;
            sprite.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (transform.position.x > player.position.x)
        {
            isFacingRight = false;
            sprite.rotation = Quaternion.Euler(0, 0, 0);
        }
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
