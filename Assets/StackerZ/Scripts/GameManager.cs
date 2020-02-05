using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnCubeSpawned = delegate {  };
    [SerializeField]
    private CubeSpawner[] _spawners;

    private int _currentSpawnerIndex;

    private void Awake()
    {
        _currentSpawnerIndex = 0;
    }

    private void Start()
    {
        SpawnCube();
    }

    private void SpawnCube()
    {
        _spawners[_currentSpawnerIndex].SpawnCube();
        _currentSpawnerIndex++;
        if (_currentSpawnerIndex > _spawners.Length - 1)
        {
            _currentSpawnerIndex = 0;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (MovingCube.CurrentCube != null) {
                MovingCube.CurrentCube.Stop();
            }
            
            SpawnCube();
            OnCubeSpawned();

        }
    }
    
}