using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class ScreenShakeHandler : MonoBehaviour
{
    public static ScreenShakeHandler Instance { get; private set; }

    [SerializeField] bool start = false;

    float duration = 1f;
    Vector3 startPosition;
    ShakeSettingsSO shakeSettings;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            startPosition = transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (start && shakeSettings != null)
        {
            start = false;
            StartCoroutine(ShakeRoutine());
        }
    }

    public void ShakeCamera(ShakeSettingsSO shakeSettings)
    {
        start = true;
        this.shakeSettings = shakeSettings;
    }

    void CalculateRandomDirections(out Vector3[] randomDirections, int directions)
    {
        randomDirections = new Vector3[directions];
        for (int i = 0; i < directions; i++)
        {
            randomDirections[i] = Random.insideUnitSphere;
        }
    }

    IEnumerator ShakeRoutine()
    {
        float elapsedTime = 0;
        duration = shakeSettings.duration;
        Vector3[] randomDirections;
        CalculateRandomDirections(out randomDirections, 10);
        int randomDirectionIndex = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = shakeSettings.GetShakeStrength(elapsedTime);

            if (randomDirectionIndex >= randomDirections.Length)
            {
                randomDirectionIndex = 0;
            }

            transform.position = startPosition + randomDirections[randomDirectionIndex] * strength;
            randomDirectionIndex++;

            yield return null;
        }

        transform.position = startPosition;
    }
}
