using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : Singleton<TimelineManager>
{
    [Header("PLANETS GAMEOBJECTS SPRITE RENDERERS")]
    [SerializeField] SpriteRenderer planet1SR;
    [SerializeField] SpriteRenderer planet2SR;
    [SerializeField] SpriteRenderer planet3SR;

    PlayableDirector playableDirector;

    protected override void Awake()
    {
        base.Awake();
        playableDirector = GetComponent<PlayableDirector>();
    }

    public void PlayCutscene()
    {
        playableDirector.Play();
    }

    public void ChangePlanetSprite()
    {
        planet1SR.sprite = GameResources.Instance.Planet_2_1;
        planet2SR.sprite = GameResources.Instance.Planet_2_2;
        planet3SR.sprite = GameResources.Instance.Planet_2_3;
    }
}
