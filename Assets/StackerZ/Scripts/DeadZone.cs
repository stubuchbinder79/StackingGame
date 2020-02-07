using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DeadZone : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    private void Awake()
    {
        GameManager.OnCubeSpawned += GameManager_OnCubeSpawned;
    }

    private void OnDestroy()
    {
        GameManager.OnCubeSpawned -= GameManager_OnCubeSpawned;
    }

    private void GameManager_OnCubeSpawned()
    {
        if (MovingCube.LastCube != null &&
            MovingCube.LastCube.gameObject != GameObject.Find("Start"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.LogFormat("on trigger exit");
        GameManager.Instance.GameOver();
    }
}
