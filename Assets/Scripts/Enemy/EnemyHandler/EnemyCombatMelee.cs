using UnityEngine;

public class EnemyCombatMelee : EnemyBase
{
    [SerializeField] int damage = 3;
    Knockback knockback;
    Health health;

    void Awake()
    {
        knockback = GetComponent<Knockback>();
        health = GetComponent<Health>();
    }

    void OnEnable()
    {
        health.OnDeath += Die;
    }

    void OnDisable()
    {
        health.OnDeath -= Die;
    }

    public override void HandleDamage() { }

    public override void Move()
    {
        if (!knockback.isKnockedBack)
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, movementSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (knockback != null)
            {
                // check if not projectile
                if (other.gameObject.GetComponent<Projectile>() == null)
                {
                    Vector3 directionToPlayer = (transform.position - PlayerController.Instance.transform.position).normalized;
                    knockback.GetKnockBack(directionToPlayer, 20f);
                    other.gameObject.GetComponent<IDamagable>().TakeDamage(gameObject.tag, damage);
                }
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }

    void Die()
    {
        EnemyController.OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }
}
