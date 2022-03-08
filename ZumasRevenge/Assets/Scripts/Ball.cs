using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public string colorValue;
    public float groundBounds = 20;
    public GameObject[] checkPoints;
    public GameObject activeCheckPoint;

    public float movementSpeed = 10;
    public bool isShot = false;

    private GameManager gameManagerScript;
    public bool isInQue = false;

    public int gridValue;
    public int gridIndex;
    public int positionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroyOutOfBounds();
        MovementForward();
    }

    void DestroyOutOfBounds()
    {
        if(transform.position.x > groundBounds)
        {
            Destroy(gameObject);
        }
        else if(transform.position.x < -groundBounds)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z > groundBounds)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < -groundBounds)
        {
            Destroy(gameObject);
        }
    }

    public int GetPosition()
    {
        return positionIndex;
    }

    public void IncrementPosition()
    {
        positionIndex++;
    }

    public int GetNewPosition()
    {
        positionIndex++;
        return positionIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EndPoint") && isInQue)
        {
            GameManager.instance.RemoveFromActiveBalls(gameObject);
            Destroy(gameObject);
        }

        if(other.gameObject.CompareTag("Ball") && isShot && other.GetComponent<Ball>().isInQue)
        {
            Ball tempBallScript = other.gameObject.GetComponent<Ball>();
            GameManager.instance.BallHit(tempBallScript.gridIndex, gridValue);
            Destroy(gameObject);
        }
    }

    public void SetGameManager(GameManager gameManagerScript)
    {
        this.gameManagerScript = gameManagerScript;
    }

    void MovementForward()
    {
        if (isShot)
        {
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
    }

    public void SetGridIndex(int gridIndex)
    {
        this.gridIndex = gridIndex;
    }
}
