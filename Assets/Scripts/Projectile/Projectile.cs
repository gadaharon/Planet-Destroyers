using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 0f;
    Vector2 moveDirection = Vector2.zero;
    int damage;

    float defaultProjectileSize = 1f;


    Bounds bounds;
    Transform t;

    void Awake()
    {
        t = transform;
    }

    void Start()
    {
        bounds = GameManager.GetCameraBounds();
    }

    void Update()
    {
        ProcessProjectileBounds();
        Move();
    }

    public void Init(Vector2 moveDirection, float speed, int damage, float size)
    {
        this.moveDirection = moveDirection;
        this.speed = speed;
        this.damage = damage;
        SetProjectileSize(size);
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
            other.gameObject.GetComponent<IDamagable>().TakeDamage(gameObject.tag, 10);
            ReleaseProjectile();
        }
        else if (!IsHittingSelf(other.gameObject.tag, gameObject.tag) && !other.CompareTag("Untagged"))
        {
            ReleaseProjectile();
        }
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
