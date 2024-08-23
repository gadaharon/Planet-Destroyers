using UnityEngine;

[CreateAssetMenu(fileName = "ShakeSettings_", menuName = "Scriptable Objects/Screen Shake/New Shake Settings")]
public class ShakeSettingsSO : ScriptableObject
{
    [Tooltip("The duration of the screen shake")]
    public float duration;

    [Tooltip("Determine the strength curve of the shake")]
    [SerializeField] AnimationCurve shakeStrengthCurve;

    public float GetShakeStrength(float time)
    {
        return shakeStrengthCurve.Evaluate(time / duration);
    }
}
