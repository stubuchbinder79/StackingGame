using System;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private MovingCube cubePrefab;

    [SerializeField]
    private Direction moveDirection;

    private void Awake()
    {
        GameManager.OnCubeSpawned += GameManager_OnCubeSpawned;
    }

    private void OnDestroy()
    {
        GameManager.OnCubeSpawned -= GameManager_OnCubeSpawned;
    }

    void GameManager_OnCubeSpawned()
    {
        if (MovingCube.LastCube != null &&
            MovingCube.LastCube.gameObject != GameObject.Find("Start"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        }
    }
    public void SpawnCube()
    {
        var cube =  Instantiate(cubePrefab);
        
        if (MovingCube.LastCube != null && 
            MovingCube.LastCube.gameObject != GameObject.Find("Start"))
        {
            float x = moveDirection == Direction.X ? transform.position.x : MovingCube.LastCube.transform.position.x;
            float z = moveDirection == Direction.Z ? transform.position.z : MovingCube.LastCube.transform.position.z;
            
            cube.transform.position = new Vector3(x, MovingCube.LastCube.transform.position.y + cubePrefab.transform.localScale.y, z: z);
            
        } else
        {
            cube.transform.position = transform.position;
        }
        
        cube.MoveDirection = moveDirection;
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }
}
