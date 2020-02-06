using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private int _currentSpawnIndex = 0;
    
    [SerializeField]
    [Tooltip("number of squares (0 based) that have to spawn until the camera will move")]
    private int maxSpawnIndex = 0;
    private void Start()
    {
        _currentSpawnIndex = 0;
        GameManager.OnCubeSpawned += GameManager_OnCubeSpawned;
    }

    private void OnDisable()
    {
        GameManager.OnCubeSpawned -= GameManager_OnCubeSpawned;
    }

    private void GameManager_OnCubeSpawned()
    {
        _currentSpawnIndex++;
        if (_currentSpawnIndex > maxSpawnIndex)
        {
            _currentSpawnIndex = 0;
            
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            StartCoroutine(LerpToPosition(0.2f, newPosition));
        }
    }

    IEnumerator LerpToPosition(float lerpSpeed, Vector3 newPosition)
    {
        float t = 0f;
        Vector3 startPos = transform.position;

        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / lerpSpeed);
            transform.position = Vector3.Lerp(startPos, newPosition, t);
            yield return 0;
        }
    }
}
