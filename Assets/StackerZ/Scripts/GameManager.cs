using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Init, Play, Paused, GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action OnStartGame = delegate { };
    public static event Action OnCubeSpawned = delegate { };
    public static event Action<GameState> OnGameState = delegate { };

    private GameState _gameState;
    public  GameState GameState
    {
        get => _gameState;
        set {
            _gameState = value;
            OnGameState(value);
        }
    }

    [SerializeField]
    private CubeSpawner[] spawners;

    private int _currentSpawnerIndex;

    private void Awake()
    {
        Instance = this;
        
        spawners = FindObjectsOfType<CubeSpawner>();
        _currentSpawnerIndex = 0;

        MovingCube.OnGameOver += MovingCube_OnGameOver;
    }

    private void Start()
    {
        GameState = GameState.Init;
    }

    private void StartGame()
    {
        GameState = GameState.Play;
        
        SpawnCube();
        OnStartGame();
    }

    private void SpawnCube()
    {
        spawners[_currentSpawnerIndex].SpawnCube();
        _currentSpawnerIndex++;
        if (_currentSpawnerIndex > spawners.Length - 1)
        {
            _currentSpawnerIndex = 0;
        }
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {

            switch (GameState)
            {
                case GameState.Init:
                    StartGame();
                    break;
                case GameState.Play:
                    if (MovingCube.CurrentCube != null) {
                        MovingCube.CurrentCube.Stop();
                    }
                    SpawnCube();
                    OnCubeSpawned();
                    break;

                case GameState.GameOver:
                    SceneManager.LoadScene(0);
                    break;
            }
        }
    }

    private void MovingCube_OnGameOver()
    {
        GameState = GameState.GameOver;
    }

    public void GameOver()
    {
        GameState = GameState.GameOver;
    }

}