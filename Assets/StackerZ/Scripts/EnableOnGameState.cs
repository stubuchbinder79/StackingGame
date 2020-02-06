using UnityEngine;

[System.Serializable]
public struct GameStateEnabler
{
    public GameState gameState;
    public bool enabled;

    public GameStateEnabler(GameState state, bool enable)
    {
        gameState = state;
        enabled = enable;
    }
}
public class EnableOnGameState : MonoBehaviour
{
    public GameStateEnabler InitEnabler = new GameStateEnabler(GameState.Init, true);
    public GameStateEnabler PlayEnabler = new GameStateEnabler(GameState.Play, enable: false);
    public GameStateEnabler PausedEnabler = new GameStateEnabler(GameState.Paused, enable: false);
    public GameStateEnabler GameOverEnabler = new GameStateEnabler(GameState.GameOver, enable: false);
    
    private void Awake()
    {
        GameManager.OnGameState += GameManager_OnGameState;
    }

    private void GameManager_OnGameState(GameState state)
    {
        
        switch (state)
        {
                case GameState.Init:
                    gameObject.SetActive(InitEnabler.enabled);

                    break;
                case GameState.Paused:
                    gameObject.SetActive(PausedEnabler.enabled);

                    break;
                case GameState.Play:
                    gameObject.SetActive(PlayEnabler.enabled);

                    break;
                case GameState.GameOver:
                    gameObject.SetActive(GameOverEnabler.enabled);
                    break;
                
        }
    }
}
