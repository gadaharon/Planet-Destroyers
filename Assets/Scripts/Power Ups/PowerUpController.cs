using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public static PowerUpController Instance { get; private set; }

    Animator animator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            animator = GetComponent<Animator>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        EnemySpawner.OnWaveChanged += ShowPowerUpsMenu;
    }

    void OnDisable()
    {
        EnemySpawner.OnWaveChanged -= ShowPowerUpsMenu;
    }

    void ShowPowerUpsMenu(EnemySpawner sender)
    {
        if (sender.CurrentWaveIndex > 0)
        {
            animator.SetBool("isPowerUpsAvailable", true);
        }
    }

    void HidePowerUpMenu()
    {
        animator.SetBool("isPowerUpsAvailable", false);
    }

    public void HandleSelectedPowerUp(string powerUp)
    {
        switch (powerUp)
        {
            case "PowerUp_Spread":
                PlayerController.Instance.AddBulletSpread();
                break;
            case "PowerUp_Health":
                PlayerController.Instance.AddHealth();
                break;
            case "PowerUp_Shield":
                PlayerController.Instance.ActivateShield();
                break;
            default:
                Debug.Log($"Not valid powerup - {powerUp} ");
                break;
        }
        HidePowerUpMenu();
    }
}
