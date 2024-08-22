using UnityEngine;

public class ShootngPatternChanger : MonoBehaviour
{
    [SerializeField] ShootingPattern shootingPattern;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerCombat>() != null)
        {
            other.GetComponent<PlayerCombat>().ChangeShootingPattern(shootingPattern);
        }
    }
}
