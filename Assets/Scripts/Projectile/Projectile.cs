using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Populate here the particle system for the projectile hit VFX")]
    [SerializeField] ParticleSystem projectileHitVFXPrefab;

    float speed = 0f;
    Vector2 moveDirection = Vector2.zero;
    int damage;
    ProjectileSettings projectileSettings;

    float defaultProjectileSize = 1f;

    ParticleSystem hitVFX;
    Bounds bounds;
    Transform t;

    void Awake()
    {
        t = transform;
    }

    void Start()
    {
        bounds = GameManager.Instance.GetCameraBounds();
    }

    void Update()
    {
        ProcessProjectileBounds();
        Move();
    }

    public void Init(Vector2 moveDirection, ProjectileSettings projectileSettings)
    {
        this.moveDirection = moveDirection;
        this.projectileSettings = projectileSettings;
        speed = projectileSettings.projectileSpeed;
        damage = projectileSettings.damage;
        SetProjectileSize(projectileSettings.projectileSize);
        SetProjectileColor(projectileSettings.projectileColor);
    }

    void Move()
    {
        t.Translate(moveDirection * speed * Time.deltaTime);
    }

    void ProcessProjectileBounds()
    {
        if (t.position.x < bounds.min.x || t.position.x > bounds.max.x || t.position.y < bounds.min.y || t.position.y > bounds.max.y)
        {
            ReleaseProjectile();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // if I not hitting my self
        if (other.gameObject.GetComponent<IDamagable>() != null && !IsHittingSelf(other.gameObject.tag, gameObject.tag))
        {
            InitHitVFX();
            other.gameObject.GetComponent<IDamagable>().TakeDamage(gameObject.tag, damage);
            ReleaseProjectile();
        }
        else if (!IsHittingSelf(other.gameObject.tag, gameObject.tag) && !other.CompareTag("Untagged"))
        {
            InitHitVFX();
            ReleaseProjectile();
        }
    }

    void InitHitVFX()
    {
        hitVFX = Instantiate(projectileHitVFXPrefab, t.position, t.rotation);
        var main = hitVFX.main;
        main.startSizeMultiplier = projectileSettings.projectileSize * main.startSize.constant;
        hitVFX.gameObject.GetComponent<ParticleSystemRenderer>().material = projectileSettings.projectileColor;
        hitVFX.Play();
    }

    void SetProjectileColor(Material material)
    {
        gameObject.GetComponent<SpriteRenderer>().material = material;
    }

    void SetProjectileSize(float size)
    {
        if (t.localScale.x != size)
        {
            t.localScale = Vector3.one * size;
        }
    }

    bool IsHittingSelf(string tag, string self)
    {
        // Remove Projectile_ prefix to check projectiles collisions Projectile_Enemy and Projectile_Player
        string tagToCheck = self.Replace("Projectile_", "");
        // Debug.Log($"OLD: {self}, NEW: {tagToCheck}, {tag} Contains {tagToCheck}? {tag.Contains(tagToCheck)}");
        return tag.Contains(tagToCheck);
    }

    void ReleaseProjectile()
    {
        gameObject.tag = "Projectile";
        SetProjectileSize(defaultProjectileSize);
        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }
}
