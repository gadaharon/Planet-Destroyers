using UnityEngine;

public class TestLaser : MonoBehaviour
{
    ISpecialAttack[] specialAttacks;

    void Awake()
    {
        specialAttacks = GetComponents<ISpecialAttack>();
    }

    void Start()
    {
        FireRandomSpecialAttack();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FireRandomSpecialAttack();
        }
    }

    void FireRandomSpecialAttack()
    {
        if (specialAttacks.Length > 0)
        {
            int randomSpecialAttackIndex = Random.Range(0, specialAttacks.Length);
            specialAttacks[randomSpecialAttackIndex].FireSpecialAttack();
        }

    }
}
