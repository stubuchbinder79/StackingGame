using System;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }
    public static GameObject LastCube { get; private set; }
    public Direction MoveDirection { get; set; }
    
    [SerializeField] 
    private float moveSpeed = 1f;

    private float moveDirection = 1f;
    
    [SerializeField] 
    [Tooltip("% acceptable overhang for a perfect drop")]
    private float hangoverBuffer = 0.03f;

    [SerializeField]
    private bool isMoving  = false;

    private void Awake()
    {
        GameManager.OnGameState
    }

    private void OnDestroy()
    {
        
    }

    private void OnEnable()
    {
        if (LastCube == null)
            LastCube = GameObject.Find("Start");
        
        CurrentCube = this;

        GetComponent<Renderer>().material.color = GetRandomColor();


        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);;
        isMoving = true;
    }

    private Color GetRandomColor()
    {
        return  new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    private void Update()
    {
        if (isMoving)
        {
            if (MoveDirection == Direction.Z)
                transform.position += (moveSpeed * moveDirection) * Time.deltaTime * transform.forward;
            else
                transform.position += (moveSpeed * moveDirection) * Time.deltaTime * transform.right;
        }
        
    }

    public void Stop()
    {
        moveSpeed = 0;
        isMoving = false;

        Debug.LogFormat("Stop: {0}", LastCube.name);
        float hangover;

        if (MoveDirection == Direction.Z)
            hangover = transform.position.z - LastCube.transform.position.z;
        else
            hangover = transform.position.x - LastCube.transform.position.x;
        
        if (Mathf.Abs(hangover) < hangoverBuffer)
        {
            Debug.LogFormat("PERFECT drop");
            hangover = 0;
        }


        float max = MoveDirection == Direction.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;
        if (Mathf.Abs(hangover) >= max)
        {
            
            GameManager.Instance.GameOver();
            LastCube = null;
            CurrentCube = null;
            isMoving = false;
            Destroy(gameObject);
            return;
        }
        float direction = hangover > 0 ? 1f : -1f;
        
        if (MoveDirection == Direction.Z)
            SplitCubeOnZ(hangover, direction);
        else 
            SplitCubeOnX(hangover, direction);
        
        LastCube = this.gameObject;
    }
    

    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZSize;
        float newZPosition = LastCube.transform.position.z + (hangover / 2);
        
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockZPos = cubeEdge + fallingBlockSize / 2f * direction;
        
        SpawnDropCube(fallingBlockZPos, fallingBlockSize);
    }
    
    private void SplitCubeOnX(float hangover, float direction)
    {
        float newSize = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newSize;
        float newXPosition = LastCube.transform.position.x + (hangover / 2);
        
        transform.localScale = new Vector3(newSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newSize / 2f * direction);
        float fallingBlockXPos = cubeEdge + fallingBlockSize / 2f * direction;
        
        SpawnDropCube(fallingBlockXPos, fallingBlockSize);
    }
    

    private void SpawnDropCube(float fallingBlockPos, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (MoveDirection == Direction.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPos);
        }
        else
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockPos, transform.position.y, transform.position.z);

        }
 
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube.gameObject, 1f);
    }

    public void Bounce()
    {
        moveDirection = moveDirection * -1f;
    }
}
