using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Collectible_Health : MonoBehaviour, ICollectible
{
    [SerializeField] int healthAmount = 10;
    [SerializeField] float lifeTime = 10f;
    [SerializeField] float detectionOffset = 3f;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Collect()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.AddHealth(healthAmount);
            Destroy(gameObject);
            // TODO - Play Collect VFX 
            // TODO - Play Collect Sound
        }
    }
}
