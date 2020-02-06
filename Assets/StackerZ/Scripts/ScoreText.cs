using System;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private int score;
    private TMP_Text text;
    
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        GameManager.OnCubeSpawned += GameManager_OnCubeSpawned;
        GameManager.OnStartGame += GameManager_OnStartGame;
    }

    private void OnDestroy()
    {
        GameManager.OnCubeSpawned -= GameManager_OnCubeSpawned;
    }

    private void GameManager_OnCubeSpawned()
    {
        score++;
        text.text = score.ToString();
    }

    private void GameManager_OnStartGame()
    {
        score = 0;
    }
}
