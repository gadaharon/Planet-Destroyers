using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("PLAYER HEALTH BAR SLIDER")]
    [SerializeField] Slider playerHealthBarSlider;

    [Header("PLAYER SHIELD BAR SLIDER")]
    [SerializeField] Slider playerShieldBarSlider;

    [Header("SPECIAL ATTACK SLIDER")]
    [SerializeField] Slider specialAttackSlider;

    [Header("SPECIAL ATTACK IS FULL INDICATOR GAMEOBJECT")]
    [SerializeField] GameObject specialAttackIsFullGO;

    [Header("SPECIAL ATTACK IS FULL INDICATOR ACTIVE VFX")]
    [SerializeField] ParticleSystem specialAttackFullVFX;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Player Health Slider
    public void SetPlayerHealthSliderMaxValue(int maxValue)
    {
        playerHealthBarSlider.maxValue = maxValue;
        playerHealthBarSlider.value = maxValue;
    }

    public void UpdatePlayerHealthSliderValue(int value)
    {
        playerHealthBarSlider.value = value;
    }

    // Player Shield Slider
    public void TogglePlayerShield(bool isActive)
    {
        playerShieldBarSlider.gameObject.SetActive(isActive);
    }

    public void SetPlayerShieldSliderMaxValue(int maxValue)
    {
        playerShieldBarSlider.maxValue = maxValue;
        playerShieldBarSlider.value = maxValue;
    }

    public void UpdatePlayerShieldSliderValue(int value)
    {
        playerShieldBarSlider.value = value;
    }

    // Special Attack Slider
    public void SetSpecialAttackSliderMaxValue(int value)
    {
        specialAttackSlider.maxValue = value;
        specialAttackSlider.value = 0;
    }

    public void UpdateSpecialAttackSliderValue(int value)
    {
        specialAttackSlider.value = value;
    }

    public void ToggleSpecialAttackIsFullIndicator(bool setActive)
    {
        if (!specialAttackIsFullGO.activeSelf && setActive)
        {
            specialAttackIsFullGO.SetActive(true);
            specialAttackFullVFX.Play();
        }
        else if (!setActive)
        {
            specialAttackIsFullGO.SetActive(false);
            specialAttackFullVFX.Stop();
        }
    }
}
