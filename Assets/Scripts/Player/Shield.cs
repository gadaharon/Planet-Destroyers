using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool hasShield = false;
    [SerializeField] int shieldDurabilityAmount = 50;
    [SerializeField] GameObject shieldGO;

    int currentShieldDurabilityAmount;

    public void ActivateShield()
    {
        ToggleShield(true);
        currentShieldDurabilityAmount = shieldDurabilityAmount;
        UIManager.Instance.SetPlayerShieldSliderMaxValue(shieldDurabilityAmount);
    }

    void ToggleShield(bool isEnabled)
    {
        hasShield = isEnabled;
        shieldGO.SetActive(isEnabled);
        UIManager.Instance.TogglePlayerShield(isEnabled);
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Alpha1) && !hasShield)
    //     {
    //         ActivateShield();
    //     }
    // }

    public void TakeDamage(int amount)
    {
        currentShieldDurabilityAmount -= amount;
        if (currentShieldDurabilityAmount <= 0)
        {
            ToggleShield(false);
            return;
        }
        UIManager.Instance.UpdatePlayerShieldSliderValue(currentShieldDurabilityAmount);
    }
}
