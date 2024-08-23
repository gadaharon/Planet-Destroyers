using System.Reflection.Emit;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Missile : MonoBehaviour
{
    [SerializeField] GameObject explosionVFX;

    [Tooltip("Determine the screen shake details when missile explodes")]
    [SerializeField] ShakeSettingsSO shakeSettings;

    GameObject target;
    GameObject targetCrosshair;
    Rigidbody2D rb;

    float speed = 25f;
    float rotateSpeed = 400f;
    int damage = 15;

    float hitThreshold = 3f;
    float hitThresholdSpeed = 2f;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.AddForce(Vector2.right * speed * 100, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (targetCrosshair != null && target != null)
        {
            targetCrosshair.transform.position = target.transform.position;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            HandleHit();
            return;
        }

        Vector2 direction = (Vector2)target.transform.position - rb.position;

        float distance = direction.magnitude;

        // Stop rotating when close enough to the target
        if (distance < hitThreshold)
        {
            rb.velocity = direction.normalized * hitThresholdSpeed;
            HandleHit();
        }
        else
        {
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;

            rb.velocity = transform.up * speed;
        }
    }

    public void Initialize(GameObject target, GameObject targetCrosshair)
    {
        this.target = target;
        this.targetCrosshair = targetCrosshair;
    }

    void HandleHit()
    {
        ScreenShakeHandler.Instance.ShakeCamera(shakeSettings);
        if (target != null)
        {
            Instantiate(explosionVFX, target.transform.position, Quaternion.identity);
            target.GetComponent<IDamagable>()?.TakeDamage("Player", damage);
        }
        else
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }
        if (targetCrosshair != null)
        {
            Destroy(targetCrosshair);
        }
        Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(target.tag))
        {
            HandleHit();
        }
    }


}
