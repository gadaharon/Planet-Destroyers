using UnityEngine;

public class LaserCanon : MonoBehaviour
{
    [SerializeField] float distanceRay = 50f;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] int laserDamage = 10;
    [SerializeField] ParticleSystem laserImpactVFX;

    Transform t;
    Transform laserCanonTransform;
    int layerMask; // target layer to hit
    string damageDealerTag;

    float shootOffset = .2f;
    float lastShootTime = 0f;

    void Awake()
    {
        t = transform;
    }

    public void Init(string targetLayerName, string damageDealerTag, Transform laserCanon)
    {
        LayerMask layer = LayerMask.NameToLayer(targetLayerName);
        layerMask = 1 << layer.value;
        this.damageDealerTag = damageDealerTag;
        laserCanonTransform = laserCanon;
    }

    public void ShootLaser()
    {
        t.position = laserCanonTransform.position;
        if (Physics2D.Raycast(t.position, t.up, distanceRay, layerMask))
        {
            RaycastHit2D hit = Physics2D.Raycast(t.position, t.up, distanceRay, layerMask);
            PlayLaserImpactVFX(hit.point.y);
            Draw2DRay(t.position, new Vector2(t.position.x, hit.point.y));

            if (hit.collider.gameObject.GetComponent<IDamagable>() != null)
            {
                if (Time.time > lastShootTime + shootOffset)
                {
                    lastShootTime = Time.time;
                    hit.collider.gameObject.GetComponent<IDamagable>().TakeDamage(damageDealerTag, laserDamage);
                }
            }
        }
        else
        {
            Draw2DRay(t.position, new Vector2(t.position.x, t.up.y * distanceRay));
            if (laserImpactVFX.isPlaying)
            {
                laserImpactVFX.Stop();
            }
        }
    }

    void PlayLaserImpactVFX(float distance)
    {
        if (laserImpactVFX.isPlaying) { return; }
        Vector2 position = new Vector2(t.position.x, distance);
        laserImpactVFX.gameObject.transform.position = position;
        laserImpactVFX.Play();
    }

    void Draw2DRay(Vector2 startPosition, Vector2 endPosition)
    {
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }
}
