using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float movementSpeed = 5f;
    public bool isRangeAttackEnemy = true;


    abstract public void Move();

    abstract public void HandleDamage();

    abstract public void TakeDamage(int damage);
}
