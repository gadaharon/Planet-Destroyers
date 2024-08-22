using UnityEngine;

public class LaserCanon : MonoBehaviour
{
    [SerializeField] float distanceRay = 50f;
    [SerializeField] Transform laserFirePosition;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] int laserDamage = 10;
    [SerializeField] ParticleSystem laserImpactVFX;

    Transform t;
    LayerMask layer;
    int layerMask;

    float shootOffset = .2f;
    float lastShootTime = 0f;

    void Awake()
    {
        t = transform;
    }

    void Start()
    {
        layer = LayerMask.NameToLayer("Enemy");
        layerMask = 1 << layer.value;
    }

    public void ShootLaser()
    {
        if (Physics2D.Raycast(t.position, t.up, distanceRay, layerMask))
        {
            RaycastHit2D hit = Physics2D.Raycast(laserFirePosition.position, t.up, distanceRay, layerMask);
            PlayLaserImpactVFX(hit.distance);
            Draw2DRay(laserFirePosition.position, hit.point);

            if (hit.collider.gameObject.GetComponent<IDamagable>() != null)
            {
                if (Time.time > lastShootTime + shootOffset)
                {
                    lastShootTime = Time.time;
                    hit.collider.gameObject.GetComponent<IDamagable>().TakeDamage("Player", laserDamage);
                }
            }
        }
        else
        {
            Draw2DRay(laserFirePosition.position, laserFirePosition.transform.up * distanceRay);
            if (laserImpactVFX.isPlaying)
            {
                laserImpactVFX.Stop();
            }
        }
    }

    void PlayLaserImpactVFX(float distance)
    {
        if (laserImpactVFX.isPlaying) { return; }
        Vector2 position = new Vector2(0, distance + 1);
        laserImpactVFX.gameObject.transform.localPosition = position;
        laserImpactVFX.Play();
    }

    void Draw2DRay(Vector2 startPosition, Vector2 endPosition)
    {
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }
}
