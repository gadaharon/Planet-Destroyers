using System;
using UnityEngine;

public class EnemyBommerr : EnemyBase
{
    public static Action<string, int> OnEnemyExplode;

    [Tooltip("Determine the speed increase when reach combat state distance")]
    [SerializeField] float speedMultiplier = 4f;

    [Tooltip("Determine the distance to switch to combat state")]
    [SerializeField] float combatStateDistance = 20f;

    [Tooltip("Determine the distance to deal damage to player")]
    [SerializeField] float explosionDistance = 2f;

    [Tooltip("Populate the damage")]
    [SerializeField] int damage = 20;

    [Tooltip("Populate the explosion VFX")]
    [SerializeField] ParticleSystem explodeVFX;

    [Tooltip("Populate the screen shake for the explosion")]
    [SerializeField] ShakeSettingsSO explosionShakeSettings;

    Bounds bounds;
    Transform player;
    Health health;
    Animator animator;
    Flash flash;
    int ATTACK_ANIMATION_HASH = Animator.StringToHash("IsAttacking");

    void Awake()
    {
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
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
        player = PlayerController.Instance.transform;
    }

    public override void HandleDamage()
    { }

    Vector2 MoveTowardsClamp(Vector2 position)
    {
        position.x = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
        position.y = Mathf.Clamp(position.y, bounds.min.y, bounds.max.y);

        return position;
    }

    public override void Move()
    {
        // Move towards player
        Vector2 position;

        if (Vector2.Distance(transform.position, player.position) < combatStateDistance)
        {
            position = Vector2.MoveTowards(transform.position, player.position, movementSpeed * speedMultiplier * Time.deltaTime);
            animator.SetBool(ATTACK_ANIMATION_HASH, true);
            // Change animation state
        }
        else
        {
            position = Vector2.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
        }
        transform.position = MoveTowardsClamp(position);

        if (Vector2.Distance(transform.position, player.position) < explosionDistance)
        {
            OnEnemyExplode?.Invoke(gameObject.tag, damage);
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explodeVFX, transform.position, Quaternion.identity);
        ScreenShakeHandler.Instance.ShakeCamera(explosionShakeSettings);
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        flash.StartFlash();
        health.TakeDamage(damage);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
