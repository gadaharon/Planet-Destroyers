using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentGameState => currentGameState;
    public Camera gameCamera { get; private set; }

    GameState currentGameState;

    protected override void Awake()
    {
        base.Awake();
        gameCamera = Camera.main;
    }

    void Start()
    {
        // Default gameState should be GameStarted, Change it after implement MainMenu
        SetGameState(GameState.Playing);
    }

    // TODO - set gameState and enable/disable game actions according to state
    public void SetGameState(GameState newState)
    {
        currentGameState = newState;
    }

    public Bounds GetCameraBounds()
    {
        if (!gameCamera.orthographic)
        {
            Debug.Log($"The camera {gameCamera.name} must be orthographic!");
            return new Bounds();
        }

        Transform cameraTransform = gameCamera.transform;
        float camXPosition = cameraTransform.position.x;
        float camYPosition = cameraTransform.position.y;
        float camSize = gameCamera.orthographicSize * 2;
        float width = camSize * (float)Screen.width / Screen.height;
        float height = camSize;

        return new Bounds(new Vector3(camXPosition, camYPosition, 0), new Vector3(width, height, 0));
    }
}
