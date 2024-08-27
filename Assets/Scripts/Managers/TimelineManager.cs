using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : Singleton<TimelineManager>
{
    [Header("PLANETS GAMEOBJECTS SPRITE RENDERERS")]
    [SerializeField] SpriteRenderer planet1SR;
    [SerializeField] SpriteRenderer planet2SR;
    [SerializeField] SpriteRenderer planet3SR;

    [Header("SPEED PARAMETERS")]
    [SerializeField] ParticleSystem speedVFX;
    [SerializeField] ShakeSettingsSO speedScreenShakeSettings;

    PlayableDirector playableDirector;

    protected override void Awake()
    {
        base.Awake();
        playableDirector = GetComponent<PlayableDirector>();
    }

    public void PlayCutscene()
    {
        GameManager.Instance.SetGameState(GameState.Cutscene);
        playableDirector.Play();
    }

    public void ChangePlanetSprite()
    {
        planet1SR.sprite = GameResources.Instance.Planet_2_1;
        planet2SR.sprite = GameResources.Instance.Planet_2_2;
        planet3SR.sprite = GameResources.Instance.Planet_2_3;
    }

    public void StartSpeedAnimation()
    {
        speedVFX.Play();
        ScreenShakeHandler.Instance.ShakeCamera(speedScreenShakeSettings);
        // TODO - Add player ship boost : fire VFX - need to create one and apply to player
    }

    public void EndCutsceneHandler()
    {
        GameManager.Instance.SetGameState(GameState.Playing);
        EnemySpawner.Instance.StartNextLevel();
    }
}
