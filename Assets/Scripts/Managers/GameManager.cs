using UnityEngine;

public enum GameState
{
    GameStarted,
    CharacterSelection,
    Playing,
    Cutscene,
    Pause,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static Camera gameCamera;

    GameState gameState;



    void Awake()
    {
        gameCamera = Camera.main;
    }

    // TODO - set gameState and enable/disable game actions according to state
    public void SetGameState(GameState newState)
    {
        gameState = newState;
    }

    public static Bounds GetCameraBounds()
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
