using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    private int maxSpawn = 0;
    private int currentSpawnCount = 0;
    private void Start()
    {
        currentSpawnCount = 0;
        GameManager.OnCubeSpawned += GameManager_OnCubeSpawned;
    }

    private void OnDisable()
    {
        GameManager.OnCubeSpawned -= GameManager_OnCubeSpawned;
    }

    private void GameManager_OnCubeSpawned()
    {
        currentSpawnCount++;
        if (currentSpawnCount > maxSpawn)
        {
            currentSpawnCount = 0;
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
